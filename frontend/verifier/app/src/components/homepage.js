import React from "react";
import { useState } from "react";
import {useNavigate} from "react-router-dom";
import axios from 'axios';


const Homepage = () => {
    const navigate = useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        
        try {
            const response = await axios.post(`http`, {
                email, 
                password
            });

            if (response.data.success) {
                alert('Login successfull');
                const data = await response.json();
                localStorage.setItem('email', data["email"]);
                navigate('./verify');
            } else {
                setError("email or password incorrect");
                setLoading(false);
            }
        } catch(error) {
            if (error.response) {
                setError(error.response.data.message || 'Login failed')
            } else if (error.request) {
                setError("No response from server");
            } else {

                setError(error.body);
            }
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="container">
            <div className="row justify-content-center">

            <div className="text-center mb-4">
                <h2 className="text pt-5">Digital Identity Verification System</h2>
            </div>

            <div className="card ">
                <h2>Login</h2>
                <form onSubmit={handleSubmit}>
                    <div>

                        <label>Email</label>
                        <input type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)} 
                        />
                    </div>

                    <div>
                        <label>Password</label>
                        <input 
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>

                    <div>
                        <button type="submit">Submit</button>
                    </div>

                </form>
                </div>
            </div>


        </div>
    )
};

export default Homepage;