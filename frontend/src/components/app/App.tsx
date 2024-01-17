import React, { useEffect } from 'react';
import './App.css';
import Header from '../header/header';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Profile from '../profile/profile';
import SignUp from '../registrationPage/registration';
import SignIn from '../loginPage/login';
import Logout from '../logout/logout';
import Stuffs from '../stuffsPage/stuffs';
import Stuff from '../stuffPage/stuff';
import Workers from '../workersPage/workers';
import Worker from '../workerPage/worker';
import AuthService from '../../services/AuthService';

function App() {
  console.log(process.env.REACT_APP_IP); 
  const refreshToken = () => {
    const service = new AuthService();
    service.refreshToken();
  };

  useEffect(() => {
    refreshToken();
  },[])

  return (
    <div className="App">
      <BrowserRouter>
      <Header/>
        <Routes>
          <Route path='*' element={<div>Error...</div>}/>
          <Route path='/' element={<div>Main page</div>}/>
          <Route path='/workers' element={<Workers/>}/>
          <Route path='/workers/:workerId' element={<Worker/>}/>
          <Route path='/stuff' element={<Stuffs/>}/>
          <Route path='/stuff/:stuffId' element={<Stuff/>}/>
          <Route path='/profile' element={<Profile/>}/>
          <Route path='/registration' element={<SignUp/>}/>
          <Route path='/login' element={<SignIn/>}/>
          <Route path='/logout' element={<Logout/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
