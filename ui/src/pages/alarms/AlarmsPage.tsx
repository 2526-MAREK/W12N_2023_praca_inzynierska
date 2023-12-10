import * as React from 'react';
import { useState, useEffect } from "react";
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import { DataGrid, GridColTypeDef } from '@mui/x-data-grid';
import { randomStatusOptions, randomPrice } from '@mui/x-data-grid-generator';
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as alarmsActions from "../../redux/actions/alarmsActions";
import moment from 'moment';
import Spinner from '../../components/common/Spinner';
import CircularProgress from '@mui/material/CircularProgress';
//import { loadAlarms } from '../../redux/actions/alarmsActions';

import { AlarmState } from "../../redux/reducers/alarmsReducer";
import { ApiAlarmServiceCallsInProgressState } from "../../redux/reducers/alarmsServiceApiStatusReducer";

import { useDispatch } from 'react-redux';

import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';
interface Alarm {
  IdAlarm: number;
  ChannelIdentifier: string;
  MessageText: string;
  OccurrenceDate: string; // ISO string
  PossibleFault: string;
  ResolutionDate: string; // ISO string, opcjonalne
  Status: string;
}


const transformApiDataToRows = (apiData: Alarm[]) => {
  return apiData.map((alarm, index) => ({
    id: alarm.IdAlarm, // Używamy IdAlarm z danych API
    Number: (index + 1).toString(),
    ChannelIdentifier: alarm.ChannelIdentifier, // Bez zmian
    OccurrenceDate: alarm.OccurrenceDate, // Bez zmian
    Status: alarm.Status, // Bez zmian
    PossibleFault: alarm.PossibleFault, // Dodane z danych API
    MessageText: alarm.MessageText, // Poprawione na MessageText
    ResolutionDate: alarm.ResolutionDate, // Dodane z danych API
    // Dodaj inne pola jeśli są potrzebne
  }));
};


type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


type Props = PropsFromState & PropsFromDispatch;

export interface AppState {
  alarms: AlarmState;
  apiAlarmServiceCallsInProgress: ApiAlarmServiceCallsInProgressState;
  loading: boolean;
  loggedUser: any;
}


const AlarmsPage = ({ alarms, loading,loggedUser, loadAlarms}: Props)=> {

  const [rows, setRows] = useState<ReturnType<typeof transformApiDataToRows>>([]);

  const handleApiData = (apiData: Alarm[]) => {
    console.log(apiData);
    const newRows = transformApiDataToRows(apiData);
    setRows(newRows);
  };
  

  useEffect(() => {
    const fetchAlarms = async () => {
      try {
        await loadAlarms();
      } catch (error) {
        alert("Loading alarms failed: " + error);
      }
    };

    if (alarms.length === 0) {
      fetchAlarms();
    }
    
    handleApiData(alarms);

  }, [alarms.length, loadAlarms]);



  return (
    <>
      {(loggedUser == "engineer") || (loggedUser == "admin")  ? (
        // Renderuj, gdy użytkownik jest zalogowany
        <>
      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 300 }}>
          <CircularProgress />
        </Box>
      ) : (
        <Box sx={{ height: 600, width: '100%', '& .font-tabular-nums': { fontVariantNumeric: 'tabular-nums' } }}>
          <DataGrid
          //  autoHeight
          columns={[
            { field: 'Number', flex: 0.5 },
            { field: 'ChannelIdentifier', flex: 1 },
            { field: 'OccurrenceDate', flex: 1.5 },
            { field: 'Status', flex: 0.5 },
            { field: 'PossibleFault', flex: 2.5 },
            { field: 'MessageText', flex: 1 },
            { field: 'ResolutionDate', flex: 1.5 },
          ]}
          rows={rows}
        />
        </Box>
      )}
    </>
        
      ) : (
        // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
        <>
       <Stack sx={{ width: '100%' }} spacing={2}>
          <Alert variant="filled" severity="error">
            Please login as an engineer to see alarms page!
          </Alert>
        </Stack>
        </>
      )}


</>
  );
};

function mapStateToProps(state: AppState)  {
  return {
    alarms:
      state.alarms.length === 0
        ? []
        : state.alarms.map((alarm: any) => {
            return {
              ...alarm,
            };
          }),
    loading: state.apiAlarmServiceCallsInProgress > 0,
    loggedUser: state.loggedUser,
  };
}

function mapDispatchToProps(dispatch: any) {
  return {
    dispatch,
    loadAlarms: () => dispatch(alarmsActions.loadAlarms()),
  };
}
type OwnProps = {};

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(AlarmsPage);
