import {beginAlarmServiceApiCall, apiAlarmServiceCallError } from "./alarmServiceApiStatusActions";
import * as alarmServiceApi from "../../api/AlarmServiceApi";
import * as types from "./actionTypes";



export function loadAlarmsSuccess(alarms: any) {
    return { type: types.LOAD_ALARMS_SUCCESS, alarms };
  }
  
  export function loadAlarms() {
    //console.log("halo2")
    return function(dispatch: any) {
      //console.log("halo3")
      dispatch(beginAlarmServiceApiCall());
      return alarmServiceApi.getAlarms().then(alarms => {
        //console.log("halo4")
        dispatch(loadAlarmsSuccess(alarms));
        return alarms; // Zwróć wynik
      }).catch(error => {
        //console.log("halo5")
        dispatch(apiAlarmServiceCallError());
        throw error; // Rzuć błąd dalej
      });
    };
  }