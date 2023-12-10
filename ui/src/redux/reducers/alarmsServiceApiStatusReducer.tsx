import * as types from "../actions/actionTypes";
import initialState from "./initialState";

function actionTypeEndsInSuccess(type: any) {
  return type.substring(type.length - 8) === "_SUCCESS";
}

export default function apiCallStatusReducer(
  state = initialState.apiAlarmServiceCallsInProgress,
  action: any
) {
  if (action.type == types.BEGIN_ALARM_SERVICE_API_CALL) {
    return state + 1;
  } else if (
    action.type === types.ALARM_SERVICE_API_CALL_ERROR ||
    actionTypeEndsInSuccess(action.type)
  ) {
    return state - 1;
  }

  return state;
}




export type ApiAlarmServiceCallsInProgressState = typeof initialState.apiAlarmServiceCallsInProgress;