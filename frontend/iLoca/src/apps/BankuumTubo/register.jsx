import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Register() {

  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: "",
    username: "",
    password: ""
  });

  const [statusMessage, setStatusMessage] = useState("");
  const [statusType, setStatusType] = useState(""); // "success" or "error"

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const res = await fetch("http://localhost:5027/login/InsertLogin", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData)
      });

      if (!res.ok) throw new Error("Failed to add login");

      setStatusType("success");
      setStatusMessage("Account successfully created! Redirecting to login...");

      // Wait 1.5 seconds then navigate
      setTimeout(() => {
        navigate("/BankuumTubo/login");
      }, 1500);

    } catch (err) {
      console.error(err);
      setStatusType("error");
      setStatusMessage("Failed to create account. Please try again.");
    }
  };

  function handleChange(event) {
    setFormData({ ...formData, [event.target.id]: event.target.value });
  }

  return (
    <div className="auth-container">
      <div className="auth-header">
        <h2>Create Account</h2>
        <p>Register for a new Bankuum Tubo account</p>
      </div>

      <form id="register-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="email">Email Address</label>
          <input type="email" id="email" onChange={handleChange} required />
        </div>

        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input type="text" id="username" onChange={handleChange} required />
        </div>

        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input type="password" id="password" onChange={handleChange} required />
        </div>

        <button type="submit" className="btn-login">Register</button>

        {statusMessage && (
          <p className={"auth-status " + (statusType === "success" ? "status-success" : "status-error")}>{statusMessage}</p>
        )}

        <div className="signup-link">
          Already have an account? <Link to="/BankuumTubo/login">Login</Link>
        </div>
      </form>
    </div>
  );
}
