import logo from './logo.svg';
import './App.css';

import {BrowserRouter, Router, Routes, Route} from "react-router-dom";
import ReceiveCredential from './pages/receiveCredential';
import Layout from './component/Layout';




function App() {
  return (
    <Router>
      <Route path="/" element={<ReceiveCredential/>}/>
      <Route/>
    </Router>
  );
}

export default App;
