import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

export default function Login(){
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [msg, setMsg] = useState('');
  const [isError, setIsError] = useState(false);

  async function doLogin(e){
    e.preventDefault();
    setMsg(''); setIsError(false);
    if(!email || !password){
      setIsError(true);
      setMsg('Please provide email and password.');
      return;
    }

    try{
      const res = await fetch('http://localhost:5112/account/Authenticate', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });

      if(res.ok){
        const authData = await res.json().catch(() => null);
        setMsg('Login successful! Redirecting...');
        setIsError(false);
        // store email (namespaced) and accountId when available
        localStorage.setItem('VidaLoca/loggedInEmail', email);
        if(authData && authData.accountId){
          localStorage.setItem('accountId', String(authData.accountId));
        }
        if(authData && authData.username){
          localStorage.setItem('VidaLoca/loggedInUsername', authData.username);
        }
  // navigate to games route in react app
  setTimeout(() => navigate('/vidaLoca/games'), 800);
      } else {
        const ebody = await res.json().catch(() => ({}));
        setIsError(true);
        setMsg(ebody.message || 'Invalid email or password');
      }
    } catch {
      setIsError(true);
      setMsg('Network error. Please try again.');
    }
  }

  return (
    <div className="auth-section center">
      <div className="auth-container">
        <div className="auth-header"><h2>Login</h2></div>
        <form onSubmit={doLogin}>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input id="email" type="email" autoComplete="email" value={email} onChange={e=>setEmail(e.target.value)} required />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input id="password" type="password" autoComplete="current-password" value={password} onChange={e=>setPassword(e.target.value)} required />
          </div>

          <div className="form-group">
            <button className="btn-login" type="submit">Login</button>
          </div>

          <p className={`muted ${isError ? 'error-message' : 'success-message'}`} aria-live="polite">{msg}</p>

          <p className="mt-1 muted">Don't have an account? <Link to="/vidaLoca/register">Register</Link></p>
        </form>
      </div>
    </div>
  );
}
