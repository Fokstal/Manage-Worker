import React, { useState } from "react";
import { useAppDispatch } from "../../hooks/hooks";
import { register } from "../../slices/authSlice";

const Profile = () => {
  const [login, changeLogin] = useState<string>('');
  const [mail, changeMail] = useState<string>('');
  const [password, changePassword] = useState<string>('');

  const dispatch = useAppDispatch();
  return (
    <div className="profile">
      <label htmlFor="login">Login</label>
      <input 
      type="text" 
      id="login"
      value={login} 
      onChange={e => changeLogin(e.target.value)}/>
      <label htmlFor="mail">Mail</label>
      <input 
      type="email"
      id="mail"
      value={mail}
      onChange={e => changeMail(e.target.value)}/>
      <label htmlFor="password">Password</label>
      <input 
      type="password" 
      id="password"
      value={password}
      onChange={e => changePassword(e.target.value)}/>
      <button onClick={() => {
        dispatch(register({
          login : login,
          email : mail,
          password : password,
        }));
      }}>Регистрация</button>
    </div>
  )
};

export default Profile;