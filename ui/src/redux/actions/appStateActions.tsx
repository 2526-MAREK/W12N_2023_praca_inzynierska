import * as types from "./actionTypes";


export function setAppState(state: string)
 {return { type: types.SET_APP_STATE, appState: state };}


