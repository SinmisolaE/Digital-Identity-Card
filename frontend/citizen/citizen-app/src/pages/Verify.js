import { useState } from "react"


const Verify = () => {


    const [response, setResponse] = useState({});
    const [error, setError] = useState('');

    const jwt = localStorage.getItem('jwt');

    const publicKey = localStorage.getItem('publicKey');

    const sendData = {
        nonce: response.nonce,
        jwt: jwt,
        publicKey: publicKey
    }

    const sendVerification = async (e) => {

        
        try {
            const resp = axios.post(`${response.verificationUrl}`, sendData)
        } catch (error) {
            setError(error);
        }
    }
    return(
        <div className="card">
            <div className="card-body">
                <h2 className="card-title mb-3">
                    Verify your National Identity
                </h2>

                
            </div>

        </div>
    )
}