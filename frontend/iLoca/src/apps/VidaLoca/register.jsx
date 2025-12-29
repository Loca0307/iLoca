import React, { useState } from 'react';

export default function Register(){
  const [email, setEmail] = useState('');
  const [pwd, setPwd] = useState('');
  const [msg, setMsg] = useState('');
  async function doRegister(e){
    e.preventDefault();
    try{
      const res = await fetch('/Account/Register', { method: 'POST', headers: { 'Content-Type':'application/json' }, body: JSON.stringify({ email, password: pwd }) });
      if(!res.ok){ setMsg('Register failed'); return; }
      setMsg('Registered');
  }catch{ setMsg('Network error'); }
  }

  return (
    <div className="auth-section center">
      <div className="auth-container">
        <div className="auth-header"><h2>Register</h2></div>
        <form onSubmit={doRegister}>
          <div className="form-group"><label>Email</label><input value={email} onChange={e=>setEmail(e.target.value)} /></div>
          <div className="form-group"><label>Password</label><input type="password" value={pwd} onChange={e=>setPwd(e.target.value)} /></div>
          <div className="form-group"><button className="btn-login" type="submit">Create</button></div>
          <p className="muted">{msg}</p>
        </form>
      </div>
    </div>
  );
}
