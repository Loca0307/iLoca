import { BrowserRouter, Routes, Route, Outlet } from "react-router-dom";

import Phone from "./iLoca";

import Home from "./apps/BankuumTubo";
import Client from "./apps/BankuumTubo/client";
import Transaction from "./apps/BankuumTubo/transaction";
import Login from "./apps/BankuumTubo/login";
import Register from "./apps/BankuumTubo/register";

import Header from "./apps/BankuumTubo/header";
import Footer from "./apps/BankuumTubo/footer";

// Layout used only for Bankuum Tubo routes
const BankuumLayout = () => {
  return (
    <div className="full-app-root">
      <Header />
      <main className="site-main">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
};

function App() {
  return (
    <BrowserRouter>
      <Routes>

        {/* Phone app (landing) */}
        <Route path="/" element={<Phone />} />

        {/* Bankuum Tubo app */}
        <Route path="/bankuum" element={<BankuumLayout />}>
          <Route index element={<Home />} />
          <Route path="clients" element={<Client />} />
          <Route path="transactions" element={<Transaction />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
        </Route>

      </Routes>
    </BrowserRouter>
  );
}

export default App;
