import React  from "react";
import { useState, useRef } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';
import {QRCodeSVG} from 'qrcode.react';


const Verify = () => {
    const navigate = useNavigate();

    const [nonce, setNonce] = useState('');
    const [verificationUrl, setVerificationUrl] = useState('');
    const [loading, setLoading] = useState(false);
    const [verificationStatus, setVerificationStatus] = useState('idle');
    const [error, setError] = useState('');
    const [userData, setUserData] = useState(null);
    //const [pollingCount, setPollingCount] = useState(0);

    const pollingCountRef = useRef(0);



    const startVerification = async (e) => {
        setLoading(true);
        setError('');
        setVerificationStatus('idle');

        try {
            
            const response = await axios.get('http://localhost:5091/verifier/challenge', {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            if (response.data.success) {

                console.log(`COntent: ${response.data}\n`);

                console.log(`Nonce from be: ${response.data.nonce}\n\n`);

                

                const {nonce: newNonce, verificationUrl: url} = response.data;

                setNonce(newNonce);
                

                setVerificationUrl(url);

                console.log(newNonce);
                console.log(url);

                setVerificationStatus('waiting');
                //setPollingCount(0);

                startPolling(newNonce);

            } else {
                setError("An error has occured");
                console.error("response data is not success");
            }
               
        } catch(err) {
            setError('Failed to start verification. Please try again.');
            setVerificationStatus('failed');
            console.error('Verification error:', err);
        } finally {
            setLoading(false);
        }
        
    }


    const startPolling = (currentNonce) => {
        const poll = async () => {
            try {

                const response = await axios.get(`http://localhost:5091/verifier/status?nonce=${currentNonce}`);

                console.log(response?.data);
                const {status, citizenData, isCompleted } = response.data;


                //setPollingCount(prev => prev + 1);

                pollingCountRef.current++;
                console.log(`Poll count: ${pollingCountRef.current}`);

                if (status == 'verified' && citizenData) {
                    setVerificationStatus('verified');
                    setUserData(citizenData);
                    console.log(citizenData.photo);
                }else if (status === 'pending' && pollingCountRef.current < 50) {
                    setTimeout(() => poll(), 2000);
                } else if (status === 'failed') {
                    setVerificationStatus('failed');
                } else if (pollingCountRef.current < 50) {
                    setTimeout(() => poll(), 2000);
                }else {
                    setVerificationStatus('failed');
                }


                    
            } catch (err) {
                //setPollingCount(prev => prev + 1);
                pollingCountRef.current++;
                if (pollingCountRef.current < 50) {
                    setTimeout(() => poll(), 5000);
                } else {
                    setVerificationStatus('failed');
                }
            }
                
        };
        poll();
    };

    const reset = async (e) => {
        setLoading(false);
        setVerificationStatus('idle');
        setVerificationUrl('');
    }

    const handleLogout = async (e) => {
        navigate('/login');
    }


    return (



        <div className="min-vh-100 ">

            <nav className="navbar navbar-expand-lg navbar-light bg-white mb-4 shadow-sm">

                <div className="container">
                    <span className="navbar-brand fw-bold text-primary ">Verifier Portal</span>
                    <button className="btn btn-outline-danger" onClick={handleLogout}>
                        Logout
                    </button>
                </div>
            </nav>

        <div className="container-fluid mt-3">
            <div className="row justify-content-center">
                <div className="col-md-6 col-lg-5">

                
                    <div className="text-center mb-5">
                        <h1> Identity Verification Portal</h1>
                        <p className="lead text-muted">Secure credentials validation system</p>
                    </div>

                    {error && (
                        <div className="alert alert-danger">{error}</div>
                    )}

                    {verificationStatus === 'failed' && (
                        <div className="text-center">
                        <div className="alert alert-danger">
                            <strong>Verification Failed</strong><br/>
                            <small>Please try again or contact support.</small>
                        </div>

                        

                            <button className="btn btn-primary btn-lg px-5 py-2" onClick={reset}>Try Again</button>
                        </div>
                    )}

                        {!verificationUrl && verificationStatus !== 'waiting' && (

                            <div className="text-center mt-3 my-5">
                                <p>
                                    Click below to generate a Verification QR code
                                </p>
                                <div className="my-5">
                                    <button 
                                        onClick={startVerification}
                                        disabled={loading}
                                        
                                        className="btn btn-primary btn-lg px-5 py-4"
                                        >
                                        {loading ? 'Starting' : 'Start Verification'}
                                    </button>
                                </div>
                            </div>
                        )}

                        
                        {verificationUrl && verificationStatus === 'waiting' && (

                            <div className="text-center">
                                    <div className=" px-3 mb-4 w-70">
                                        <div className=" rounded border">
                                            <QRCodeSVG
                                                value={JSON.stringify({
                                                    verifierUrl: verificationUrl,
                                                    nonce: nonce
                                                })}
                                                size={250}
                                                level="H"
                                                includeMargin/>
                                        </div>
                                    </div>

                                    <div className="mb-4">
                                        <button
                                            className="btn btn-primary w-100 mg-3"
                                            onClick={startVerification}
                                            disabled={loading}
                                            >
                                            Start New Verification
                                            </button>
                                    </div>

                                    <div className="mt-4">
                                        <div className="alert alert-info small text-start">
                                            <strong>How to Verify:</strong>

                                            <ol>
                                                <li>Citizen scans QR Code</li>
                                                <li>Verification would be detected automatically</li>
                                            </ol>
                                        </div>
                                    </div>
                             </div>   
                             
                        )}
                        
                        {verificationStatus === 'verified' && userData && (
                            <div className="text-center">
                                <div className="alert alert-success mb-4">
                                    <strong>✓ Verification Successful</strong>
                                </div>
                                
                                <div className="card mx-auto" style={{maxWidth: '500px'}}>
                                    <div className="card-header bg-primary text-white">
                                        <h5 className="card-title mb-0">
                                            <i className="fas fa-id-card me-2"></i>
                                            Verified Identity
                                        </h5>
                                    </div>
                                    <div className="card-body">
                                        <div className="row">
                                            <div className="col-md-4 text-center mb-3">
                                                {console.log(userData.photo)}
                                                {userData.photo ? (
                                                
                                                    <img 
                                                        src={`http://localhost:5091/issuer${userData.photo}`} 
                                                        alt="Profile" 
                                                        className="img-thumbnail rounded-circle"
                                                        style={{width: '100px', height: '100px', objectFit: 'cover'}}
                                                    />
                                                ) : (
                                                     <div className="bg-secondary rounded-circle d-flex align-items-center justify-content-center text-white" 
                                                         style={{width: '100px', height: '100px', margin: '0 auto'}}>
                                                        <i className="fas fa-user fa-2x"></i>
                                                    </div>
                                                )}
                                            </div>
                                            <div className="col-md-8">
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>Last Name:</strong></div>
                                                    <div className="col-7"><strong>{userData.lastName || 'N/A'}</strong></div>
                                                </div>
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>First Name:</strong></div>
                                                    <div className="col-7"><strong>{`${userData.firstName} ${userData.otherNames}` || 'N/A'}</strong></div>
                                                </div>
                                                
                                                
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>ID Number:</strong></div>
                                                    <div className="col-7"><strong>{userData.nationalIdNumber || 'N/A'}</strong></div>
                                                </div>
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>Date of Birth:</strong></div>
                                                    <div className="col-7"><strong>{userData.dob || 'N/A'}</strong></div>
                                                </div>
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>Gender:</strong></div>
                                                    <div className="col-7"><strong>{userData.gender || 'N/A'}</strong></div>
                                                </div>
                                                <div className="row mb-2">
                                                    <div className="col-5"><strong>Date of Issue:</strong></div>
                                                    <div className="col-7"><strong>{userData.dateOfIssue || 'N/A'}</strong></div>
                                                </div>
                                                <div className="row mb-2">
                                                        <div className="col-5"><strong>Date of Expiry:</strong></div>
                                                        <div className="col-7"><strong>{userData.expiryDate}</strong></div>
                                                </div>
                                                
                                                {userData.address && (
                                                    <div className="row mb-2">
                                                        <div className="col-5"><strong>Address:</strong></div>
                                                        <div className="col-7"><strong>{userData.address}</strong></div>
                                                    </div>
                                                )}
                                                
                                                {userData.placeOfBirth && (
                                                    <div className="row mb-2">
                                                        <div className="col-5"><strong>Place of Birth</strong></div>
                                                        <div className="col-7"><strong>{userData.placeOfBirth}</strong></div>
                                                    </div>
                                                )}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="card-footer bg-light">
                                        <small className="text-muted">
                                            <i className="fas fa-shield-alt text-success me-1"></i>
                                            Verified on {new Date().toLocaleString()}
                                        </small>
                                    </div>
                                </div>

                                <div className="mt-4">
                                    <button
                                        className="btn btn-primary me-2"
                                        onClick={startVerification}
                                        disabled={loading}
                                    >
                                        Verify Another Identity
                                    </button>
                                    <button
                                        className="btn btn-outline-secondary"
                                        onClick={() => {
                                            setVerificationStatus('idle');
                                            setUserData(null);
                                            setVerificationUrl('');
                                            setNonce('');
                                        }}
                                    >
                                        Reset
                                    </button>
                                </div>
                            </div>
                        )}
                        
                    </div>
                </div>
            </div>
        </div>
    
    );
    
}

export default Verify;