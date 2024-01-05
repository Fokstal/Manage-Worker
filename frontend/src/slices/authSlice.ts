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
    return service.register(user)
      .then(() => {
        return service.login(user);
      })
      .then((token) => {
        localStorage.setItem('jwt-token', token);
        return true;
      })
      .catch((err) => {
        throw err;
      });
  }
);

const login = createAsyncThunk(
  'login/',
  async (user : user, thunkApi) => {
    const service = new AuthService();
    return service.login(user)
      .then((token) => {
        localStorage.setItem('jwt-token', token);
      })
      .catch((err) => {
        throw err;
      });
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
      localStorage.removeItem('jwt-token');
    },
  },
  extraReducers : (builder) => {
    builder.addCase(register.fulfilled, state => {state.isAuth = !!localStorage['jwt-token']});
    builder.addCase(login.fulfilled, state => {state.isAuth = !!localStorage['jwt-token']});
  },
});

export { AuthSlice, register, login };
export const {logout} = AuthSlice.actions
export default AuthSlice.reducer;