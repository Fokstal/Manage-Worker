import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { getStuffs } from "../../slices/stuffSlice";
import { Container, Grid, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const Stuffs = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const stuffs = useAppSelector(state => state.stuff.stuffs);

  useEffect(() =>{
    dispatch(getStuffs());      
  // eslint-disable-next-line react-hooks/exhaustive-deps
  },[])

  const stuffsRender = stuffs.map((stuff, i) => {
    return (
      <Container 
      key={stuff.id} 
      sx={{
        padding: '5px',
        borderRadius: '15px',
        backgroundColor: '#bab6b9',
        cursor: 'pointer',
      }}
      onClick={() => navigate(`${stuff.id}`)}>
        <Typography 
          variant="h6"
          sx={{
            mr: 2,
            display: { xs: 'none', md: 'flex' },
            fontFamily: 'monospace',
            fontWeight: 700,
            letterSpacing: '.3rem',
            color: 'inherit',
            textDecoration: 'none',
          }}>
          {i+1 + '. ' + stuff.name}</Typography>
      </Container>
    )
  });
  return (
    <Grid direction='column' alignItems='center' container>
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
        Stuffs:
      </Typography>
      <Stack>
        {stuffsRender}
      </Stack>
    </Grid>
  )
};

export default Stuffs;