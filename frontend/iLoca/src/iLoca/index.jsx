import { Link } from "react-router-dom"; // When the routing is User-based use "Link"

export default function Home() {
  return (
    <div className="phone-outer">
      <div className="phone-container">
        <header className="phone-header">
          <h1>iLoca</h1>
        </header>

        <main className="phone-main">
          <div className="home-screen">
            <div className="app-grid">
              <Link to="/BankuumTubo" className="app-tile">
                <div className="app-icon" aria-hidden>🏦</div>
                <div className="app-label">Bankuum Tubo</div>
              </Link>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
