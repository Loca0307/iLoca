import React from 'react';

export default function Index(){
  return (
    <div className="container center">
      <main>
        <section className="games-cta center" style={{paddingTop: '2rem', paddingBottom: '2rem', minHeight: '40vh'}}>
          <h2 className="game-title" style={{display:'none'}}>Welcome to VidaLoca</h2>
          <p className="games-intro" style={{display:'none'}}>Create an account or login to start playing.</p>
          <div className="auth-cta" style={{marginTop: '0.4rem'}}>
            <a className="btn btn-primary" href="/vidaLoca/register">Register</a>
            <a className="btn btn-ghost" href="/vidaLoca/login">Login</a>
          </div>
        </section>
      </main>
    </div>
  );
}
