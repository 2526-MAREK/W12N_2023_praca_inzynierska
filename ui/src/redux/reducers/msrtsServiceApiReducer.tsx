import * as types from "../actions/actionTypes";
import initialState from "./initialState";

function actionTypeEndsInSuccess(type: any) {
  return type.substring(type.length - 8) === "_SUCCESS";
}

export default function apiMstrsServiceCallStatusReducer(
  state = initialState.apiMstrsServiceCallsInProgress,
  action: any
) {
  if (action.type == types.BEGIN_MSTRS_SERVICE_API_CALL) {
    return state + 1;
  } else if (
    action.type === types.MSTRS_SERVICE_API_CALL_ERROR ||
    actionTypeEndsInSuccess(action.type)
  ) {
    return state - 1;
  }

  return state;
}




export type ApiMstrsServiceCallsInProgressState = typeof initialState.apiMstrsServiceCallsInProgress;