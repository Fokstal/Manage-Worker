import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";
import StuffService from "../../services/stuffService";
import stuff from "../../types/stuff";

const Stuff = () => {
  const {stuffId} = useParams();
  const [item, setItem] = useState<stuff>();

  const updateItem = () => {
    const service = new StuffService();
    service.getStuff(+(stuffId ?? '1'))
    .then((stuff) => {
      setItem(stuff);
    })
  };

  useEffect(() => {
    updateItem();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])
  return (
    <>
      <Typography 
        variant="h3"
        sx={{
          mr: 2,
          display: { xs: 'none', md: 'flex' },
          fontFamily: 'monospace',
          fontWeight: 700,
          letterSpacing: '.3rem',
          color: 'inherit',
          textDecoration: 'none',
        }}>
        {`Название - ${item?.name}`}
      </Typography>
      <Typography 
        variant="h3"
        sx={{
          mr: 2,
          display: { xs: 'none', md: 'flex' },
          fontFamily: 'monospace',
          fontWeight: 700,
          letterSpacing: '.3rem',
          color: 'inherit',
          textDecoration: 'none',
        }}>
        {`Колличество работников - ${item?.countWorker}`}
      </Typography>
    </>
  )
};

export default Stuff;