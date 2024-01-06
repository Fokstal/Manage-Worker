import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Button, Grid, Typography } from "@mui/material";
import StuffService from "../../services/stuffService";
import stuff from "../../types/stuff";
import { useAppSelector } from "../../hooks/hooks";

const Stuff = () => {
  const {stuffId} = useParams();
  const [item, setItem] = useState<stuff>();
  const isAuth = useAppSelector(state => state.auth.isAuth);

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
      <Grid sx={{gap: '10px', display: 'flex'}}>
        {isAuth ? <Button variant="contained">Change</Button> : null}
        {isAuth ? 
        <Button 
          variant="contained" 
          color="error"
          onClick={() => {
            const service = new StuffService();
            if (stuffId) {
              service.deleteStuff(+stuffId)
              .then(() => alert("Успешно удаленно"));
            }
          }}>
            Delete
        </Button> 
        : 
        null}
      </Grid>
    </>
  )
};

export default Stuff;