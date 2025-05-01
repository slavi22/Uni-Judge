import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  type ProblemSolutionsState,
  type SolutionDialogDataObject,
} from "@/features/problems/types/problems-types.ts";

const initialState: ProblemSolutionsState = {
  allInputsValid: null,
  solutionDialogData: null,
};

export const problemSolutionsSlice = createSlice({
  name: "problemSolutions",
  initialState: initialState,
  reducers: {
    setValid: (state) => {
      state.allInputsValid = true;
    },
    setInvalid: (state) => {
      state.allInputsValid = false;
    },
    setSolutionDialogData: (
      state,
      action: PayloadAction<SolutionDialogDataObject>,
    ) => {
      const payload = action.payload;
      if (!state.solutionDialogData) {
        state.solutionDialogData = {}
      }
      state.solutionDialogData = {...state.solutionDialogData, ...payload}
    },
    clearSolutionDialogData: (state) => {
      state.solutionDialogData = null;
    },
  },
});

export const {
  setValid,
  setInvalid,
  setSolutionDialogData,
  clearSolutionDialogData,
} = problemSolutionsSlice.actions;

export default problemSolutionsSlice.reducer;
