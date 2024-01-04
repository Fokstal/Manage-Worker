import { configureStore } from '@reduxjs/toolkit';
import authSlice from '../slices/authSlice';
import stuffSlice from '../slices/stuffSlice';

const store = configureStore({
  reducer : {
    auth : authSlice,
    stuff : stuffSlice
  }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;