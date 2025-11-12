import React, { useState, useRef, useEffect } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';
import {QRCodeSVG} from 'qrcode.react';


const AutoVerify = () => {
    const navigate = useNavigate();

    const [nonce, setNonce] = useState('');
    const [verificationUrl, setVerificationUrl] = useState('');
    const [loading, setLoading] = useState(false);
    const [verificationStatus, setVerificationStatus] = useState('idle');
    const [error, setError] = useState('');
    const [userData, setUserData] = useState(null);
    const [doorOpen, setDoorOpen] = useState(false);
 //const [pollingCount, setPollingCount] = useState(0);
 
     const pollingCountRef = useRef(0);
     const restartTimerRef = useRef(null);
 
    // when verification becomes 'verified' => speak and animate door
    useEffect(() => {
        if (verificationStatus === 'verified') {
            // speak "Access granted"
            try {
                const text = 'Access granted';
                if (window && window.speechSynthesis) {
                    window.speechSynthesis.cancel();
                    const utterance = new SpeechSynthesisUtterance(text);
                    window.speechSynthesis.speak(utterance);
                }
            } catch (e) {
                console.warn('Speech synthesis failed', e);
            }

            // start door animation, keep it open for 6s
            setDoorOpen(true);
            const t = setTimeout(() => setDoorOpen(false), 6000);
            return () => clearTimeout(t);
        } else if (verificationStatus === 'failed') {
            try {
                const text = 'Access denied';
                if (window && window.speechSynthesis) {
                    window.speechSynthesis.cancel();
                    const utterance = new SpeechSynthesisUtterance(text);
                    window.speechSynthesis.speak(utterance);
                }
            } catch (e) {
                console.warn('Speech synthesis failed', e);
            }

            // start door animation, keep it open for 6s
            setDoorOpen(false);
            const t = setTimeout(() => setDoorOpen(false), 6000);
            return () => clearTimeout(t);
        }
    }, [verificationStatus]);

    useEffect(() => {
        // auto-start verification when component mounts
        startVerification();

        // eslint-disable-next-line react-hooks/exhaustive-deps
    

        // cleanup timers on unmount
        return () => {
            if (restartTimerRef.current) {
                clearTimeout(restartTimerRef.current);
                restartTimerRef.current = null;
            }
        };
    }, []);

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


                pollingCountRef.current++;
                console.log(`Poll count: ${pollingCountRef.current}`);

                if (status === 'verified' && citizenData) {
                    console.log(citizenData);
                    console.log(citizenData.expiryDate);
                    // mark verified, show UI and schedule automatic restart after 6s
                    pollingCountRef.current = 0;
                    setVerificationStatus('verified');
                    setUserData(citizenData);

                    // avoid scheduling multiple restarts
                    if (restartTimerRef.current) {
                        clearTimeout(restartTimerRef.current);
                        restartTimerRef.current = null;
                    }

                    restartTimerRef.current = setTimeout(() => {
                        // clear current verified state and start a fresh verification cycle
                        setUserData(null);
                        setVerificationUrl('');
                        setNonce('');
                        setVerificationStatus('idle');
                        restartTimerRef.current = null;
                        startVerification();
                    }, 6000);
                 }else if (status === 'pending' && pollingCountRef.current) {
                     setTimeout(() => poll(), 2000);
                 } else if (status === 'expired') {
                    startVerification();
                 }
                  else if (status === 'failed') {
                     setVerificationStatus('failed');
                     //display sound of 'access denied' and image of door locked
                     startVerification();
                 } else if (pollingCountRef.current) {
                     setTimeout(() => poll(), 2000);
                 }else {
                     setVerificationStatus('failed');
                    startVerification();
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
         if (restartTimerRef.current) {
            clearTimeout(restartTimerRef.current);
            restartTimerRef.current = null;
        }
     }

    const handleLogout = async (e) => {
        navigate('/login');
    }


    return (



        <div className="min-vh-100 ">

            <nav className="navbar navbar-expand-lg navbar-light bg-white mb-4 shadow-sm">

                <div className="container">
                    <span className="navbar-brand fw-bold text-primary ">Verifier Portal</span>
                </div>
            </nav>

        <div className="container-fluid mt-3">
            <div className="row justify-content-center">
                <div className="col-md-6 col-lg-5">

            

                    {error && (
                        <div className="alert alert-danger">{error}</div>
                    )}

                    <div className="text-center mb-3">
                        <strong>Scan qrcode to grant access</strong>
                    </div>

                    {verificationStatus === 'failed' && (
                        <div className="text-center">
                        <div className="alert alert-danger">
                            <strong>Verification Failed</strong><br/>
                            <small>Please try again or contact support.</small>
                        </div>

                        

                            <button className="btn btn-primary btn-lg px-5 py-2" onClick={reset}>Try Again</button>
                        </div>
                    )}

                    {/* removed manual start button - verification now auto-starts on mount */}
                        
                    {loading && !verificationUrl && (
                            <div className="text-center mt-3 my-5">
                                <p>Generating verification QR code...</p>
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

                                    

                                
                             </div>   
                             
                    )}
                        
                    {verificationStatus === 'verified' && userData && (
                            <div className="text-center">
                                <div className="alert alert-success mb-3">
                                    <strong>✓ Verification Successful</strong>
                                </div>

                               <div className="alert alert-success mb-3">
                                    <strong>Welcome {`${userData.firstName} ${userData.otherNames}`}</strong>
                              </div>

                                {/* inline styles for the door animation */}
                                <div style={{margin: '0 auto', width: 220}}>
                                    <style>{`
                                        .door-wrap { width: 220px; height: 280px; margin: 0 auto; perspective: 1000px; }
                                        .door { width: 100%; height: 100%; position: relative; transform-style: preserve-3d; }
                                        .door-panel { position: absolute; top: 0; width: 50%; height: 100%; background: linear-gradient(#7b5746,#513829); box-shadow: inset 0 0 10px rgba(0,0,0,0.35); }
                                        .door-left { left: 0; transform-origin: left center; border-right: 2px solid rgba(0,0,0,0.18); transition: transform 900ms cubic-bezier(.2,.9,.2,1); }
                                        .door-right { right: 0; transform-origin: right center; border-left: 2px solid rgba(0,0,0,0.18); transition: transform 900ms cubic-bezier(.2,.9,.2,1); }
                                        .door-handle { position: absolute; top: 46%; width: 10px; height: 26px; background: #cbb089; border-radius: 3px; }
                                        .door-left .door-handle { right: 12px; }
                                        .door-right .door-handle { left: 12px; }
                                        .door.open .door-left { transform: rotateY(-75deg) translateX(-8%); }
                                        .door.open .door-right { transform: rotateY(75deg) translateX(8%); }
                                        .door-caption { text-align: center; font-size: 0.9rem; color: #444; margin-top: 8px; }
                                    `}</style>

                                    <div className={`door-wrap`}>
                                        <div className={`door ${doorOpen ? 'open' : ''}`} aria-hidden={!doorOpen}>
                                            <div className="door-panel door-left">
                                                <div className="door-handle" />
                                            </div>
                                            <div className="door-panel door-right">
                                                <div className="door-handle" />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="door-caption">{doorOpen ? 'Door opening...' : 'Door closed'}</div>
                                </div>
                           </div>
                    )}
                    </div>
                </div>
            </div>
        </div>
    
    );
    
}

export default AutoVerify;