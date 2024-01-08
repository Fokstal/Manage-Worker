import { configureStore } from '@reduxjs/toolkit';
import authSlice from '../slices/authSlice';
import stuffSlice from '../slices/stuffSlice';
import workerSlice from '../slices/workerSlice';

const store = configureStore({
  reducer : {
    auth : authSlice,
    stuff : stuffSlice,
    worker : workerSlice
  }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;