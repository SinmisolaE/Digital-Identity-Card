import './App.css';

import {BrowserRouter as Router, Routes, Route} from "react-router-dom";

import Homepage from "./components/homepage";
import Verify  from './components/Verify';
import AutoVerify from './components/AutoVerify';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Verify/>}/>
        <Route path="/verify" element={<Verify/>}/>
        <Route path="/autoverify" element={<AutoVerify/>}/>
      </Routes>
    </Router>
  );
}

export default App;
