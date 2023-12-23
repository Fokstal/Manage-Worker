import React from 'react';
import './App.css';
import Header from '../header/header';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
      <Header/>
        <Routes>
          <Route path='*' element={<div>Error...</div>}/>
          <Route path='/' element={<div>Main page</div>}/>
          <Route path='/workers' element={<div>Workers</div>}/>
          <Route path='/stuff' element={<div>Stuff</div>}/>
          <Route path='/profile' element={<div>Profile</div>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
