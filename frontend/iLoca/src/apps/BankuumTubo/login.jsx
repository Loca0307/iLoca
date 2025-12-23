import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Login() {
  const [username] = useState(() => localStorage.getItem("loggedInUsername"));
  const [emailStored] = useState(() => localStorage.getItem("loggedInEmail"));

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [clientData, setClientData] = useState({ iban: "", balance: 0 });

  const navigate = useNavigate();

  useEffect(() => {
    if (!username || !emailStored) return;

    (async () => {
      try {
        const resp = await fetch(
          `http://localhost:5027/client/GetClientByEmail?email=${encodeURIComponent(emailStored)}`
        );

        if (resp.ok) {
          const client = await resp.json();
          setClientData({
            iban: client?.iban || "",
            balance: Number(client?.balance) || 0
          });
        }
      } catch {
        alert("Error authenticating client");
      }
    })();
  }, [username, emailStored]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const email = event.target.email.value;
    const password = event.target.password.value;

    setErrorMessage("");
    setSuccessMessage("");

    try {
      const response = await fetch("http://localhost:5027/login/authenticate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
      });

      if (!response.ok) {
        const error = await response.json();
        setErrorMessage(error.message || "Invalid email or password");
        return;
      }

      setSuccessMessage("Login successful! Redirecting...");
      localStorage.setItem("loggedInEmail", email);

      try {
        const res = await fetch(
          `http://localhost:5027/login/GetUsernameByEmail?email=${encodeURIComponent(email)}`
        );

        if (res.ok) {
          const text = await res.text();
          let fetchedUsername = text;

          try {
            const json = JSON.parse(text);
            fetchedUsername = json?.username ?? json;
          } catch {
            // ignore for now
          }

          if (fetchedUsername) {
            localStorage.setItem("loggedInUsername", fetchedUsername);
          }
        }
      } catch {
        console.warn("Username fetch failed");
      }

      setTimeout(() => navigate("/BankuumTubo"), 1200);
    } catch {
      setErrorMessage("Network error. Please try again.");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("loggedInUsername");
    localStorage.removeItem("loggedInEmail");
    navigate("/BankuumTubo");
  };

  return (
    <div className="auth-container">
      <div className="auth-header">
        <h2>Welcome Back</h2>
        <p>Login to your Bankuum Tubo account</p>
      </div>

      {errorMessage && (
        <div id="error-message" className="error-message">
          {errorMessage}
        </div>
      )}
      {successMessage && (
        <div id="success-message" className="success-message">
          {successMessage}
        </div>
      )}

      {!username ? (
        <form id="login-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email Address</label>
            <input
              type="email"
              id="email"
              name="email"
              placeholder="Enter your email"
              autoComplete="email"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              placeholder="Enter your password"
              autoComplete="current-password"
              required
            />
          </div>

          <button type="submit" className="btn-login">
            Login
          </button>

          <div id="signup-link" className="signup-link">
            Don't have an account? <Link to="/BankuumTubo/register">Register</Link>
          </div>
        </form>
      ) : (
        <div
          id="profile-section"
          style={{ display: "block", marginTop: "1rem", textAlign: "center" }}
        >
          <p style={{ fontWeight: 600, color: "#333" }}>
            Logged in as {username}
          </p>
          {emailStored && <p style={{ color: "#666" }}>Email: {emailStored}</p>}
          {clientData.iban && (
            <p style={{ fontWeight: 600 }}>IBAN: {clientData.iban}</p>
          )}
          <p
            style={{
              color: "#2b7a0b",
              fontSize: "1.3rem",
              fontWeight: 700
            }}
          >
            Balance: {clientData.balance.toFixed(2)} €
          </p>

          <button
            className="btn-login"
            style={{
              background: "#e53935",
              border: "none",
              padding: "0.6rem 1.2rem",
              color: "#fff",
              cursor: "pointer",
              borderRadius: "6px"
            }}
            onClick={handleLogout}
          >
            Logout
          </button>
        </div>
      )}
    </div>
  );
}
