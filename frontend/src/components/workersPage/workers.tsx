import React, { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { Box, Button, Container, Grid, Modal, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { getWorkers } from "../../slices/workerSlice";
import WorkerService from "../../services/workerService";

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

const Workers = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [isOpen, openCloseModal] = useState<boolean>(false);
  const isAuth = useAppSelector(state => state.auth.isAuth);
  const workers = useAppSelector(state => state.worker.workers);

  const handleOpen = () => openCloseModal(true);
  const handleClose = () => openCloseModal(false);

  const createWorker = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const service = new WorkerService();
    const data = new FormData(event.currentTarget);
    service.createWorker(data)
    .then(() => {
      handleClose();
    })
  }

  useEffect(() =>{
    dispatch(getWorkers());   
  // eslint-disable-next-line react-hooks/exhaustive-deps
  },[isOpen])

  const workersRender = workers?.map((worker, i) => {
    return (
      <Container 
      key={worker.id} 
      sx={{
        padding: '5px',
        borderRadius: '15px',
        backgroundColor: '#bab6b9',
        cursor: 'pointer',
      }}
      onClick={() => navigate(`${worker.id}`)}>
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
          {i+1 + '. ' + worker.name}</Typography>
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
        Workers:
      </Typography>
      <Stack sx={{display: "flex", gap: "10px"}}>
        {workersRender}
      </Stack>
      <Button 
      sx={{alignSelf: 'start', marginLeft: '10px'}} 
      variant="contained"
      onClick={handleOpen}>
        Create new worker
      </Button>
      <Modal
        open={isOpen}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
      >
        <Box sx={style} component={"form"} onSubmit={createWorker} noValidate>
          <Typography id="modal-modal-title" variant="h6" component="h2" sx={{textAlign: "center", fontWeight: "bold"}}>
            CREATE NEW WORKER
          </Typography>
          <TextField
            margin="normal"
            required
            fullWidth
            name="name"
            label="Worker name"
            id="name"
            focused
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="stuffId"
            label="Stuff Id"
            type="number"
            id="stuffId"
            focused
          />
          <TextField
            margin="normal"
            fullWidth
            name="avatar"
            label="avatar"
            type="file"
            id="avatar"
            focused
          /> 
          <Button variant="outlined" color="success" type="submit">Create</Button>
        </Box>
      </Modal>
    </Grid>
  )
};

export default Workers;