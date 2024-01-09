import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import StuffService from "../services/stuffService";

interface stuffState {
  stuffs : Array<any>,
}

const initialState : stuffState = {
  stuffs : []
}

const getStuffs = createAsyncThunk(
  'getStuffs/',
  async (thunkApi) => {
    const service = new StuffService();
    return service.getStuffs()
    .then((stuffs) => {
      return stuffs;
    })
    .catch((err) => {
      throw new Error(err);
    })
  }
);

const StuffSlice = createSlice({
  name : 'stuff',
  initialState,
  reducers : {
  },
  extraReducers : (builder) => {
    builder.addCase(getStuffs.fulfilled, (state, action) => {
      state.stuffs = action.payload
    });
  },
});

export { StuffSlice, getStuffs };
export default StuffSlice.reducer;