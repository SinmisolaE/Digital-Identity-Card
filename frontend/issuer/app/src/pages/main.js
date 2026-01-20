import React from "react";
import { useState, useRef } from "react";
import axios from 'axios';
import {QRCodeSVG} from 'qrcode.react';



const Main = () => {

    const [modal, setModal] = useState(false);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [otherNames, setOtherNames] = useState('');
    const [nationalIdNumber, setIdNumber] = useState('');
    const [gender, setGender] = useState('');
    const [dob, setDateOfBirth] = useState(Date);
    const [placeOfBirth, setPlaceOfBirth] = useState('');
    const [address, setAddress] = useState('');
    const [dateOfIssue, setDateOfIssue] = useState(Date);
    const [expiryDate, setExpiryDate] = useState(Date);
    const [photo, setPhoto] = useState('');
    const [photoPreview, setPhotoPreview] = useState('');

    const [displaySuccess, setDisplaySuccess] = useState(false);
    const [qrScanned, setQrScanned] = useState(false);
    const [error, setError] = useState('');
    const [status, setStatus] = useState(false);

    const [content, setContent] = useState('');
    
    const fileInputRef = useRef(null);
    const cameraInputRef = useRef(null);


    const displayModal = () => {
        setModal(true);
    }

    const resetModal = () => {
        setFirstName('');
        setLastName('');
        setOtherNames('');
        setIdNumber('');
        setGender('');
        setDateOfBirth('');
        setPlaceOfBirth('');
        setAddress('');
        setDateOfIssue('');
        setExpiryDate('');
        setPhoto('');
        setPhotoPreview('');

        setError('');

        setModal(false);
    }
    

    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            if (file.size > 5 * 1024 * 1024) { // 5MB limit
                setError('Photo size should be less than 5MB');
                return;
            }
            
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result;
                setPhoto(base64String);
                setPhotoPreview(base64String);
            };
            reader.readAsDataURL(file);
        }
    };
/*
    const handleFileChange = async (e) => {
        const file = e.target.files[0];
        if (!file) return;
        
        // Check file size
        if (file.size > 5 * 1024 * 1024) { // 5MB limit
            setError('Photo size should be less than 5MB');
            return;
        }
        
        // Clear any previous errors
        setError('');
        
        try {
            // Compress the image for QR code
            const compressedImage = await compressImageForQR(file);
            
            // Also keep a higher quality version for preview
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result;
                setPhoto(compressedImage); // Use compressed for QR
                setPhotoPreview(base64String); // Use original for preview
            };
            reader.readAsDataURL(file);
            
        } catch (error) {
            setError('Failed to process image. Please try again.');
            console.error('Image processing error:', error);
        }
    };

    const compressImageForQR = (file, maxWidth = 64, maxHeight = 64, quality = 0.2) => {
        return new Promise((resolve, reject) => {
            const img = new Image();
            const reader = new FileReader();
            
            reader.onload = (e) => {
                img.src = e.target.result;
                
                img.onload = () => {
                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    
                    // Calculate dimensions while maintaining aspect ratio
                    let width = img.width;
                    let height = img.height;
                    
                    if (width > height) {
                        if (width > maxWidth) {
                            height = Math.round((height * maxWidth) / width);
                            width = maxWidth;
                        }
                    } else {
                        if (height > maxHeight) {
                            width = Math.round((width * maxHeight) / height);
                            height = maxHeight;
                        }
                    }
                    
                    // Set canvas dimensions
                    canvas.width = width;
                    canvas.height = height;
                    
                    // Draw and compress image
                    ctx.drawImage(img, 0, 0, width, height);
                    
                    // Try multiple formats to find smallest
                    let compressedBase64 = canvas.toDataURL('image/webp', quality);
                    
                    // Fallback to JPEG if WebP is too large or not supported
                    if (compressedBase64.length > 3 * 1024) { // More than 3KB
                        compressedBase64 = canvas.toDataURL('image/jpeg', quality * 0.7);
                    }
                    
                    // If still too large, reduce quality further
                    if (compressedBase64.length > 5 * 1024) { // More than 5KB
                        compressedBase64 = canvas.toDataURL('image/jpeg', 0.1);
                    }
                    
                    resolve(compressedBase64);
                };
                
                img.onerror = () => {
                    reject(new Error('Failed to load image'));
                };
            };
            
            reader.onerror = () => {
                reject(new Error('Failed to read file'));
            };
            
            reader.readAsDataURL(file);
        });
    };

    */
    const handleCameraCapture = (e) => {
        cameraInputRef.current.click();
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result;
                setPhoto(base64String);
                setPhotoPreview(base64String);
            };
            reader.readAsDataURL(file);
        }
    };

    const removePhoto = () => {
        setPhoto('');
        setPhotoPreview('');
        if (fileInputRef.current) fileInputRef.current.value = '';
        if (cameraInputRef.current) cameraInputRef.current.value = '';
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatus(true);
        
        // Validate required fields
        if (!firstName.trim() || !lastName.trim() || !nationalIdNumber.trim()) {
            setError('Please fill in all required fields');
            setStatus(false)
            return;
        }
        
        // Prepare form data
        const formData = {
            "FirstName": firstName,
            "LastName": lastName,
            "OtherNames": otherNames,
            "NationalIdNumber": nationalIdNumber,
            "Photo": photo,
            "Gender": gender,
            "DOB": dob,
            "PlaceOfBirth": placeOfBirth,
            "Address": address,
            "DateOfIssue": dateOfIssue,
            "ExpiryDate": expiryDate,
            
        };
        
        try {
            

            const response = await axios.post('https://gateway-mmjm.onrender.com/issuer/issue', formData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            setStatus(false)

            resetModal();

            

            if (response.status === 200) {
                console.log(formData);
                setDisplaySuccess(true);
                console.log(response?.data);
                setContent(response?.data);
                
            } else {
                console.error("Error occured");
                
                setError(response?.data);
                //alert('Failed to issue identity');
            }
            //console.log('Form data:', formData);
            
            // Reset form and close modal on success
            //resetModal();
            //alert('Identity issued successfully!');
        } catch (error) {
            //resetModal();
            setStatus(false);
            console.error('catch error');
            console.error('Error:', error.response?.data);
            //setError("problem");
            setError(error.response?.data);

            
            //alert('Failed to issue identity');
        }
    };
    

    const resetIssue = () => {
        setQrScanned(false);
        setContent('');
        setError('');
        setDisplaySuccess(false);
    }

    const handleIssueDone = () => {
        setDisplaySuccess(false);
    }



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

                    

                    {displaySuccess && (
                        <div className="text-center">
                            <div className="alert alert-success mb-3">
                                Identity issued successfully!
                            </div>
                            {!qrScanned ? (

                                <>
                            
                                <p>Scan below to save identity on wallet</p>
                                
                                <div className="p-3 bg-white rounded border shadow-sm qr-container" 
                                    >
                                    <QRCodeSVG
                                        value={JSON.stringify({jwt: content})}
                                        size={400}//{Math.min(250, window.innerWidth * 0.8)}
                                        level="H"
                                        includeMargin={true}
                                    />
                                </div>

                                <div className="mb-3">
                                    <button
                                        className="btn btn-primary"
                                        onClick={resetIssue}
                                    >
                                        Scan Complete
                                    </button>
                                </div>
                                
                                </>
                            ) : (
                                <div className="alert alert-info">
                                    <h5>✓ Identity Saved to Wallet!</h5>
                                    <p>The digital identity has been successfully transferred.</p>
                                    <button 
                                        className="btn btn-primary"
                                        onClick={handleIssueDone}
                                    >
                                        Close
                                    </button>
                                </div>
                            )}
                        </div>
                    )}

                    <div className="text-center mt-3">
                        <p className="mt-2">
                            Click below to issue a new Identity
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

                                            {error && (
                                                <div className="alert alert-danger">{error}</div>
                                            )}

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
                                                        value={nationalIdNumber}
                                                        onChange={(e) => setIdNumber(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter national ID number"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Photo</label>
                                                    <div className="d-flex gap-2 mb-2">
                                                        <button
                                                            type="button"
                                                            className="btn btn-outline-primary"
                                                            onClick={() => fileInputRef.current?.click()}
                                                        >
                                                            📁 Upload Photo
                                                        </button>
                                                        <button
                                                            type="button"
                                                            className="btn btn-outline-primary"
                                                            onClick={() => cameraInputRef.current?.click()}
                                                        >
                                                            📷 Take Photo
                                                        </button>
                                                    </div>
                                                    <input
                                                        ref={fileInputRef}
                                                        type="file"
                                                        accept="image/*"
                                                        onChange={handleFileChange}
                                                        style={{ display: 'none' }}
                                                    />
                                                    <input
                                                        ref={cameraInputRef}
                                                        type="file"
                                                        accept="image/*"
                                                        capture="environment"
                                                        onChange={handleCameraCapture}
                                                        style={{ display: 'none' }}
                                                    />
                                                    {photoPreview && (
                                                        <div className="mt-2 position-relative d-inline-block">
                                                            <img
                                                                src={photoPreview}
                                                                alt="Preview"
                                                                style={{
                                                                    maxWidth: '200px',
                                                                    maxHeight: '200px',
                                                                    objectFit: 'cover',
                                                                    borderRadius: '8px'
                                                                }}
                                                            />
                                                            <button
                                                                type="button"
                                                                className="btn btn-danger btn-sm position-absolute top-0 end-0 m-1"
                                                                onClick={removePhoto}
                                                                style={{ fontSize: '0.75rem' }}
                                                            >
                                                                ✕
                                                            </button>
                                                        </div>
                                                    )}
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
                                                        value={dob}
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
                                                    <label className="form-label">Address</label>
                                                    <input 
                                                        type="text" 
                                                        value={address}
                                                        onChange={(e) => setAddress(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter address"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Date of Issue</label>
                                                    <input 
                                                        type="date" 
                                                        value={dateOfIssue}
                                                        onChange={(e) => setDateOfIssue(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter the date of issue"
                                                        required
                                                    />
                                                </div>

                                                <div className="mb-3">
                                                    <label className="form-label">Expiry Date</label>
                                                    <input 
                                                        type="date" 
                                                        value={expiryDate}
                                                        onChange={(e) => setExpiryDate(e.target.value)}
                                                        className="form-control"
                                                        placeholder="Enter Date of Expiry"
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
                                                {status ? 'Issuing...' : 'Issue Identity'}
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