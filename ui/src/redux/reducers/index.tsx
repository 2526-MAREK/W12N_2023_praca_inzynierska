import { combineReducers } from "redux";
import apiCallStatusReducer from "./alarmsServiceApiStatusReducer";
import appState from "./appStateReducer";
import alarms from "./alarmsReducer";
import channelsDataToSystem from "./channelsDataReducer";

import isInitilizedModelsStructureJson from "./modelsStructureJsonReducer";
import isInitializeStructureAllObjectToFastModifyJson from "./modelsStructureJsonReducer";
import isInitializeStructureAllObjectWithBasicInfoJson from "./modelsStructureJsonReducer";

import isConnectedWebSocketModelsStructureJsonActions from "./webSocketReducer"
import messagesWebSocketModelsStructureJsonActions from "./webSocketReducer"
import structureAllObjectWithBasicInfoJson from "./structureAllObjectWithBasicInfoJsonReducer"
import structureAllObjectToFastModifyJson from "./structureAllObjectToFastModifyJsonReducer"

import apiMstrsServiceCallsInProgress from "./msrtsServiceApiReducer";
import mstrs from "./mstrsReducer";
import loggedUser from "./loggedUserReducer";

const rootReducer = combineReducers({
  apiAlarmServiceCallsInProgress: apiCallStatusReducer,
  appState,
  alarms,
  channelsDataToSystem,
  isInitilizedModelsStructureJson,
  isInitializeStructureAllObjectToFastModifyJson,
  isInitializeStructureAllObjectWithBasicInfoJson,
  isConnectedWebSocketModelsStructureJsonActions,
  messagesWebSocketModelsStructureJsonActions,
  structureAllObjectWithBasicInfoJson,
  structureAllObjectToFastModifyJson,
  apiMstrsServiceCallsInProgress,
  mstrs,
  loggedUser,
});

export default rootReducer;
