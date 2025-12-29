import { Link } from "react-router-dom";
import "../styles/iLoca.css";

export default function Home() {
  return (
    <div className="page-wrap">
      {/* App title outside the phone */}
      <h1 className="page-title">iLoca</h1>

      <div className="device-wrap">
        <div className="iphone-body">

          {/* Top thin bezel + speaker */}
          <div className="iphone-top">
            <div className="speaker" />
          </div>

          {/* Phone screen */}
          <div className="screen">
            <main className="phone-main">
              <div className="home-screen">
                <div className="app-grid">
                  <Link to="/BankuumTubo" className="app-tile">
                    <div className="app-icon" aria-hidden>🏦</div>
                    <div className="app-label">Bankuum Tubo</div>
                  </Link>
                  <Link to="/vidaLoca" className="app-tile">
                    <div className="app-icon" aria-hidden>🎲</div>
                    <div className="app-label">VidaLoca</div>
                  </Link>
                </div>
              </div>
            </main>
          </div>

          {/* Bottom bezel with home button */}
          <div className="iphone-bottom">
            <div className="home-button" />
          </div>

        </div>
      </div>
    </div>
  );
}
