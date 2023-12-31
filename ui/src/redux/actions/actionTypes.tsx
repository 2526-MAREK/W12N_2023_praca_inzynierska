

export const DOWNOLAD_CHANNELS_DATA_FROM_API = "DOWNOLAD_CHANNELS_DATA_FROM_API"
export const STOP_DOWNOLAD_CHANNELS_DATA_FROM_API = "STOP_DOWNOLAD_CHANNELS_DATA_FROM_API"

export const LOAD_ALARMS_SUCCESS = "LOAD_ALARMS_SUCCESS";
export const BEGIN_ALARM_SERVICE_API_CALL = "BEGIN_ALARM_SERVICE_API_CALL";
export const ALARM_SERVICE_API_CALL_ERROR = "ALARM_SERVICE_API_CALL_ERROR";


export const LOAD_MSTRS_SUCCESS = "LOAD_MSTRS_SUCCESS";
export const BEGIN_MSTRS_SERVICE_API_CALL = "BEGIN_MSTRS_SERVICE_API_CALL";
export const MSTRS_SERVICE_API_CALL_ERROR = "MSTRS_SERVICE_API_CALL_ERROR";





export const SET_LOGGED_USER = "SET_LOGGED_USER";


export const SET_APP_STATE = "SET_APP_STATE";

export const SET_IS_INITIALIZE_MODELS_JSON = "SET_IS_INITIALIZE_MODELS_JSON";
export const IS_INITIALIZE_MODELS_JSON = "IS_INITIALIZE_MODELS_JSON";



export const SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON = "SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON";
export const SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON= "SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON";
export const IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON = "IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON";
export const IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON= "IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON";


// By convention, actions that end in "_SUCCESS" are assumed to have been the result of a completed
// API call. But since we're doing an optimistic delete, we're hiding loading state.
// So this action name deliberately omits the "_SUCCESS" suffix.
// If it had one, our apiCallsInProgress counter would be decremented below zero
// because we're not incrementing the number of apiCallInProgress when the delete request begins.
export const DELETE_EMP_DATA_OPTIMISTIC = "DELETE_EMP_DATA_OPTIMISTIC";


export const CONNECT_WEB_SOCKET = "CONNECT_WEB_SOCKET"
export const IS_CONNECT_WEB_SOCKET = "IS_CONNECT_WEB_SOCKET"


export const CONNECT_WEB_SOCKET_SUCCESS = "CONNECT_WEB_SOCKET_SUCCESS"
export const CONNECT_WEB_SOCKET_FAILED = "CONNECT_WEB_SOCKET_FAILED"
export const DISCONNECT_WEB_SOCKET= "DISCONNECT_WEB_SOCKET"
export const SEND_MESSAGE_WEB_SOCKET= "SEND_MESSAGE_WEB_SOCKET"
export const RECEIVE_MESSAGE_WEB_SOCKET= "SEND_MESSAGE_WEB_SOCKET"

export const RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON_WEB_SOCKET="RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON_WEB_SOCKET"
export const RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET="RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET"
