import React  from "react";
import { useState } from "react";
import axios from 'axios';
import {QRCodeCanvas} from 'qrcode.react';


const Verify = () => {

    const [nonce, setNonce] = useState('');
    const [verificationUrl, setVerificationUrl] = useState('');
    const [loading, setLoading] = useState(false);
    const [verificationStatus, setVerificationStatus] = useState('idle');
    const [error, setError] = useState('');



    const startVerification = async (e) => {
        setLoading(true);

        try {
            
           /* const response = await axios.get(``, {
                
            //});
            
            if (response.data.success) {
                const {nonce: newNonce, verificationUrl: url} = response.data;

                setNonce("newNonce");
                setVerificationUrl("http");
                setVerificationStatus('pending');

            }
                */
               setNonce("newNonce");
                setVerificationUrl("http");
        } catch(err) {
            setError('Failed to start verification. Please try again.');
            console.error('Verification error:', err);
        } finally {
            setLoading(false);
        }
        
    }


    return (

        
        <div>
            <div>
                <div>
                    <h2>Digital Identity Verification System</h2>
                </div>

                <div>
                    {!verificationUrl ? (
                        <button 
                        onClick={startVerification}
                        disabled={loading}
                        >
                            {loading ? 'Starting' : 'Start Verification'}
                        </button>
                        ) : (
                            <div>
                            <div className="mb-4 p-3 bg-white rounded border">
                                <QRCodeCanvas
                                    value={JSON.stringify({
                                        url: verificationUrl,
                                        nonce: nonce
                                    })}
                                    size={200}
                                    level="M"
                                    includeMargin
                                    />
                            </div>
                        </div>
                    )
                    
                }
                    
                </div>
            </div>
        </div>
    
    )
    
}

export default Verify;