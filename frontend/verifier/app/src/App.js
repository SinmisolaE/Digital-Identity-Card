import './App.css';

import {BrowserRouter as Router, Routes, Route} from "react-router-dom";

import Homepage from "./components/homepage";
import Verify  from './components/Verify';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Homepage/>}/>
        <Route path="/verify" element={<Verify/>}/>
      </Routes>
    </Router>
  );
}

export default App;
