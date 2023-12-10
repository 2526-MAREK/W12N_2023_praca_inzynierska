import * as types from "./actionTypes";

export function beginAlarmServiceApiCall() {
  return { type: types.BEGIN_ALARM_SERVICE_API_CALL };
}

export function apiAlarmServiceCallError() {
  return { type: types.ALARM_SERVICE_API_CALL_ERROR };
}
