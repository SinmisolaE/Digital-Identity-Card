import { useState, useEffect, useRef } from "react";
import QrScanner from 'qr-scanner';

const ReceiveCredential = () => {
  const [publicKey, setPublicKey] = useState("");
  const [scanning, setScanning] = useState(false);
  const [credentialReceived, setCredentialReceived] = useState(false);
  const videoRef = useRef(null);
  const qrScannerRef = useRef(null);

  useEffect(() => {
    // Generate or retrieve public key
    let key = localStorage.getItem("publicKey");
    if (!key) {
      key = generatePublicKey();
      localStorage.setItem("publicKey", key);
    }
    setPublicKey(key);
  }, []);

  // QR Scanner effect - same pattern as Verify.js
  useEffect(() => {
    if (scanning && videoRef.current) {
      qrScannerRef.current = new QrScanner(
        videoRef.current,
        (result) => {
          handleScan(result.data);
        },
        {
          highlightScanRegion: true,
          highlightCodeOutline: true,
        }
      );

      qrScannerRef.current.start();
    }

    return () => {
      if (qrScannerRef.current) {
        qrScannerRef.current.stop();
        qrScannerRef.current.destroy();
      }
    };
  }, [scanning]);

  const generatePublicKey = () => {
    const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    let result = "";
    for (let i = 0; i < 64; i++) {
      result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
  };

  const handleScan = (result) => {
    setScanning(false);
    
    // Store the scanned credential
    const credentials = JSON.parse(localStorage.getItem("credentials") || "[]");
    credentials.push({
      id: Date.now(),
      data: result,
      timestamp: new Date().toISOString(),
    });
    localStorage.setItem("credentials", JSON.stringify(credentials));
    setCredentialReceived(true);
    
    // Reset after 3 seconds
    setTimeout(() => setCredentialReceived(false), 3000);
  };

  return (
    <div className="mx-auto" style={{ maxWidth: "700px" }}>
      <div className="d-flex flex-column gap-4">
        {/* Public Key Card */}
        <div className="card shadow-card">
          <div className="card-body">
            <h5 className="card-title">Your Public Key</h5>
            <p className="card-text text-secondary small">
              This is your unique identifier for receiving credentials
            </p>
            <div className="p-3 bg-light rounded border mb-3">
              <p className="font-monospace small text-break mb-0">
                {publicKey}
              </p>
            </div>
          </div>
        </div>

        {/* Scan Credential Card */}
        <div className="card shadow-card">
          <div className="card-body">
            <h5 className="card-title">Receive Credential</h5>
            <p className="card-text text-secondary small">
              Scan a QR code to receive a new credential
            </p>
            <button
              onClick={() => setScanning(true)}
              className="btn btn-primary w-100 py-3 fs-5"
              disabled={scanning}
            >
              <i className="bi bi-qr-code me-2"></i>
              {scanning ? 'Scanning...' : 'Scan QR Code'}
            </button>
          </div>
        </div>

        {/* Success Message */}
        {credentialReceived && (
          <div className="card border-success shadow-elevated animate__animated animate__fadeInUp">
            <div className="card-body">
              <div className="d-flex align-items-center gap-3">
                <div className="d-flex align-items-center justify-content-center rounded-circle bg-success bg-opacity-25" style={{ width: "48px", height: "48px" }}>
                  <i className="bi bi-check-circle text-success fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title text-success mb-1">Credential Received!</h5>
                  <p className="card-text text-secondary small mb-0">
                    Your credential has been securely stored
                  </p>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* QR Scanner - same modal pattern as Verify.js */}
        {scanning && (
          <div className="qr-scanner-modal" style={{
            position: 'fixed',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: 'rgba(0,0,0,0.9)',
            zIndex: 1000,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center'
          }}>
            <div className="qr-scanner-container" style={{
              backgroundColor: 'white',
              borderRadius: '15px',
              padding: '20px',
              maxWidth: '90vw',
              maxHeight: '90vh',
              boxShadow: '0 10px 30px rgba(0,0,0,0.3)'
            }}>
              <div className="d-flex justify-content-between align-items-center mb-3">
                <h5 className="mb-0">
                  <i className="bi bi-qr-code me-2"></i>
                  Scan QR Code
                </h5>
                <button
                  onClick={() => setScanning(false)}
                  className="btn btn-outline-secondary btn-sm"
                >
                  <i className="bi bi-x-lg"></i>
                </button>
              </div>
              <video
                ref={videoRef}
                style={{
                  width: '100%',
                  maxWidth: '400px',
                  height: '300px',
                  objectFit: 'cover',
                  borderRadius: '10px',
                  border: '2px solid #dee2e6'
                }}
                playsInline
              />
              <p className="text-center mt-3 mb-0 text-muted">
                <i className="bi bi-camera me-1"></i>
                Position the QR code within the camera view
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ReceiveCredential;