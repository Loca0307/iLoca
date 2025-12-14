import { Link } from "react-router-dom";
import "../../styles/style.css";

export default function Header() {
  return (
    <header className="site-header">
      <h1 className="site-logo">
        <Link to="/bankuum">Bankuum Tubo</Link>
      </h1>

      <nav className="site-nav">
        <Link to="/">Phone</Link>
        <Link to="/clients">Clients</Link>
        <Link to="/transactions">Transactions</Link>
        <Link to="/login">Profile</Link>
      </nav>
    </header>
  );
}
