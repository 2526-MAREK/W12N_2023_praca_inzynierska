import * as types from "../actions/actionTypes";
import { createStore, applyMiddleware } from 'redux';

export const webSocketMiddleware = ({ dispatch }: any) => {
  let socket: WebSocket | null = null;
  
  const onOpen = () => {
    dispatch({ type: types.CONNECT_WEB_SOCKET_SUCCESS });
    dispatch({ type: types.IS_CONNECT_WEB_SOCKET });
  };

  const onClose = () => {
    dispatch({ type: types.DISCONNECT_WEB_SOCKET });
  };

  const onMessage = (event: MessageEvent) => {
    //console.log('I wait for message from server:');
    const payload = event.data;
    //console.log('Message received from server:', payload);
    if(payload == null)
    {
      dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET, structureAllObjectToFastModifyJson: "" });
    }
    else
    {
      dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET, structureAllObjectToFastModifyJson: payload});
    }
  };

  const onMessageInitilizeStructureAllObjectWithBasicInfoJson = (event: MessageEvent) => {
    const payload = JSON.parse(event.data);
    //console.log('Message received from server:', payload);
    dispatch({ type: types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON, isInitializeStructureAllObjectWithBasicInfoJson: "true" });
    
    dispatch({ type: types.IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON });

    if(payload == null)
            {
              dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON_WEB_SOCKET, structureAllObjectWithBasicInfoJson: [] });
            }
            else
            {
              dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON_WEB_SOCKET, structureAllObjectWithBasicInfoJson : payload});
            }
  };

  const onMessageInitilizeStructureAllObjectToFastModifyJson = (event: MessageEvent) => {
    const payload = event.data;
    //console.log('Message received from server:', payload);
    dispatch({ type: types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON, isInitializeStructureAllObjectToFastModifyJson: "true" });
    dispatch({ type: types.IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON});

    if(payload == null)
            {
              dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET, structureAllObjectToFastModifyJson: "" });
            }
            else
            {
              dispatch({ type: types.RECEIVE_MESSAGE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON_WEB_SOCKET, structureAllObjectToFastModifyJson: payload});
            }
  };
  
  const onMessageNormalFlowStructureAllObjectToFastModifyJson = (event: MessageEvent) => {
    //console.log('Message received from server:', event.data);
    dispatch({ type: types.SET_IS_INITIALIZE_MODELS_JSON, isInitilizedModelsStructureJson: "true" });
    dispatch({ type: types.IS_INITIALIZE_MODELS_JSON });
  };

  return (next: any) => (action: any) => {
    //console.log('Action received in middleware:', action);
    switch (action.type) {
      case types.CONNECT_WEB_SOCKET:
        if (socket !== null) {
          socket.close();
        }
        console.log('Connecting to:', action.url);
        socket = new WebSocket(action.url);
        //socket.onclose = onClose;
        socket.onopen = onOpen;
        break;
      case types.SEND_MESSAGE_WEB_SOCKET:
        if (socket && socket.readyState === WebSocket.OPEN) {
          console.log('Sending message:', action.payload);
          socket.send(JSON.stringify(action.payload));
        }
        break;
      
      case types.DISCONNECT_WEB_SOCKET:
        if (socket !== null) {
          console.log('Disconnected :', action.url);
          if (socket && socket.readyState === WebSocket.OPEN) {
          socket.send("close");
          }
          socket.close();

        }
        socket = null;
        break;

      case types.IS_CONNECT_WEB_SOCKET:
        if (socket !== null) {
          if (socket && socket.readyState === WebSocket.OPEN) {
          socket.send("initializeStructureAllObjectWithBasicInfoJson");
          }
          socket.onmessage = onMessageInitilizeStructureAllObjectWithBasicInfoJson;
        }
        break;

      case types.IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON:
        if (socket !== null) {
          if (socket && socket.readyState === WebSocket.OPEN) {
          socket.send("initializeStructureAllObjectToFastModifyJson");
          }
          socket.onmessage = onMessageInitilizeStructureAllObjectToFastModifyJson;
        }
        break;
      
        case types.IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON:
          if (socket !== null) {
            if (socket && socket.readyState === WebSocket.OPEN) {
            socket.send("normalFlowStructureAllObjectToFastModifyJson");
            }
            socket.onmessage = onMessageNormalFlowStructureAllObjectToFastModifyJson;
          }
          break;
        
          case types.IS_INITIALIZE_MODELS_JSON:
            if (socket !== null) {
              if (socket && socket.readyState === WebSocket.OPEN) {
                //console.log('I wait for message from server 1 :');
                socket.onmessage = onMessage;
              }
              
            }
          break;

      default:
        return next(action);
    }
  };
};