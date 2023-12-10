import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function alarmsReducer(state = initialState.alarms, action: any) {
  switch (action.type) {
    case types.LOAD_ALARMS_SUCCESS:
      return action.alarms;
    default:
      return state;
  }
}

export type AlarmState = typeof initialState.alarms;