import { useState, useEffect, useRef } from "react";
import QrScanner from 'qr-scanner';


const VerifyIdentity = () => {
  const [scanning, setScanning] = useState(false);
  const [verificationRequest, setVerificationRequest] = useState(null);
  const [verificationStatus, setVerificationStatus] = useState(null);
  const [storedCredentials, setStoredCredentials] = useState([]);
  const videoRef = useRef(null);
  const qrScannerRef = useRef(null);

  // Load stored credentials on component mount
  useEffect(() => {
    const credentials = JSON.parse(localStorage.getItem("credentials") || "[]");
    setStoredCredentials(credentials);
  }, []);

  // QR Scanner effect
  useEffect(() => {

  const handleScan = (result) => {
    setScanning(false);

    alert("Scanned qr code");
    
    // Parse the scanned QR code (contains nonce and verifier URL)
    try {
      const data = JSON.parse(result);

      if (!data.nonce || !data.verifierUrl) {
        alert("Invalid QR code: missing required information");
        return;
      }

      alert("nonce:" + data?.nonce);
      alert("url: " + data?.verifierUrl);
      setVerificationRequest({
        nonce: data.nonce, // The unique verification session ID
        verifierUrl: data.verifierUrl, // URL to send credentials to
      });


      handleApprove(data.nonce, data.verifierUrl);

    } catch {

      alert('not getting nonce');
      // If not JSON, assume it's just a URL with nonce as parameter
      const url = new URL(result);
      const nonce = url.searchParams.get('nonce');
      
      setVerificationRequest({
        nonce: nonce,
        verifierUrl: result,
      });

    }
  };


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
  }, [scanning]); // eslint-disable-next-line react-hooks/exhaustive-deps




  const handleApprove = async (nonce, verifierUrl) => {
    if (storedCredentials.length === 0) {
      alert("No credentials available to share. Please receive a credential first.");
      setVerificationRequest(null);
      return;
    }

    try {
      // Use the most recent credential or let user select
      const credentialToShare = storedCredentials[storedCredentials.length - 1];
      
      // Send credentials with nonce to verifier URL
      const payload = {
        Jwt: credentialToShare.data,
        nonce: verificationRequest.nonce, // JWT token
      };

      alert(payload);

      const response = await fetch(verificationRequest.verifierUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload)
      });

      if (response.ok) {
        alert("Credential shared successfully");
        console.log("Credential shared successfully");
        setVerificationStatus("approved");
      } else {
        alert("Failed to share credential");
        console.error("Failed to share credential");
        setVerificationStatus("error");
      }
      
    } catch (error) {
      alert("Error sharing credential:", error);
      console.error("Error sharing credential:", error);
      setVerificationStatus("error");
    }
    
    // Reset after 5 seconds
    setTimeout(() => {
      setVerificationRequest(null);
      setVerificationStatus(null);
    }, 5000);
  };

  const handleReject = async () => {
    try {
      // Optionally notify verifier of rejection
      const payload = {
        nonce: verificationRequest.nonce,
        status: "rejected"
      };

      await fetch(verificationRequest.verifierUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload)
      });
    } catch (error) {
      console.error("Error notifying rejection:", error);
    }

    setVerificationStatus("rejected");
    
    // Reset after 5 seconds
    setTimeout(() => {
      setVerificationRequest(null);
      setVerificationStatus(null);
    }, 5000);
  };

  return (
    <div className="mx-auto" style={{ maxWidth: "700px" }}>
      <div className="d-flex flex-column gap-4">
        {!verificationRequest ? (
          // Scan Card
          <div className="card shadow-card">
            <div className="card-body">
              <div className="d-flex align-items-center gap-3 mb-3">
                <div className="d-flex align-items-center justify-content-center rounded-circle bg-primary bg-opacity-10" style={{ width: "48px", height: "48px" }}>
                  <i className="bi bi-shield-check text-primary fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title mb-1">Verify Your Identity</h5>
                  <p className="card-text text-secondary small mb-0">
                    Scan a verifier's QR code to begin
                  </p>
                </div>
              </div>
              <button
                onClick={() => setScanning(true)}
                className="btn btn-primary w-100 py-3 fs-5"
              >
                <i className="bi bi-qr-code me-2"></i>
                Scan Verifier QR
              </button>
            </div>
          </div>
        ) : verificationStatus === null ? (
          // Verification Request Card
          <div className="card shadow-elevated border-primary border-opacity-25">
            <div className="card-body">
              <h4 className="card-title mb-1">Share Request</h4>
              <p className="card-text text-secondary small mb-4">
                Review the requested information before sharing
              </p>

              {/* Verifier Info */}
              <div className="mb-4">
                <h6 className="text-secondary small mb-2">Verifier</h6>
                <div className="d-flex align-items-center gap-3 p-3 bg-primary bg-opacity-10 rounded border border-primary border-opacity-25">
                  <div className="d-flex align-items-center justify-content-center rounded-circle bg-primary bg-opacity-10" style={{ width: "40px", height: "40px" }}>
                    <i className="bi bi-shield-check text-primary"></i>
                  </div>
                  <span className="fs-5 fw-semibold">{verificationRequest.verifierName}</span>
                </div>
              </div>

              {/* Session Info */}
              <div className="mb-4">
                <h6 className="text-secondary small mb-2">Session Info</h6>
                <div className="p-3 bg-light rounded">
                  <small className="text-muted">Nonce: {verificationRequest.nonce}</small>
                </div>
              </div>

              {/* Requested Data */}
              <div className="mb-4">
                <h6 className="text-secondary small mb-2">Requested Information</h6>
                <div className="d-flex flex-column gap-2">
                  {verificationRequest.requestedData.map((item, index) => (
                    <div
                      key={index}
                      className="d-flex align-items-center gap-2 p-3 bg-light rounded"
                    >
                      <i className="bi bi-check-circle text-primary"></i>
                      <span className="fw-medium">{item}</span>
                    </div>
                  ))}
                </div>
              </div>

              {/* Action Buttons */}
              <div className="d-flex gap-3 pt-3">
                <button
                  onClick={handleReject}
                  className="btn btn-outline-danger flex-fill py-2"
                >
                  <i className="bi bi-x-circle me-2"></i>
                  Reject
                </button>
                <button
                  onClick={handleApprove}
                  className="btn btn-success flex-fill py-2 gradient-success border-0"
                >
                  <i className="bi bi-check-circle me-2"></i>
                  Approve & Share
                </button>
              </div>
            </div>
          </div>
        ) : (
          // Status Card
          <div
            className={`card shadow-elevated animate__animated animate__fadeInUp ${
              verificationStatus === "approved"
                ? "border-success"
                : verificationStatus === "error"
                ? "border-warning"
                : "border-danger"
            }`}
          >
            <div className="card-body text-center py-5">
              <div
                className={`d-flex align-items-center justify-content-center rounded-circle mx-auto mb-3 ${
                  verificationStatus === "approved"
                    ? "bg-success bg-opacity-25"
                    : verificationStatus === "error"
                    ? "bg-warning bg-opacity-25"
                    : "bg-danger bg-opacity-25"
                }`}
                style={{ width: "80px", height: "80px" }}
              >
                <i
                  className={`bi ${
                    verificationStatus === "approved"
                      ? "bi-check-circle text-success"
                      : verificationStatus === "error"
                      ? "bi-exclamation-triangle text-warning"
                      : "bi-x-circle text-danger"
                  }`}
                  style={{ fontSize: "2.5rem" }}
                ></i>
              </div>
              <h2
                className={`mb-2 ${
                  verificationStatus === "approved"
                    ? "text-success"
                    : verificationStatus === "error"
                    ? "text-warning"
                    : "text-danger"
                }`}
              >
                {verificationStatus === "approved" 
                  ? "Approved" 
                  : verificationStatus === "error"
                  ? "Error"
                  : "Rejected"}
              </h2>
              <p className="text-secondary mb-0">
                {verificationStatus === "approved"
                  ? "Your credential has been shared successfully"
                  : verificationStatus === "error"
                  ? "Failed to share credential with verifier"
                  : "You have rejected the verification request"}
              </p>
            </div>
          </div>
        )}

        {/* QR Scanner */}
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
                Point your camera at the verifier's QR code
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default VerifyIdentity;
