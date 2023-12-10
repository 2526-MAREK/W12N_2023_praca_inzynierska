import * as types from "../actions/actionTypes";
import initialState from "./initialState";

export default function loadStructureAllObjectWithBasicInfoJsonReducer(state = initialState.structureAllObjectWithBasicInfoJson, action: any)   {

    if (
       action.type === types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON_WEB_SOCKET
     ) {
       return { structureAllObjectWithBasicInfoJson:  action.structureAllObjectWithBasicInfoJson };
     }
     
     return state;
     
     }