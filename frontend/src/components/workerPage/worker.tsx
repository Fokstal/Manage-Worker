import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Box, Button, Grid, Modal, TextField, Typography } from "@mui/material";
import { useAppSelector } from "../../hooks/hooks";
import WorkerService from "../../services/workerService";
import worker from "../../types/worker";
import StuffService from "../../services/stuffService";
import stuff from "../../types/stuff";
import './worker.css';

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

const Worker = () => {
  const {workerId} = useParams();
  const [item, setItem] = useState<worker>();
  const [stuff, setsTuff] = useState<stuff>();
  const [isOpen, openCloseModal] = useState<boolean>(false);
  const isAuth = useAppSelector(state => state.auth.isAuth);
  const navigate = useNavigate();

  const handleOpen = () => openCloseModal(true);
  const handleClose = () => openCloseModal(false);

  const getStuffName = (id : number) => {
    const service = new StuffService();
    service.getStuff(id)
    .then((stuff) => {
      setsTuff(stuff);
    })
  }

  const updateItem = () => {
    const service = new WorkerService();
    service.getWorker(+(workerId ?? '1'))
    .then((worker) => {
      setItem(worker);
      getStuffName(worker.stuffId);
    })
  };

  const deleteItem = () => {
    const service = new WorkerService();
    service.deleteWorker(+(workerId ?? 0)).then(() => {
      navigate('/workers');
    })
  }

  const editWorker = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const service = new WorkerService();
    data.append("id", workerId ?? '0');
    service.editWorker(+(workerId ?? '0'), data)
    .then(() => {
      updateItem();
      handleClose();
    })
  };

  useEffect(() => {
    updateItem();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])
  return (
    <>
      <img className="worker-image" src={`http://localhost:5177/images/avatars/${item?.avatarUrl}`} alt="Worker icon"/>
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
        {`Рабочий - ${item?.name}`}
      </Typography>
      <Typography 
        variant="h3"
        component={"a"}
        href={`/stuff/${stuff?.id}`}
        sx={{
          mr: 2,
          display: { xs: 'none', md: 'flex' },
          fontFamily: 'monospace',
          fontWeight: 700,
          letterSpacing: '.3rem',
          color: 'inherit',
          textDecoration: 'none',
        }}>
        {`Название отряда - ${stuff?.name}`}  
      </Typography>
      <Grid sx={{gap: '10px', display: 'flex'}}>
        {isAuth ? <Button variant="contained" onClick={handleOpen}>Change</Button> : null}
        {isAuth ? <Button variant="contained" color="error" onClick={deleteItem}>Delete</Button> : null}
        <Modal
        open={isOpen}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
      >
        <Box sx={style} component={"form"} onSubmit={editWorker} noValidate>
          <Typography id="modal-modal-title" variant="h6" component="h2" sx={{textAlign: "center", fontWeight: "bold"}}>
            EDIT WORKER
          </Typography>
          <TextField
            margin="normal"
            required
            fullWidth
            name="name"
            label="Worker name"
            id="name"
            focused
            defaultValue={item?.name}
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
            defaultValue={stuff?.id}
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
          <Button variant="outlined" color="success" type="submit">Change</Button>
        </Box>
      </Modal>
      </Grid>
    </>
  )
};

export default Worker;