import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function mstrsReducer(state = initialState.mstrs, action: any) {
  switch (action.type) {
    case types.LOAD_MSTRS_SUCCESS:
      return action.mstrs;
    default:
      return state;
  }
}

export type MstrsState = typeof initialState.mstrs;