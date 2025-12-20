import { useState } from "react";

export default function Register() {
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const username = (e.target.username.value || "").trim();
    const email = (e.target.email.value || "").trim();
    const password = (e.target.password.value || "").trim();

    setErrorMessage("");
    setSuccessMessage("");

    if (!username || !email || !password) {
      setErrorMessage("Please fill in username, email and password.");
      return;
    }

    try {
      const response = await fetch("http://localhost:5027/login/InsertLogin", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password, username }),
      });

      if (response.ok) {
        setSuccessMessage("Registration successful! Redirecting to login...");
        setTimeout(() => {
          window.location.href = "/login";
        }, 800);
      } else {
        const error = await response.json();
        setErrorMessage(error.message || "Registration failed.");
      }
    } catch {
      setErrorMessage("Network error. Please try again.");
    }
  };

  return (
    
    <div className="auth-container">
      <div className="auth-header">
        <h2>Create Account</h2>
        <p>Register for a new Bankuum Tubo account</p>
      </div>

      {errorMessage && <div className="error-message">{errorMessage}</div>}
      {successMessage && <div className="success-message">{successMessage}</div>} 

      <form id="register-form" onSubmit={handleSubmit}>
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
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            name="username"
            placeholder="Choose a username"
            autoComplete="username"
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            placeholder="Create a password"
            autoComplete="new-password"
            required
          />
        </div>

        <button type="submit" className="btn-login">
          Register
        </button>

        <div className="signup-link">
          Already have an account? <a href="/login">Login</a>
        </div>
      </form>
    </div>
  );
}
