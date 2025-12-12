import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Home from "./apps/Bankuum Tubo/index";
import Clients from "./apps/Bankuum Tubo/client";
import Transactions from "./apps/Bankuum Tubo/transaction";
import Login from "./apps/Bankuum Tubo/login";
import Register from "./apps/Bankuum Tubo/register";
import "./styles/style.css";

function App() {
  return (
    <Router>
      <header>
        <h1 className="site-logo"><Link to="/">Bankuum Tubo</Link></h1>
        <nav>
          <Link to="/">Home</Link>
          <Link to="/clients">Clients</Link>
          <Link to="/transactions">Transactions</Link>
          <Link to="/login">Profile</Link>
        </nav>
      </header>

      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/clients" element={<Clients />} />
          <Route path="/transactions" element={<Transactions />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Routes>
      </main>

      <footer>
        <p>© 2025 Bankuum Tubo. All rights reserved.</p>
      </footer>
    </Router>
  );
}

export default App;
