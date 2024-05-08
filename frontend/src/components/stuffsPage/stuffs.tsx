import React, { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { getStuffs } from "../../slices/stuffSlice";
import { Box, Button, Container, Grid, Modal, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import StuffService from "../../services/stuffService";

const style = {
  position: 'absolute' as 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  boxShadow: 24,
  p: 4,
  borderRadius: 5,
};

const Stuffs = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [isOpen, openCloseModal] = useState<boolean>(false);
  const stuffs = useAppSelector(state => state.stuff.stuffs);
  const isAuth = useAppSelector(state => state.auth.isAuth);

  const handleOpen = () => openCloseModal(true);
  const handleClose = () => openCloseModal(false);

  useEffect(() =>{
    dispatch(getStuffs());    
  // eslint-disable-next-line react-hooks/exhaustive-deps
  },[isOpen])

  const createNewStuff = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const service = new StuffService();
    const data = new FormData(event.currentTarget);
    service.createStuff({
      name : data.get('name') as string,
    }).then(() => {
      handleClose();
    })
  }

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
  if (!isAuth) return <>Please authorize</>
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
      <Stack sx={{display: "flex", gap: "10px"}}>
        {stuffsRender}
      </Stack>
      <Button 
      sx={{alignSelf: 'start', marginLeft: '10px'}} 
      variant="contained"
      onClick={handleOpen}>
        Create new stuff
      </Button>
      <Modal
        open={isOpen}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style} component={"form"} onSubmit={createNewStuff} noValidate>
          <Typography id="modal-modal-title" variant="h6" component="h2" sx={{textAlign: "center", fontWeight: "bold"}}>
            CREATE NEW STUFF
          </Typography>
          <TextField
              margin="normal"
              required
              fullWidth
              name="name"
              label="Stuff name"
              id="name"
            />
          <Button variant="outlined" color="success" type="submit">Create</Button>
        </Box>
      </Modal>
    </Grid>
  )
};

export default Stuffs;