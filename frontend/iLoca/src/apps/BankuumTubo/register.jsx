import { useState } from "react";
import { Link } from "react-router-dom";

export default function Register() {

  const [formData, setFormData] = useState({
    email: "",
    username: "",
    password: ""
  });

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const res = await fetch("http://localhost:5027/login/InsertLogin", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData)  
      });

      if (!res.ok) throw new Error("Failed to add login");


    } catch (err) {
      console.error(err);
      alert("Error adding login");
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

        <div className="signup-link">
          Already have an account? <Link to="/BankuumTubo/login">Login</Link>
        </div>
      </form>
    </div>
  );
}
