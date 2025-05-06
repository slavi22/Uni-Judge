import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  type ProblemSolutionsState,
  SetUsedLanguagesPayload
} from "@/features/problems/types/problems-types.ts";

const initialState: ProblemSolutionsState = {
  usedLanguages: []
};

export const problemSolutionsSlice = createSlice({
  name: "problemSolutions",
  initialState: initialState,
  reducers: {
    setUsedLanguages: (
      state,
      action: PayloadAction<SetUsedLanguagesPayload>
    ) => {
      if (!state.usedLanguages) {
        state.usedLanguages = [];
      }
      // If the old language is not 0 (could be thought as null - not selected) then we swap the old language with the new one
      // essentially if we have c# and the old language is c# and the new language is javascript, we swap c# with javascript
      // our array will initially be [c#], then we will have [javascript]
      if (action.payload.oldLanguageId) {
        state.usedLanguages = state.usedLanguages.map((language) =>
          language === action.payload.oldLanguageId
            ? action.payload.newLanguageId
            : language
        );
      }
      // otherwise we add the new language to the list
      else {
        state.usedLanguages = [
          ...state.usedLanguages,
          action.payload.newLanguageId
        ];
      }
    },
    removeLanguage: (state, action: PayloadAction<number>) => {
      state.usedLanguages = state.usedLanguages.filter(id => id !== action.payload);
    },
    clearUsedLanguages: (state) => {
      state.usedLanguages = [];
    }
  }
});

export const { setUsedLanguages, removeLanguage, clearUsedLanguages } =
  problemSolutionsSlice.actions;

export default problemSolutionsSlice.reducer;
