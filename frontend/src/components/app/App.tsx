import React from 'react';
import './App.css';
import Header from '../header/header';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Profile from '../profile/profile';
import SignUp from '../registrationPage/registration';
import SignIn from '../loginPage/login';
import Logout from '../logout/logout';

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
