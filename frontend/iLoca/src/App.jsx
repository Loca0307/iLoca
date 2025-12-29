import { BrowserRouter, Routes, Route, Outlet } from "react-router-dom";

import Phone from "./iLoca";

import BankuumHeader from "./apps/BankuumTubo/header";
import BankuumFooter from "./apps/BankuumTubo/footer";

import BankuumHome from "./apps/BankuumTubo/index";
import BankuumClient from "./apps/BankuumTubo/client";
import BankuumTransaction from "./apps/BankuumTubo/transaction";
import BankuumLogin from "./apps/BankuumTubo/login";
import BankuumRegister from "./apps/BankuumTubo/register";


import VidaHeader from "./apps/VidaLoca/header";
import VidaFooter from "./apps/VidaLoca/footer";

import VidaHome from "./apps/VidaLoca/index";
import VidaGames from "./apps/VidaLoca/games";
import VidaRoulette from "./apps/VidaLoca/roulette";
import VidaTransaction from "./apps/VidaLoca/transaction";
import VidaLogin from "./apps/VidaLoca/login";
import VidaRegister from "./apps/VidaLoca/register";


// Layout used only for Bankuum Tubo routes
const BankuumTuboLayout = () => {
  return (
    <div className="full-app-root">
      <BankuumHeader />
      <main className="site-main">
        <Outlet /> {/* Actual placeholder for the components*/}
      </main>
      <BankuumFooter />
    </div>
  );
};

const VidaLocaLayout = () => (
  <div className="full-app-root">
    <VidaHeader />
    <main className="site-main"><Outlet/></main>
    <VidaFooter />
  </div>
);

function App() {
  return (
    <BrowserRouter> {/* Router that uses browser history*/}
      <Routes>

        {/* Phone app (landing) */}
        <Route path="/" element={<Phone />} />

        {/* Bankuum Tubo app routes, with BankuumTubo as route prefix*/}
        <Route path="BankuumTubo" element={<BankuumTuboLayout />} >
          <Route index element={<BankuumHome />} />
          <Route path="clients" element={<BankuumClient />} />
          <Route path="transactions" element={<BankuumTransaction/> } />
          <Route path="login" element={<BankuumLogin />} />
          <Route path="register" element={<BankuumRegister />} />
        </Route>

        {/* VidaLoca routes */}
        <Route path="vidaLoca" element={<VidaLocaLayout/>}>
          <Route index element={<VidaHome/>} />
          <Route path="games" element={<VidaGames/>} />
          <Route path="roulette" element={<VidaRoulette/>} />
          <Route path="transaction" element={<VidaTransaction/>} />
          <Route path="login" element={<VidaLogin/>} />
          <Route path="register" element={<VidaRegister/>} />
        </Route>

      </Routes>
    </BrowserRouter>
  );
}

export default App;
