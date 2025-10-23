import { Link, useLocation } from "react-router-dom";

const Layout = ({ children }) => {
  const location = useLocation();

  const isActive = (path) => location.pathname === path;

  return (
    <div className="d-flex flex-column min-vh-100">
      {/* Header */}
      <header className="sticky-top border-bottom bg-white shadow-sm">
        <div className="container">
          <div className="d-flex align-items-center justify-content-between py-3">
            <div className="d-flex align-items-center gap-2">
              <div className="d-flex align-items-center justify-content-center rounded gradient-primary" style={{ width: "40px", height: "40px" }}>
                <i className="bi bi-wallet2 text-white fs-5"></i>
              </div>
              <span className="fs-4 fw-bold">Citizen Wallet</span>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="flex-grow-1 container py-4">
        {children}
      </main>

      {/* Bottom Navigation */}
      <nav className="sticky-bottom border-top bg-white shadow-sm">
        <div className="container">
          <div className="d-flex justify-content-around py-3">
            <Link
              to="/"
              className={`d-flex flex-column align-items-center gap-1 px-4 py-2 rounded text-decoration-none ${
                isActive("/")
                  ? "text-primary bg-primary bg-opacity-10"
                  : "text-secondary"
              }`}
            >
              <i className="bi bi-qr-code fs-4"></i>
              <span className="small fw-medium">Receive</span>
            </Link>
            <Link
              to="/verify"
              className={`d-flex flex-column align-items-center gap-1 px-4 py-2 rounded text-decoration-none ${
                isActive("/verify")
                  ? "text-primary bg-primary bg-opacity-10"
                  : "text-secondary"
              }`}
            >
              <i className="bi bi-shield-check fs-4"></i>
              <span className="small fw-medium">Verify</span>
            </Link>
          </div>
        </div>
      </nav>
    </div>
  );
};

export default Layout;
