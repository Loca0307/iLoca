import { useState } from "react";
import { Link, useNavigate } from "react-router-dom"

export default function Register() {
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault(); // Prevent default HTML behavior


    // Store in variables the data written and submitted by the user
    const username = (event.target.username.value || "").trim();
    const email = (event.target.email.value || "").trim();
    const password = (event.target.password.value || "").trim();

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
          navigate("/login"); // After registration, return to the login page with useNavigate
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
          Already have an account? <Link to="/BankuumTubo/login">Login</Link>
        </div>
      </form>
    </div>
  );
}
