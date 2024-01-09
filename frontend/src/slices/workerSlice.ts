import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import WorkerService from "../services/workerService";

interface stuffState {
  workers : Array<any>,
}

const initialState : stuffState = {
  workers : []
}

const getWorkers = createAsyncThunk(
  'getStuffs/',
  async (thunkApi) => {
    const service = new WorkerService();
    return service.getWorkers()
    .then((workers) => {
      return workers;
    })
    .catch((err) => {
      throw new Error(err)
    })
  }
);

const WorkerSlice = createSlice({
  name : 'worker',
  initialState,
  reducers : {
  },
  extraReducers : (builder) => {
    builder.addCase(getWorkers.fulfilled, (state, action) => {
      state.workers = action.payload
    });
  },
});

export { WorkerSlice, getWorkers };
export default WorkerSlice.reducer;