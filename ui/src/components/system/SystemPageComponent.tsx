import React, { useState, useEffect, useMemo } from 'react';
import { useSelector } from 'react-redux';
import NormalDisplayValueDevice from '../../components/device/NormalDisplayValueDevice';
import AirSystemBackground from '../../components/systemBackground/airSystemBackground';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { connect } from "react-redux";

import { ChannelsDataToSystemState } from "../../redux/reducers/channelsDataReducer";
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Box from '@mui/material/Box';
import FormControlLabel from '@mui/material/FormControlLabel';
import Switch from '@mui/material/Switch';

import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';

const theme = createTheme({
  // ... your theme settings
});

type SensorPosition = {
  x: number;
  y: number;
};


type SensorData = {
  Name: string;
  Value: string;
  alarmStatus: "no_alarm" | "warning_alarm" | "urgent_alarm";
};

type AlarmStatus = "no_alarm" | "warning_alarm" | "urgent_alarm";
type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


type Props = PropsFromState & PropsFromDispatch;

export interface AppState {
  channelsDataToSystem: SensorData[];
  structureAllObjectToFastModifyJson: any;
  loggedUser: any;

}

type SystemPageProps = {
    sensorPositions: Record<string, SensorPosition>;
    systemPointKey: string;
    loggedUser: any;
    channelsDataToSystem: SensorData[];
    structureAllObjectToFastModifyJson: any; // Typ zależy od twojego stanu Redux
    SvgComponent: any ;
  };



const SystemPageComponent: React.FC<SystemPageProps>   = ({sensorPositions, 
  systemPointKey, 
  channelsDataToSystem, 
  structureAllObjectToFastModifyJson, 
  loggedUser, SvgComponent})=> {


    
  const [sensorData, setSensorData] = useState<SensorData[]>([]);

  //console.log("channelsDataToSystemState")
  //console.log(channelsDataToSystem);
  //console.log("structureAllObjectToFastModifyJson")
  //console.log(structureAllObjectToFastModifyJson)
  
  const getAlarmStatusFromJson = (topLevelKey: string, sensorKey: string, jsonString: any): string | null => {
    try {
      //console.log("Przekazany topLevelKey:", topLevelKey);
      // Usuwanie dodatkowych znaków ucieczki i parsowanie JSONa
      const correctedJsonString = jsonString.replace(/\\r\\n/g, "").replace(/\\"/g, '"');
      const nestedJson = JSON.parse(correctedJsonString);
      
      /*console.log(nestedJson)
      console.log("Typ nestedJson:", typeof nestedJson);
console.log("Czy nestedJson to obiekt:", nestedJson instanceof Object);
console.log("Klucze w nestedJson:", Object.keys(nestedJson));
console.log("Typ topLevelKey:", typeof topLevelKey);
console.log("Wartość topLevelKey:", topLevelKey);*/


try {
  const parsedJson = JSON.parse(nestedJson);

  // Teraz możesz spróbować ponownie uzyskać dostęp do klucza i iterować
  if (!parsedJson.hasOwnProperty(topLevelKey)) {
    console.error(`Klucz ${topLevelKey} nie istnieje w podanym JSONie.`);
    return null;
  }

  for (const key in parsedJson[topLevelKey]) {
    const device = parsedJson[topLevelKey][key];
    for (const subkey in device) {
      const sensor = device[subkey];
      if (sensor.hasOwnProperty(sensorKey)) {
        return sensor[sensorKey].ActualStatusAlarm;
      }
    }
  }
} catch (error) {
  console.error("Błąd podczas parsowania nestedJson:", error);
}

console.log("Nie znaleziono odpowiedniego sensora.");
return null;
    } catch (error) {
      console.error("Błąd podczas przetwarzania JSONa:", error);
      return null;
    }
    return null;
  };

  
  useEffect(() => {
    let updatedSensorData = channelsDataToSystem;
  
    if (structureAllObjectToFastModifyJson) {
      
      updatedSensorData = updatedSensorData.map(sensor => {
        // Zakładając, że getAlarmStatusFromJson jest zdefiniowana
        //const json = JSON.parse(structureAllObjectToFastModifyJson);
        /*console.log("structureAllObjectToFastModifyJson type of")
        console.log(typeof structureAllObjectToFastModifyJson)*/

        

        
        const nestedJsonString = structureAllObjectToFastModifyJson.sstructureAllObjectToFastModifyJson;


        const alarmStatusTemp = getAlarmStatusFromJson( systemPointKey, sensor.Name, nestedJsonString );
        /*console.log("alarmStatusTemp")
        console.log(alarmStatusTemp)*/
        let alarmStatus: AlarmStatus = "no_alarm"; // wartość domyślna
        if(alarmStatusTemp === 'unknown')
        {
          alarmStatus = "no_alarm";
        }
        else if(alarmStatusTemp === 'Resolved')
        {
          alarmStatus = "no_alarm";
        }
        else if(alarmStatusTemp === 'Warning')
        {
          alarmStatus = "warning_alarm";
        }
        else if(alarmStatusTemp === 'Urgency')
        {
          alarmStatus = "urgent_alarm";
        }
        else
        {
          alarmStatus = "no_alarm";
        }
        return { ...sensor, alarmStatus };
      });
    }
  
    setSensorData(updatedSensorData);
  
  }, [channelsDataToSystem, structureAllObjectToFastModifyJson]);


  const [selectedSensor, setSelectedSensor] = useState<string | null>(null);
  const [anchorEl, setAnchorEl] = useState<HTMLDivElement | null>(null);

  const handleClick = (sensorName: string) => (event: React.MouseEvent<HTMLElement>) => {
    // Use type assertion here to assure TypeScript that the target is indeed an HTMLDivElement
    setAnchorEl(event.currentTarget as HTMLDivElement);
    setSelectedSensor(sensorName);
  };

  const handleClose = () => {
    setAnchorEl(null);
    setSelectedSensor(null);
  };

  const open = Boolean(anchorEl);
  const id = open ? 'simple-popover' : undefined;


  // Stan dla wiadomości wpisanej przez użytkownika do wysłania
  const [userMessage, setUserMessage] = useState('');

  const [isAcknowledgedUrgentAlarm, setIsAcknowledgedUrgentAlarm] = React.useState(false);


  return (
    <ThemeProvider theme={theme}>
      <div>

        
      {(loggedUser == "engineer") || (loggedUser == "admin")   || (loggedUser == "operator") ? (
        <>
        {Array.isArray(sensorData) && sensorData.map((sensor) => {
          const position = sensorPositions[sensor.Name] || { x: 0, y: 0 }; // Default position if not found

          return (
            <React.Fragment key={sensor.Name}>
              <NormalDisplayValueDevice
                x={position.x}
                y={position.y}
                content={sensor.Value} // Value from the server
                unit="bar" // Assume unit is always "bar"
                alarmStatus={sensor.alarmStatus}
                onClick={handleClick(sensor.Name)}
                nameOfSensor={sensor.Name}
              />
              <Popover
  id={selectedSensor === sensor.Name ? id : undefined}
  open={selectedSensor === sensor.Name && open}
  anchorEl={anchorEl}
  onClose={handleClose}
  anchorOrigin={{
    vertical: 'bottom',
    horizontal: 'left',
  }}
>
  <Box
    sx={{
      position: 'relative', // Ustawienie kontenera na pozycję względną
      padding: 2, // Dodanie paddingu do całego kontenera
    }}
  >
    <IconButton
      aria-label="close"
      onClick={handleClose}
      sx={{
        position: 'absolute',
        right: 8,
        top: 8,
        color: (theme) => theme.palette.grey[500],
      }}
    >
      <CloseIcon />
    </IconButton>
    <Box
      sx={{
        paddingTop: 4, // Dodanie górnego paddingu, aby zrobić miejsce dla przycisku zamknięcia
        // Możesz dostosować ten padding, aby przycisk nie nachodził na zawartość poniżej
      }}
    >
      <FormControlLabel
        sx={{
          display: 'block',
        }}
        control={
          <Switch
            checked={isAcknowledgedUrgentAlarm}
            onChange={() => setIsAcknowledgedUrgentAlarm(!isAcknowledgedUrgentAlarm)}
            name="loading"
            color="primary"
          />
        }
        label="Acknowledged Urgent Alarm"
      />
     <Typography sx={{ p: 2 }}>The content of the Popover for {sensor.Name}.</Typography>
    </Box>
  </Box>
</Popover>
            </React.Fragment>
          );
        })}
  </>
        ) : (
          // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
          <>
         <Stack sx={{ width: '100%' }} spacing={2}>
            <Alert variant="filled" severity="error">
              Please login as an engineer or  operator to see channels!
            </Alert>
          </Stack>
          </>
        )}
  

        {/* <AirSystemBackground style={{ left: '900px', top: '157px' }} /> */}
        <SvgComponent />
      </div>
    </ThemeProvider>
  );
};


function mapStateToProps(state: AppState)  {
  return {
    channelsDataToSystem:
      state.channelsDataToSystem.length === 0
        ? []
        : state.channelsDataToSystem.map((alarm: any) => {
            return {
              ...alarm,
            };
          }),
          structureAllObjectToFastModifyJson: state.structureAllObjectToFastModifyJson,
          loggedUser: state.loggedUser,
  };
}

function mapDispatchToProps(dispatch: any) {
  return {
    dispatch,
  };
}
type OwnProps = {};

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(SystemPageComponent);

