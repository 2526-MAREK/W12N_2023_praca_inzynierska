import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function appStateReducer(state = initialState.appState, action: any) {

    switch (action.type) {
        case types.SET_APP_STATE:
            return action.appState;
        default:
            return state;
    }
}

export type AppState = typeof initialState.appState;