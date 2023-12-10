import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function modelsStructureJsonReducer(state = initialState.isInitilizedModelsStructureJson, action: any) {

    switch (action.type) {
        case types.SET_IS_INITIALIZE_MODELS_JSON:
            return action.isInitilizedModelsStructureJson;
        case types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON:
            return action.isInitializeStructureAllObjectToFastModifyJson;
        case types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON:
            return action.isInitializeStructureAllObjectWithBasicInfoJson;
        default:
            return state;
    }
}

export type IsInitilizedModelsStructureJson = typeof initialState.isInitilizedModelsStructureJson;