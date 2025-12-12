import { BrowserRouter as Router, Routes, Route, Link, Outlet } from "react-router-dom";
import Home from "./iLoca/index"; 
import Clients from "./apps/Bankuum Tubo/client";
import Transactions from "./apps/Bankuum Tubo/transaction";
import Login from "./apps/Bankuum Tubo/login";
import Register from "./apps/Bankuum Tubo/register";
import BankuumLanding from "./apps/Bankuum Tubo/index";
import "./styles/style.css";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />

        <Route element={<FullAppLayout />}>
          <Route path="/bankuum" element={<BankuumLanding />} />
          <Route path="/clients" element={<Clients />} />
          <Route path="/transactions" element={<Transactions />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;

function FullAppLayout() {
  return (
    <div className="full-app-root">
      <header className="site-header">
        <h1 className="site-logo"><Link to="/bankuum">Bankuum Tubo</Link></h1>
        <nav className="site-nav">
          <Link to="/">Phone</Link>
          <Link to="/clients">Clients</Link>
          <Link to="/transactions">Transactions</Link>
          <Link to="/login">Profile</Link>
        </nav>
      </header>

      <main className="site-main">
        <Outlet />
      </main>

      <footer className="site-footer">
        <p>© 2025 Bankuum Tubo. All rights reserved.</p>
      </footer>
    </div>
  );
}
