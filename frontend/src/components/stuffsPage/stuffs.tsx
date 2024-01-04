import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { getStuffs } from "../../slices/stuffSlice";

const Stuffs = () => {
  const dispatch = useAppDispatch();
  const stuffs = useAppSelector(state => state.stuff.stuffs);

  useEffect(() =>{
    dispatch(getStuffs());      
  // eslint-disable-next-line react-hooks/exhaustive-deps
  },[])

  const stuffsRender = stuffs.map((stuff) => {
    return (
      <div>{stuff.name}</div>
    )
  });
  return (
    <>{stuffsRender}</>
  )
};

export default Stuffs;