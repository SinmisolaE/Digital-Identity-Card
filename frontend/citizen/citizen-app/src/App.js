
import './App.css';

import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import ReceiveCredential from './pages/receiveCredential';
import Layout from './component/Layout';
import Verify from './pages/Verify';




function App() {
  return (
    <Router>
      <Layout>

        <Routes>

          <Route path="/" element={<ReceiveCredential/>}/>
          <Route path="/verify" element={<Verify/>}/>
          <Route/>
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;
