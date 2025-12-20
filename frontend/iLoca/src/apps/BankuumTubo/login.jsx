import { useState, useEffect} from "react";
import { Link } from "react-router-dom"
 

export default function Login() {
  const [username] = useState(() => localStorage.getItem("loggedInUsername"));
  const [emailStored] = useState(() => localStorage.getItem("loggedInEmail"));
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [clientData, setClientData] = useState({ iban: "", balance: 0 });

  useEffect(() => {
    if (username && emailStored) {
      (async () => {
        try {
          const resp = await fetch(`http://localhost:5027/client/GetClientByEmail?email=${encodeURIComponent(emailStored)}`);
          if (resp.ok) {
            const client = await resp.json();
            if (client) setClientData({ iban: client.iban || "", balance: Number(client.balance) || 0 });
          }
        } catch {
            // ignore errors 
            }
      })();
    }
  }, [username, emailStored]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const email = e.target.email.value;
    const password = e.target.password.value;
    setErrorMessage("");
    setSuccessMessage("");

    try {
      const response = await fetch('http://localhost:5027/login/authenticate', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });

      if (response.ok) {
        setSuccessMessage('Login successful! Redirecting...');
        try {
          const usersResp = await fetch('/Login/ShowLogins');
          if (usersResp.ok) {
            const users = await usersResp.json();
            const found = Array.isArray(users) ? users.find(u => (u.email || '').toLowerCase() === email.toLowerCase()) : null;
            if (found && found.username) localStorage.setItem('loggedInUsername', found.username);
          }
        } catch {
            // ignore errors 
            }

        localStorage.setItem('loggedInEmail', email);

        if (!localStorage.getItem('loggedInUsername')) {
          const fallback = (email || '').split('@')[0];
          if (fallback) localStorage.setItem('loggedInUsername', fallback);
        }

        setTimeout(() => window.location.href = '/html/index.html', 1200);
      } else {
        const error = await response.json();
        setErrorMessage(error.message || 'Invalid email or password');
      }
    } catch {
      setErrorMessage('Network error. Please try again.');
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('loggedInUsername');
    localStorage.removeItem('loggedInEmail');
    window.location.href = '/html/index.html';
  };

  return (
    <div className="auth-container">
      <div className="auth-header">
        <h2>Welcome Back</h2>
        <p>Login to your Bankuum Tubo account</p>
      </div>

      {errorMessage && <div id="error-message" className="error-message">{errorMessage}</div>}
      {successMessage && <div id="success-message" className="success-message">{successMessage}</div>}

      {!username ? (
        <form id="login-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email Address</label>
            <input type="email" id="email" name="email" placeholder="Enter your email" autoComplete="email" required />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input type="password" id="password" name="password" placeholder="Enter your password" autoComplete="current-password" required />
          </div>

          <button type="submit" className="btn-login">Login</button>

          <div id="signup-link" className="signup-link">
            Don't have an account? <Link to="/BankuumTubo/register">Register</Link>
          </div>
        </form>
      ) : (
        <div id="profile-section" style={{ display: 'block', marginTop: '1rem', textAlign: 'center' }}>
          <p id="profile-username" style={{ fontWeight:600, color:'#333', marginBottom:'0.5rem' }}>Logged in as {username}</p>
          <p id="profile-email" style={{ color:'#666', fontSize:'0.95rem', marginTop:0, marginBottom:'1rem' }}>{emailStored ? `Email: ${emailStored}` : ''}</p>
          <p id="profile-iban" style={{ color:'#333', fontSize:'1.1rem', fontWeight:600, margin:'0.2rem 0' }}>{clientData.iban ? `IBAN: ${clientData.iban}` : ''}</p>
          <p id="profile-balance" style={{ color:'#2b7a0b', fontSize:'1.3rem', fontWeight:700, margin:'0.2rem 0' }}>{`Balance: ${clientData.balance.toFixed(2)} €`}</p>
          <button id="logout-btn" className="btn-login" style={{ background:'#e53935', border:'none', padding:'0.6rem 1.2rem', color:'#fff', cursor:'pointer', borderRadius:'6px' }} onClick={handleLogout}>Logout</button>
        </div>
      )}
    </div>
  );
}
