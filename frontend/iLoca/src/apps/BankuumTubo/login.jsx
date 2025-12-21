import { useState, useEffect} from "react";
import { Link, useNavigate } from "react-router-dom"
 

export default function Login() {
  // Define local variables from the localstorage
  const [username] = useState(() => localStorage.getItem("loggedInUsername")); 
  const [emailStored] = useState(() => localStorage.getItem("loggedInEmail"));
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [clientData, setClientData] = useState({ iban: "", balance: 0 });
  const navigate = useNavigate(); // Defines the navigation function for logic-induced routing

  useEffect(() => {
    if (username && emailStored) {
      (async () => {
        try {
          const resp = await fetch(`http://localhost:5027/client/GetClientByEmail?email=${encodeURIComponent(emailStored)}`); // Get information for a specific email
          if (resp.ok) {
            const client = await resp.json();
            if (client) setClientData({ iban: client.iban || "", balance: Number(client.balance) || 0 });
          }
        } catch (err) {
            console.error(err);
            alert("Error authenticating client");
          }
      });
    }
  }, [username, emailStored]);
   // The use effect runs only on mount up, and when username 
   // or emailStored are changed, to keep the authentication in sync.

  const handleSubmit = async (event) => {
    event.preventDefault(); // prevent default HTML behavior
    const email = event.target.email.value;
    const password = event.target.password.value;
    setErrorMessage("");
    setSuccessMessage("");

    try {
      const response = await fetch('http://localhost:5027/login/authenticate', { // sends the authentication request for the given email and password
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
        } catch  {
            // ignore for now
            }

        localStorage.setItem('loggedInEmail', email); // Set  email in localStorage

        if (!localStorage.getItem('loggedInUsername')) { // if email not found in localStorage, put the given one in localStorage
          const fallback = (email || '').split('@')[0];
          if (fallback) localStorage.setItem('loggedInUsername', fallback);
        }
        // After logging in, redirect with a delay to home page, logged in
        setTimeout(() => navigate("/BankuumTubo"), 0); 
      } else {
        const error = await response.json();
        setErrorMessage(error.message || 'Invalid email or password');
      }
    } catch {
      setErrorMessage('Network error. Please try again.');
    }
  };

  const handleLogout = () => { // When logging out, reset the localStorage values
    localStorage.removeItem('loggedInUsername');
    localStorage.removeItem('loggedInEmail');


    navigate("/BankuumTubo"); // Reset the html file when logging out
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
