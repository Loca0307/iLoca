import React from 'react';

export default function Index(){
  return (
    <div className="container center">
      <header className="site-header">
        <h1 className="site-title">VidaLoca</h1>
      </header>
      <main>
        <section className="games-cta center">
          <h2 className="game-title">Welcome to VidaLoca</h2>
          <p className="games-intro">Choose a game and have fun.</p>
          <div className="games-grid">
            <div className="auth-cta">
              <a className="btn btn-primary" href="/vida/register">Register</a>
              <a className="btn btn-ghost" href="/vida/login">Login</a>
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
