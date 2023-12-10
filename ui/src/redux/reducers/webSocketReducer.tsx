import * as types from "../actions/actionTypes";
import initialState from "./initialState";


/*export default function webSocketReducer(stateIsConnectedWebSocketModelsStructureJsonActions = initialState.isConnectedWebSocketModelsStructureJsonActions,
    stateMessagesWebSocketModelsStructureJsonActions = initialState.messagesWebSocketModelsStructureJsonActions, action: any) {

        switch (action.type) {
            case 'CONNECT_SUCCESS':
              return { ...stateIsConnectedWebSocketModelsStructureJsonActions, isConnectedWebSocketModelsStructureJsonActions: true };
            case 'DISCONNECT':
              return { ...stateIsConnectedWebSocketModelsStructureJsonActions, isConnectedWebSocketModelsStructureJsonActions: false };
            case 'RECEIVE_MESSAGE':
              return { ...stateMessagesWebSocketModelsStructureJsonActions, messagesWebSocketModelsStructureJsonActions: [...stateMessagesWebSocketModelsStructureJsonActions.messagesWebSocketModelsStructureJsonActions, action.payload] };
            default:
              return state;
          }
}*/


export default function webSocketReducer(state = initialState, action: any)   {

if (action.type == types.CONNECT_WEB_SOCKET_SUCCESS) {
  return { ...state, isConnectedWebSocketModelsStructureJsonActions: true };
} else if (
  action.type === types.DISCONNECT_WEB_SOCKET
) {
  return { ...state, isConnectedWebSocketModelsStructureJsonActions: false };
}
else if (
  action.type === types.RECEIVE_MESSAGE_WEB_SOCKET
) {
  return { ...state, structureAllObjectWithBasicInfoJson: [...state.messagesWebSocketModelsStructureJsonActions, action.payload] };
}

return state;

}


