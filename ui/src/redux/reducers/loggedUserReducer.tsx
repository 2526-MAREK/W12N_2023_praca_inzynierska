import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function loggedUserReducer(state = initialState.loggedUser, action: any) {

    switch (action.type) {
        case types.SET_LOGGED_USER:
            return action.loggedUser;
        default:
            return state;
    }
}

