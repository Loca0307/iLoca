import React, { useState } from 'react';

export default function Login(){
  const [email, setEmail] = useState('');
  const [msg, setMsg] = useState('');
  async function doLogin(e){
    e.preventDefault();
    try{
      const res = await fetch('/Account/GetAccountByEmail?email='+encodeURIComponent(email));
      if(!res.ok) { setMsg('Login failed'); return; }
      const acct = await res.json();
      if(acct && acct.accountId){
        localStorage.setItem('VidaLoca/loggedInEmail', email);
        localStorage.setItem('accountId', String(acct.accountId));
        if(acct.username) localStorage.setItem('VidaLoca/loggedInUsername', acct.username);
        setMsg('Logged in as ' + (acct.username || email));
      }else setMsg('Account not found');
  }catch{ setMsg('Network error'); }
  }

  return (
    <div className="auth-section center">
      <div className="auth-container">
        <div className="auth-header"><h2>Login</h2></div>
        <form onSubmit={doLogin}>
          <div className="form-group"><label>Email</label><input value={email} onChange={e=>setEmail(e.target.value)} /></div>
          <div className="form-group"><button className="btn-login" type="submit">Login</button></div>
          <p className="muted">{msg}</p>
        </form>
      </div>
    </div>
  );
}
