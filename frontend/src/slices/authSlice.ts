import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import AuthService from "../services/AuthService";
import user from "../types/user";

interface authState {
  isAuth : boolean,
}

const initialState : authState = {
  isAuth : !!localStorage['jwt-token']
}

const register = createAsyncThunk(
  'register/',
  async (user : user, thunkApi) => {
    const service = new AuthService();
    service.register(user)
    .then((token) => {
      localStorage.setItem('jwt-token', token);
      return true;
    })
    .catch((err) => {
      throw new Error(err);
    })
  }
);

const AuthSlice = createSlice({
  name : 'auth',
  initialState,
  reducers : {
    login  : state => {
      state.isAuth = true;
    },
    logout : state => {
      state.isAuth = false;
    },
  },
  extraReducers : (builder) => {
    builder.addCase(register.fulfilled, state => {state.isAuth = true});
  },
});

export { AuthSlice, register };
export const {login, logout} = AuthSlice.actions
export default AuthSlice.reducer;