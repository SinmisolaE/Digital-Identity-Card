import React  from "react";
import { useState } from "react";
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
    const [pollingCount, setPollingCount] = useState(0);



    const startVerification = async (e) => {
        setLoading(true);
        setVerificationStatus('idle');

        try {
            
            const response = await axios.get('http://localhost:5091/verifier/challenge', {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            if (response.data.success) {

                console.log(`COntent: ${response.data}\n\n`);

                console.log(`Nonce from be: ${response.data.nonce}`);

                

                const {nonce: newNonce, verificationUrl: url} = response.data;

                setNonce(nonce);
                setVerificationUrl(verificationUrl);
                setVerificationStatus('pending');

                setVerificationStatus('waiting');
                setPollingCount(0);

                startPolling(nonce);

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
                //const response = await axios.get('localhost?nonce=${currentNonce}')
                //const {status : serverStatus, userData: serverUserData} = Response.data;
/*

                setPollingCount(prev => prev + 1);

                if (serverStatus === 'verfied') {
                    setVerificationStatus('verified');
                    setUserData(userData);
                } else if (serverStatus == 'failed') {
                    setVerificationStatus('failed');
                } else if (pollingCount < 30) {
                    setTimeout(() => poll(), 2000);
                }else {
                    setVerificationStatus('failed');
                }
                    */
            } catch (err) {
                if (pollingCount < 30) {
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
                        <div className="alert alert-danger">
                            <strong>Verification Failed</strong><br/>
                            <small>Please try again or contact support.</small>
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
                                                    url: verificationUrl,
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
                        
                        
                    </div>
                </div>
            </div>
        </div>
    
    );
    
}

export default Verify;