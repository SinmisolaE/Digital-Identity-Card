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

        if ((email === "admin@gmail.com") && password === 'admin'){
            
            navigate('./verify');
        }
        
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
        <div className="min-vh-100 d-flex align-items-center bg-background">

        <div className="container-fluid ">
            <div className="row justify-content-center ">
                <div className="col-md-6 col-lg-5">

                  <div className="card shadow-lg">
                    <div className="card-body p-5">
                    <div className="text-center mb-4">
                        <h1 className="mb-2">Verifier Portal</h1>
                        <p className="text-muted">Digital Identity Verification System</p>
                    </div>

                    {error && (
                        <div className="alert alert-danger">{error}</div>
                    )}
                

                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">

                            <label
                                className="form-label"
                            >
                                Email
                            </label>
                            <input
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)} 
                                className="form-control "
                                placeholder="name@example.com"
                                required
                            />
                        </div>

                        <div className="mb-4">
                            <label
                                className="form-label"
                            >
                                Password
                            </label>
                            <input 
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="form-control"
                                placeholder="Enter your password"
                                required

                            />
                        </div>

                        <div className="">
                            <button type="submit"
                                className="btn btn-primary mb-3 w-100"
                            >
                                Sign In
                            </button>
                        </div>

                    </form>
                    </div>
                </div>
                </div>
            </div>


        </div>
        </div>

    )
};

export default Homepage;