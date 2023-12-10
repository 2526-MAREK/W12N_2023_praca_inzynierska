import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function loadStructureAllObjectToFastModifyJson(state = initialState.structureAllObjectToFastModifyJson, action: any)   {

    if (
       action.type === types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET
     ) {
       return { sstructureAllObjectToFastModifyJson:  action.structureAllObjectToFastModifyJson };
     }
     
     return state;
     
     }