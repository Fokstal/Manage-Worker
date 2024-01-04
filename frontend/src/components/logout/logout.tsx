import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../hooks/hooks";
import { logout } from "../../slices/authSlice";

const Logout = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  dispatch(logout());
  
  useEffect(() => {
    navigate('/');
  });
  return (
    <></>
  )
}

export default Logout;