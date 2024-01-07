import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Box, Button, Grid, Modal, TextField, Typography } from "@mui/material";
import StuffService from "../../services/stuffService";
import stuff from "../../types/stuff";
import { useAppSelector } from "../../hooks/hooks";

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

const Stuff = () => {
  const {stuffId} = useParams();
  const [item, setItem] = useState<stuff>();
  const [isOpen, openCloseModal] = useState<boolean>(false);
  const isAuth = useAppSelector(state => state.auth.isAuth);
  const navigate = useNavigate();

  const handleOpen = () => openCloseModal(true);
  const handleClose = () => openCloseModal(false);

  const updateItem = () => {
    const service = new StuffService();
    service.getStuff(+(stuffId ?? '1'))
    .then((stuff) => {
      setItem(stuff);
    })
  };

  const editItem = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const service = new StuffService();
    service.ChangeStuff(+(stuffId ?? '1'), {
      name : data.get("name") as string
    })
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
        {isAuth ? <Button variant="contained" onClick={handleOpen}>Change</Button> : null}
        {isAuth ? 
        <Button 
          variant="contained" 
          color="error"
          onClick={() => {
            const service = new StuffService();
            if (stuffId) {
              service.deleteStuff(+stuffId)
              .then(() => navigate('/stuff'))
            }
          }}>
            Delete
        </Button> 
        : 
        null}
        <Modal
          open={isOpen}
          onClose={handleClose}
          aria-labelledby="modal-modal-title"
        >
          <Box sx={style} component={"form"} onSubmit={editItem} noValidate>
            <Typography id="modal-modal-title" variant="h6" component="h2" sx={{textAlign: "center", fontWeight: "bold"}}>
              EDIT STUFF
            </Typography>
            <TextField
              margin="normal"
              required
              fullWidth
              name="name"
              label="Stuff name"
              id="name"
              defaultValue={item?.name}
            />
          <Button variant="outlined" color="success" type="submit">Save changes</Button>
        </Box>
      </Modal>
      </Grid>
    </>
  )
};

export default Stuff;