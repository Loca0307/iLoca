import React from 'react';

export default function Games(){
  return (
    <div className="container">
      <header className="site-header center"><h1 className="site-title">VidaLoca</h1></header>
      <main>
        <div className="games-grid">
          <a className="game-card game-roulette" href="/vidaLoca/roulette">
            <img className="game-bg" src="/vida/assets/roulette.jpeg" alt="Roulette" />
            <div className="game-meta"><div className="game-title">Roulette</div></div>
          </a>
        </div>
      </main>
    </div>
  );
}
