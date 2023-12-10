import { writevSync } from 'fs';
import { useDispatch } from 'react-redux';
import { connect } from "react-redux";
import { useState, useEffect } from "react";
import * as channelsDataActions from "../redux/actions/channelsDataActions";

type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


type Props = PropsFromState & PropsFromDispatch;

export interface AppState {
  appState: AppState;
}

export const closeWebSocket = () => {
  
};

const WebSocketManager = ({ appState, setChannelsDataState}: Props)=> {

  const sendCloseMessage = (ws: WebSocket) => {
    if (ws && ws.readyState === WebSocket.OPEN) {
      ws.send("close");
    }
  };

  console.log("appState")
  console.log(appState)
  const channelsDataToSystemWebSocket = new WebSocket('ws://localhost:5193/getData');
  
  let actualSendedMessageToWebSocket = ""
  //if(channelsDataToSystemWebSocket.readyState == channelsDataToSystemWebSocket.OPEN){
      channelsDataToSystemWebSocket.onopen = () => {
        console.log('Connected to WebSocket');
        if(appState.toString() == 'systems.airsystem')
      {
        console.log("send to websocket: airSystemPoint_1")
        actualSendedMessageToWebSocket = "airSystemPoint_1"
        channelsDataToSystemWebSocket.send("airSystemPoint_1");
      }
      else if(appState.toString() == 'systems.coolingsystem')
      {
        console.log("send to websocket: collingSystemPoint_1")
        actualSendedMessageToWebSocket = "collingSystemPoint_1"
        channelsDataToSystemWebSocket.send("collingSystemPoint_1");
      }
      else
      {
        console.log("send to websocket: nothing")
        actualSendedMessageToWebSocket = "nothing"
        channelsDataToSystemWebSocket.send("nothing");
      }
      };

      

      channelsDataToSystemWebSocket.onmessage = (event) => {
        try {
          
          /*console.log("event.data in begin on message ")
            console.log(event.data)*/

          if(actualSendedMessageToWebSocket == "nothing")
          {
            /*console.log("event.data in if(actualSendedMessageToWebSocket == nothing")
            console.log(event.data)*/
            if(event.data.toString() != "nothing")
            {
              /*console.log("event.data in event.data != nothing")
            console.log(event.data)*/
              actualSendedMessageToWebSocket = "nothing"
              channelsDataToSystemWebSocket.send("nothing");

            }
          }
          else
          {
            /*console.log("event.data in esle of if(actualSendedMessageToWebSocket == nothing")
            console.log(event.data)*/
            // First parse to get the string
            const jsonString = JSON.parse(event.data);
            //console.log('JSON String:', jsonString);
        
            // Second parse to get the actual array
            const messageFromServer = JSON.parse(jsonString);
            /*console.log('Type of messageFromServer after second parse:', typeof messageFromServer);
            console.log('Is messageFromServer an array after second parse?:', Array.isArray(messageFromServer));
            console.log('Message from server:', messageFromServer);*/
            if(messageFromServer == null)
            {
              setChannelsDataState([])
            }
            else
            {
              console.log(messageFromServer)
              setChannelsDataState(messageFromServer)
            }
          }

      
          
        } catch (error) {
          console.error("Error parsing message from server:", error);
        }
      };
    /*}
    else
    {
      console.log("Connect with api websocket is down")
    }*/
    


  // Return JSX or null if it's just for managing WebSocket
  return null;
};

function mapStateToProps(state: AppState)  {
  return {
    appState: state.appState
  };
}

function mapDispatchToProps(dispatch: any) {
  return {
    dispatch,
    setChannelsDataState: (channelsData: []) => dispatch(channelsDataActions.setChannelsDataState(channelsData)),
  };
}
type OwnProps = {};

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(WebSocketManager);
