import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Phone from "./iLoca/index";
import Client from "./apps/Bankuum Tubo/client";
import Transaction from "./apps/Bankuum Tubo/transaction";
import Login from "./apps/Bankuum Tubo/login";
import Register from "./apps/Bankuum Tubo/register";
import BankuumLanding from "./apps/Bankuum Tubo/index";

import Header from "./apps/Bankuum Tubo/header";
import Footer from "./apps/Bankuum Tubo/footer";

import "./styles/style.css";

// Local layout used only for Bankuum app routes with its specific footer and header
const BankuumLayout = ({ children }) => (
  <div className="full-app-root">
    <Header />
    <main className="site-main">{children}</main>
    <Footer />
  </div>
);

function App() {
  return (
    <Router>
      <Routes>
        {/* Phone */}
        <Route path="/" element={<Phone />} />

        {/* Bankuum Tubo*/}
        <Route path="/bankuum" element={<BankuumLayout><BankuumLanding /></BankuumLayout>} />
        <Route path="/clients" element={<BankuumLayout><Client /></BankuumLayout>} />
        <Route path="/transactions" element={<BankuumLayout><Transaction /></BankuumLayout>} />
        <Route path="/login" element={<BankuumLayout><Login /></BankuumLayout>} />
        <Route path="/register" element={<BankuumLayout><Register /></BankuumLayout>} />
      </Routes>
    </Router>
  );
}

export default App;
