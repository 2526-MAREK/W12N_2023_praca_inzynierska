import { writevSync } from 'fs';
import { useDispatch } from 'react-redux';
import { connect } from "react-redux";
import { useState, useEffect } from "react";
import * as channelsDataActions from "../redux/actions/channelsDataActions";

import * as modelsStructureJsonActions from "../redux/actions/modelsStructureJsonActions";
import * as webSocketMiddleware from "../redux/middleware/webSocketMiddleware"
import * as types from "../redux/actions/actionTypes";

type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


type Props = PropsFromState & PropsFromDispatch;

export interface AppState {
  appState: AppState;
  isInitilizedModelsStructureJson: string;
  isInitializeStructureAllObjectToFastModifyJson: string;
  isInitializeStructureAllObjectWithBasicInfoJson: string;
}

export const closeWebSocket = () => {
  
};

const initilizeModelsStructureJson = (modelsStructureJsonWebSocket: WebSocket, actualSendedMessageToWebSocket: string, messageFromServer: [], isInitializeStructureAllObjectToFastModifyJson: string, setIsInitializeStructureAllObjectToFastModifyJson: any) =>
{

  if(actualSendedMessageToWebSocket == "initializeStructureAllObjectToFastModifyJson")
              {
                actualSendedMessageToWebSocket = "normalFlowStructureAllObjectToFastModifyJson"
                if (modelsStructureJsonWebSocket.readyState === WebSocket.OPEN) {
                modelsStructureJsonWebSocket.send("normalFlowStructureAllObjectToFastModifyJson")
                }
              }


            if((actualSendedMessageToWebSocket == "initializeStructureAllObjectWithBasicInfoJson") && (isInitializeStructureAllObjectToFastModifyJson = "false"))
              {
                actualSendedMessageToWebSocket = "initializeStructureAllObjectToFastModifyJson"
                if (modelsStructureJsonWebSocket.readyState === WebSocket.OPEN) {
                modelsStructureJsonWebSocket.send("initializeStructureAllObjectToFastModifyJson")
                }
                setIsInitializeStructureAllObjectToFastModifyJson("true")
              }
            
            if(messageFromServer == null)
            {
              if(actualSendedMessageToWebSocket == "initializeStructureAllObjectWithBasicInfoJson")
              {
                //setStructureAllObjectWithBasicInfoJson([])
              }


              if(actualSendedMessageToWebSocket == "initializeStructureAllObjectToFastModifyJson")
              {
                //setStructureAllObjectToFastModifyJson([])
              }
              
            }
            else
            {
              if(actualSendedMessageToWebSocket == "initializeStructureAllObjectWithBasicInfoJson")
              {
                //setStructureAllObjectWithBasicInfoJson(messageFromServer)
              }

              if(actualSendedMessageToWebSocket == "initializeStructureAllObjectToFastModifyJson")
              {
                //setStructureAllObjectToFastModifyJson(messageFromServer)
              }
              
            }

            return actualSendedMessageToWebSocket;

}

const WebSocketInitializeModelsStructureJsonApi = ({ setChannelsDataState, setIsInitializeModelsJson,
  setIsInitializeStructureAllObjectWithBasicInfoJson,
  setIsInitializeStructureAllObjectToFastModifyJson, 
   isInitilizedModelsStructureJson, isInitializeStructureAllObjectWithBasicInfoJson, isInitializeStructureAllObjectToFastModifyJson,dispatch}: Props)=> {

    useEffect(() => {
      dispatch({ type: types.CONNECT_WEB_SOCKET, url: 'ws://localhost:5193/getModelsStructureJson' });
      //dispatch({ type: types.SEND_MESSAGE_WEB_SOCKET, payload: 'initializeStructureAllObjectWithBasicInfoJson' })
      // Tutaj możesz też obsłużyć rozłączenie, jeśli potrzebne
      return () => {
        dispatch({ type: types.DISCONNECT_WEB_SOCKET });
      };
    }, []);

    
  
    //dispatch

  // Return JSX or null if it's just for managing WebSocket
  return null;
};

function mapStateToProps(state: AppState)  {
  return {
    isInitilizedModelsStructureJson: state.isInitilizedModelsStructureJson,
    isInitializeStructureAllObjectToFastModifyJson: state.isInitializeStructureAllObjectToFastModifyJson,
    isInitializeStructureAllObjectWithBasicInfoJson: state.isInitializeStructureAllObjectWithBasicInfoJson,
  };
}

function mapDispatchToProps(dispatch: any) {
  return {
    dispatch,
    setChannelsDataState: (channelsData: []) => dispatch(channelsDataActions.setChannelsDataState(channelsData)),
    setIsInitializeModelsJson: (isInitializeModelsJson: string) => dispatch(modelsStructureJsonActions.setIsInitializeModelsJson(isInitializeModelsJson)),
    setIsInitializeStructureAllObjectWithBasicInfoJson: (isInitializeStructureAllObjectWithBasicInfoJson: string) => dispatch(modelsStructureJsonActions.setIsInitializeStructureAllObjectWithBasicInfoJson(isInitializeStructureAllObjectWithBasicInfoJson)),
    setIsInitializeStructureAllObjectToFastModifyJson: (isInitializeStructureAllObjectToFastModifyJson: string) => dispatch(modelsStructureJsonActions.setIsInitializeStructureAllObjectToFastModifyJson(isInitializeStructureAllObjectToFastModifyJson)),

  };
}
type OwnProps = {};

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(WebSocketInitializeModelsStructureJsonApi);
