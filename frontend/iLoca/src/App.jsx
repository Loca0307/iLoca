import { BrowserRouter, Routes, Route, Outlet } from "react-router-dom";

import Phone from "./iLoca";

import Home from "./apps/BankuumTubo/index";
import Client from "./apps/BankuumTubo/client";
import Transaction from "./apps/BankuumTubo/transaction";
import Login from "./apps/BankuumTubo/login";
import Register from "./apps/BankuumTubo/register";

import Header from "./apps/BankuumTubo/header";
import Footer from "./apps/BankuumTubo/footer";

// Layout used only for Bankuum Tubo routes
const BankuumTuboLayout = () => {
  return (
    <div className="full-app-root">
      <Header />
      <main className="site-main">
        <Outlet /> {/* Actual placeholder for the components*/}
      </main>
      <Footer />
    </div>
  );
};

function App() {
  return (
    <BrowserRouter> {/* Router that uses browser history*/}
      <Routes>

        {/* Phone app (landing) */}
        <Route path="/" element={<Phone />} />

        {/* Bankuum Tubo app routes, with BankuumTubo as route prefix*/}
        <Route path="BankuumTubo" element={<BankuumTuboLayout />} >
          <Route index element={<Home />} />
          <Route path="clients" element={<Client />} />
          <Route path="transactions" element={<Transaction/> } />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
        </Route>

      </Routes>
    </BrowserRouter>
  );
}

export default App;
