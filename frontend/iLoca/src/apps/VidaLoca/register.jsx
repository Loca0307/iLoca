import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

export default function Register(){
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [username, setUsername] = useState('');
  const [pwd, setPwd] = useState('');
  const [msg, setMsg] = useState('');

  async function doRegister(e){
    e.preventDefault();
    setMsg('');
    if(!email || !username || !pwd){ setMsg('Please provide username, email and password'); return; }
    try{
      const res = await fetch('http://localhost:5112/account/InsertAccount', {
        method: 'POST', headers: { 'Content-Type':'application/json' },
        body: JSON.stringify({ email, username, password: pwd })
      });
      if(!res.ok){
        const err = await res.json().catch(()=>({}));
        setMsg(err.message || 'Register failed');
        return;
      }
  setMsg('Registration successful! Redirecting to login...');
  setTimeout(() => navigate('/vidaLoca/login'), 900);
  }catch{ setMsg('Network error'); }
  }

  return (
    <div className="auth-section center">
      <div className="auth-container">
        <div className="auth-header"><h2>Create Account</h2></div>
        <form onSubmit={doRegister}>
          <div className="form-group"><label>Email</label><input type="email" value={email} onChange={e=>setEmail(e.target.value)} required /></div>
          <div className="form-group"><label>Username</label><input value={username} onChange={e=>setUsername(e.target.value)} required /></div>
          <div className="form-group"><label>Password</label><input type="password" value={pwd} onChange={e=>setPwd(e.target.value)} required /></div>
          <div className="form-group"><button className="btn-login" type="submit">Create</button></div>
          <p className="muted">{msg}</p>

          <p className="mt-1 muted">Already have an account? <Link to="/vidaLoca/login">Login</Link></p>
        </form>
      </div>
    </div>
  );
}
