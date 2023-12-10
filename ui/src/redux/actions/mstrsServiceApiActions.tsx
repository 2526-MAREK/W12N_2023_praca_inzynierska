import * as types from "./actionTypes";

export function beginMstrsServiceApiCall() {
  return { type: types.BEGIN_MSTRS_SERVICE_API_CALL };
}

export function apiMstrsServiceCallError() {
  return { type: types.MSTRS_SERVICE_API_CALL_ERROR};
}
