import React, { use }  from "react";
import { useState } from "react";
import axios from 'axios';



const Main = () => {

    const [modal, setModal] = useState(false);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [otherNames, setOtherNames] = useState('');
    const [idNumber, setIdNumber] = useState('');
    const [nationality, setNationality] = useState('');
    const [gender, setGender] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState(Date);
    const [placeOfBirth, setPlaceOfBirth] = useState('');
    const [address, setAddress] = useState('');
    const [dateOfIssue, setDateOfIssue] = useState('');
    const [expiryDate, setExpiryDate] = useState('');
    const [publicKey, setPublicKey] = useState('');

    const [displaySuccess, setDisplaySuccess] = useState(false);
    const [error, setError] = useState('');
    const [status, setStatus] = useState(false);
    


    const displayModal = () => {
        setModal(true);
    }

    const resetModal = () => {
        setModal(false);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatus(true);
        
        // Validate required fields
        if (!firstName.trim() || !lastName.trim() || !idNumber.trim()) {
            alert('Please fill in all required fields');
            return;
        }
        
        // Prepare form data
        const formData = {
            firstName,
            lastName,
            otherNames,
            idNumber,
            nationality,
            gender,
            dateOfBirth,
            placeOfBirth,
            address,
            dateOfIssue,
            expiryDate,
            publicKey
        };
        
        try {
            

            const response = await axios.post('http://localhost:5091/issuer/issue', {
                Headers: {
                    'Content-Type': 'application/json'
                },
                body: {
                   formData
                }
            });

            resetModal();

            if (response.status === 200) {
                setDisplaySuccess(true);
                
            } else {
                console.error("Error occured");
                setError(response.data);
                alert('Failed to issue identity');
            }
            console.log('Form data:', formData);
            
            // Reset form and close modal on success
            resetModal();
            alert('Identity issued successfully!');
        } catch (error) {
            //resetModal();
            console.error('Error:', error.response?.data);
            setError(error);

            
            alert('Failed to issue identity');
        }
    };
    

    return(
        <div className="miv-vh-100">

            <nav className="navbar navbar-expand-lg navbar-light bg-white mb-4 shadow-sm">

                <div className="container">
                    <span className="navbar-brand fw-bold text-primary ">Issuer Portal</span>
                    <button className="btn btn-outline-danger" >
                        Logout
                    </button>
                </div>
            </nav>

            <div className="container">
                <div className="row justify-content-center">
                    <div className="text-center mb-4 mt-4">
                        <h1>Issuance of Digital Identity</h1>
                        <p>Secure Identity Issuance system</p>
                    </div>

                    {error && (
                        <div className="alert alert-danger">{error}</div>
                    )}

                    {displaySuccess && (
                        <div className="alert alert-success">Identity issued successfully!</div>
                    )}

                    <div className="text-center mt-3">
                        <p>
                            Click below to issue an Identity
                        </p>
                        <button 
                            onClick={displayModal}
                            className="btn btn-primary btn-lg w-25"
                        >
                            Issue 
                        </button>
                    </div>


                    {modal && (
                        <>
                            <div className="modal-backdrop fade show"></div>
                            <div className="modal fade show d-block" tabIndex="-1">
                                <div className="modal-dialog modal-lg">
                                    <div className="modal-content">
                                        <div className="modal-header">
                                            <h5 className="modal-title">Issue Digital Identity</h5>
                                            <button 
                                                type="button" 
                                                className="btn-close" 
                                                onClick={resetModal}
                                                aria-label="Close"
                                            ></button>
                                        </div>

                                       

                                        <div className="modal-body">
                                            <form id="identityForm">

                                                <div className="mb-3">
                                                    <label className="form-label">First Name</label>
                                                    <input 
                                                        type="text" 
                                                        value={firstName}
                                                        onChange={(e) => setFirstName(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter first name"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Last Name</label>
                                                    <input 
                                                        type="text" 
                                                        value={lastName}
                                                        onChange={(e) => setLastName(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter last name"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Other Names</label>
                                                    <input 
                                                        type="text" 
                                                        value={otherNames}
                                                        onChange={(e) => setOtherNames(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter other names (optional)"
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">ID Number</label>
                                                    <input 
                                                        type="text" 
                                                        value={idNumber}
                                                        onChange={(e) => setIdNumber(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter national ID number"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Nationality</label>
                                                    <input 
                                                        type="text" 
                                                        value={nationality}
                                                        onChange={(e) => setNationality(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter nationality"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Gender</label>
                                                    <select 
                                                        value={gender}
                                                        onChange={(e) => setGender(e.target.value)}
                                                        className="form-control"
                                                        required
                                                    >
                                                        <option value="">Select gender</option>
                                                        <option value="Male">Male</option>
                                                        <option value="Female">Female</option>
                                                        <option value="Other">Other</option>
                                                    </select>
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Date of Birth</label>
                                                    <input 
                                                        type="date" 
                                                        value={dateOfBirth}
                                                        onChange={(e) => setDateOfBirth(e.target.value)}
                                                        className="form-control"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Place of Birth</label>
                                                    <input 
                                                        type="text" 
                                                        value={placeOfBirth}
                                                        onChange={(e) => setPlaceOfBirth(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter place of birth"
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label className="form-label">Verification Key</label>
                                                    <input 
                                                        type="text" 
                                                        value={publicKey}
                                                        onChange={(e) => setPublicKey(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter verification key"
                                                        required
                                                    />
                                                </div>

                                            </form>
                                        </div>

                                           

                                        <div className="modal-footer">
                                            <button 
                                                type="button" 
                                                className="btn btn-secondary" 
                                                onClick={resetModal}
                                            >
                                                Cancel
                                            </button>
                                            <button 
                                                type="button" 
                                                className="btn btn-primary"
                                                form="identityForm"
                                                onClick={handleSubmit}
                                            >
                                                Issue Identity
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </>
                    )}




                </div>
            </div>
        </div>
    )
};

export default Main;