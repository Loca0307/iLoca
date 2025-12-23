import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Login() {
  const username = localStorage.getItem("loggedInUsername");
  const emailStored = localStorage.getItem("loggedInEmail");

  const [statusMessage, setStatusMessage] = useState("");
  const [statusType, setStatusType] = useState(""); // "success" or "error"
  const [clientData, setClientData] = useState({ iban: "", balance: 0 });

  const navigate = useNavigate();

  // Load client info if already logged in
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

    setStatusMessage("");
    setStatusType("");

    try {
      const response = await fetch("http://localhost:5027/login/authenticate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
      });

      if (!response.ok) {
        const error = await response.json();
        setStatusType("error");
        setStatusMessage(error.message || "Invalid email or password");
        return;
      }

      setStatusType("success");
      setStatusMessage("Login successful! Redirecting...");
      localStorage.setItem("loggedInEmail", email);

      // Fetch username
      try {
        const res = await fetch(
          `http://localhost:5027/login/GetUsernameByEmail?email=${encodeURIComponent(email)}`
        );

        if (res.ok) {
          const username = await res.text();
          if (username) localStorage.setItem("loggedInUsername", username);
        }
      } catch {
        console.warn("Username fetch failed");
      }

      setTimeout(() => navigate("/BankuumTubo"), 1200);
    } catch {
      setStatusType("error");
      setStatusMessage("Network error. Please try again.");
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

      {/* STATUS MESSAGE LIKE REGISTER */}
      {statusMessage && (
        <p
          style={{
            marginTop: "10px",
            color: statusType === "success" ? "green" : "red",
            fontWeight: "bold",
            textAlign: "center"
          }}
        >
          {statusMessage}
        </p>
      )}

      {!username ? (
        <form id="login-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email Address</label>
            <input type="email" id="email" placeholder="Enter your email"
                   autoComplete="email" required />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input type="password" id="password"
                   placeholder="Enter your password"
                   autoComplete="current-password" required />
          </div>

          <button type="submit" className="btn-login">Login</button>

          <div className="signup-link">
            Don’t have an account? <Link to="/BankuumTubo/register">Register</Link>
          </div>
        </form>
      ) : (
        <div style={{ marginTop: "1rem", textAlign: "center" }}>
          <p style={{ fontWeight: 600 }}>Logged in as {username}</p>
          {emailStored && <p>Email: {emailStored}</p>}
          {clientData.iban && <p>IBAN: {clientData.iban}</p>}
          <p style={{ color: "#2b7a0b", fontSize: "1.3rem", fontWeight: 700 }}>
            Balance: {clientData.balance.toFixed(2)} €
          </p>

          <button className="btn-login"
                  style={{ background: "#e53935" }}
                  onClick={handleLogout}>
            Logout
          </button>
        </div>
      )}
    </div>
  );
}
