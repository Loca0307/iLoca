import { useNavigate } from "react-router-dom";

import "../styles/style.css";

export default function Home() {
  const navigate = useNavigate();

  const openBankuum = () => {
    navigate("/bankuum");
  };

  return (
    <div className="phone-outer">
      <div className="phone-container">
        <header className="phone-header">
          <h1>iLoca</h1>
        </header>

        <main className="phone-main">
            <div className="home-screen">
              <div className="app-grid">
                <div
                  className="app-tile"
                  role="button"
                  tabIndex={0}
                  onClick={openBankuum}
                  onKeyDown={(e) => e.key === "Enter" && openBankuum()}
                >
                  <div className="app-icon">🏦</div>
                  <div className="app-label">Bankuum Tubo</div>
                </div>
                
              </div>
            </div>
        </main>
      </div>
    </div>
  );
}
