import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import '../../styles/vida.css';

export default function Header() {
  useEffect(() => {
    // ensure Vida styles only active when Vida app is mounted
    document.body.classList.add('vida');
    return () => document.body.classList.remove('vida');
  }, []);

  return (
    <header className="site-header">
      <h1 className="site-title"><Link to="/vidaLoca">VidaLoca</Link></h1>
      <nav className="site-nav">
        <Link to="/vidaLoca/games">Games</Link>
        <Link to="/vidaLoca/transaction">Transactions</Link>
        <Link to="/vidaLoca/login">Login</Link>
      </nav>
    </header>
  );
}
