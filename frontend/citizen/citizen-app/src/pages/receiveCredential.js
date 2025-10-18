import React from "react";



const ReceiveCredential = () => {





    return(
            <div>
                <div className="card">
                    <div className="card-body">
                        <h2 className="card-title">
                            Verification Key
                        </h2>
                        <span>This is your unique identifier for receiving credentials</span>

                        <div>
                            <p>{publicKey}</p>
                        </div>
                    </div>

                </div>


                <div className="card">
                    <div className="card-body">
                        <h2 className="card-title">
                            Scan QrCode to get Credentials
                        </h2>

                        <div>
                            
                        </div>

                    </div>

                </div>
            </div>
    )
};

export default ReceiveCredential;

