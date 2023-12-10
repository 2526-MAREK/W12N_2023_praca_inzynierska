import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function channelsDataToSystemReducer(state = initialState.channelsDataToSystem, action: any) {

    switch (action.type) {
        case types.DOWNOLAD_CHANNELS_DATA_FROM_API:
            return action.channelsDataToSystem;
        default:
            return state;
    }
}

export type ChannelsDataToSystemState = typeof initialState.channelsDataToSystem;