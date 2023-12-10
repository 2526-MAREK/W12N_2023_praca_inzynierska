import {beginMstrsServiceApiCall, apiMstrsServiceCallError } from "./mstrsServiceApiActions";
import * as mstrtsServiceApi from "../../api/MsrtsServiceApi";
import * as types from "./actionTypes";



export function loadMstrsSuccess(mstrs: any) {
    return { type: types.LOAD_MSTRS_SUCCESS, mstrs };
  }
  
  export function loadMstrs(channelIdentifier: string, startDate: any, endDate: any) {
    //console.log("halo2")
    return function(dispatch: any) {
      //console.log("halo3")
      dispatch(beginMstrsServiceApiCall());
      return mstrtsServiceApi.getMstrs(channelIdentifier, startDate, endDate).then(mstrs => {
        //console.log("halo4")
        dispatch(loadMstrsSuccess(mstrs ));
        return mstrs; // Zwróć wynik
      }).catch(error => {
        //console.log("halo5")
        dispatch(apiMstrsServiceCallError());
        throw error; // Rzuć błąd dalej
      });
    };
  }