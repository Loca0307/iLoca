

import React from "react";
import "../../styles/style.css";

export default function Home() {
  const [username] = React.useState(() => localStorage.getItem("loggedInUsername"));

  return (
    <div className="home-container">
      <header>
        <h1 className="site-logo">
          <a id="site-logo" href="/">Bankuum Tubo</a>
        </h1>
        <nav>
          <a href="/">Home</a>
          <a href="/clients">Clients</a>
          <a href="/transactions">Transactions</a>
          <a href="/profile">Profile</a>
        </nav>
      </header>

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
              <a href="/clients" className="main-action-btn">View Clients</a>
              <a href="/transactions" className="main-action-btn">Transactions</a>
              {!username && <a href="/login" className="main-action-btn">Login</a>}
              {username && <a href="/profile" className="main-action-btn">Profile</a>}
            </div>
            <p className="home-footer-note">
              Manage your clients, view transactions, and more.
            </p>
          </div>
        </section>
      </main>

      <footer>
        <p>© 2025 Bankuum Tubo. All rights reserved.</p>
      </footer>
    </div>
  );
}
