import { useState } from "react";
import { Link } from "react-router-dom";


export default function Home() {
  const [username] = useState(() => localStorage.getItem("BankuumTubo/loggedInUsername"));

  return (
    <div className="home-container">

      <main>
        <section id="home-section" className="home-section">
          <div className="home-card">
            <h2 className="home-title">Welcome to Bankuum Tubo</h2>
            {username && (
              <p style={{ color: "#333", fontSize: "1.1rem", marginBottom: "1rem" }}>
                Logged in as <strong>{username}</strong>
              </p>
            )}
            <p className="home-subtitle">Your secure, modern banking dashboard.</p>
            <div className="home-actions">
              <Link to="/BankuumTubo/clients" className="main-action-btn">View Clients</Link>
              <Link to="/BankuumTubo/transactions" className="main-action-btn">Transactions</Link>
              {!username && <Link to="/BankuumTubo/login" className="main-action-btn">Login</Link>}
              {username && <Link to="/BankuumTubo/login" className="main-action-btn">Profile</Link>}
            </div>
            <p className="home-footer-note">
              Manage your clients, view transactions, and more.
            </p>
          </div>
        </section>
      </main>


    </div>
  );
}
