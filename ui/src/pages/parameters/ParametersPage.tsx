import * as React from 'react';
import Box from '@mui/material/Box';
import {Card, Stack} from '@mui/material';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

import { connect } from "react-redux";

import Alert from '@mui/material/Alert';
import { any } from 'prop-types';


type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;

type Props = PropsFromState & PropsFromDispatch;
export interface AppState {
  loggedUser: any;
  structureAllObjectWithBasicInfoJson: any;
}

const bull = (
  <Box
    component="span"
    sx={{ display: 'inline-block', mx: '2px', transform: 'scale(0.8)' }}
  >
    •
  </Box>
);

const card = (
  <React.Fragment>
    <CardContent>
      <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
        Word of the Day
      </Typography>
      <Typography variant="h5" component="div">
        be{bull}nev{bull}o{bull}lent
      </Typography>
      <Typography sx={{ mb: 1.5 }} color="text.secondary">
        adjective
      </Typography>
      <Typography variant="body2">
        well meaning and kindly.
        <br />
        {'"a benevolent smile"'}
      </Typography>
    </CardContent>
    <CardActions>
      <Button size="small">Learn More</Button>
    </CardActions>
  </React.Fragment>
);

type PropsToChannel = {
  channel: any;
}

const ChannelCard = ({ channel }: PropsToChannel) => (
  <Card variant="outlined" sx={{ mb: 1 }}>
    <CardContent>
      <Typography variant="body2">Channel: {channel.ChannelIdentifier}</Typography>
      <Typography variant="body2">Type: {channel.Type}</Typography>
      <Typography variant="body2">Unit: {channel.Unit}</Typography>
      <Typography variant="body2">Factor: {channel.Factor}</Typography>
    </CardContent>
  </Card>
);


type PropsToDevice = {
  device: any;
  channels: any;
}

const DeviceCard = ({ device, channels }: PropsToDevice) => {
  //console.log("Device data in DeviceCard:", device);
  return( <Card variant="outlined" sx={{ mb: 1 }}>
  <CardContent>
    <Typography variant="body1">Device: {device.Name}</Typography>
    <Typography variant="body2">Identifier: {device.DeviceIdentifier}</Typography>
    <Typography variant="body2">Description: {device.Description}</Typography>
    <Typography variant="body2">Location: {device.Location}</Typography>
    <Typography variant="body2">Type: {device.Type}</Typography>
    {/* Renderuj kanały, jeśli istnieją */}
    {Array.isArray(channels) && channels.map((channel: any) => (
      ChannelCard ({ channel })
      // <Typography key={channel.IdChannel} variant="body2">
      //   Channel: {channel.ChannelIdentifier} - {channel.Factor}
      // </Typography>
    ))}
  </CardContent>
</Card>);
};

type PropsToHub = {
  hub: any;
  devices: any;
}


const HubCard = ({ hub, devices }: PropsToHub) => {
  //console.log("Hub data:", hub);
  //console.log("devices data:", devices);
  return (
    <Card variant="outlined" sx={{ mb: 1 }}>
      <CardContent>
        <Typography variant="h6">Hub: {hub.Name}</Typography>
        <Typography variant="body2">Identifier: {hub.HubIdentifier}</Typography>
        <Typography variant="body2">Description: {hub.Description}</Typography>
        <Typography variant="body2">Location: {hub.Location}</Typography>
      </CardContent>
      <CardContent>
        <Stack>
          {Array.isArray(devices) && devices.map((deviceData: any) => (
            <DeviceCard 
              key={deviceData.Device.IdDevice} 
              device={deviceData.Device} 
              channels={deviceData.Channels} // Przekazujemy kanały
            />
          ))}
        </Stack>
      </CardContent>
    </Card>
  );
};

type PropsToCard = {
  msrtPoint: any;
  hubs: any;
}

const MsrtPointCard = ({ msrtPoint, hubs }: PropsToCard) => (
  <Card variant="outlined" sx={{ m: 1 }}>
    <CardContent>
      <Typography variant="h5">{msrtPoint.Name}</Typography>
      <Typography variant="body1">Identifier: {msrtPoint.MsrtPointIdentifier}</Typography>
      <Typography variant="body1">Description: {msrtPoint.Description}</Typography>
      <Typography variant="body1">Factor: {msrtPoint.Factor}</Typography>
      <Typography variant="body1">Location: {msrtPoint.Location}</Typography>
      <Stack>
        {Array.isArray(hubs) && hubs.map((hubData: any) => (
          <HubCard key={hubData.Hub.IdHub} hub={hubData.Hub} devices={hubData.Devices} />
        ))}
      </Stack>
    </CardContent>
  </Card>
);


const ParametersPage = ({data, loggedUser}: Props) => {
  //console.dir(data, { depth: 20 });
  //console.log(JSON.stringify(data, null, 2));
  //const parsedData = JSON.parse(data);
  //console.log(typeof data)

  let parsedData;
  try {
    if (data && data.structureAllObjectWithBasicInfoJson) {
      parsedData = JSON.parse(data.structureAllObjectWithBasicInfoJson);
    }
  } catch (e) {
    console.error("Błąd podczas parsowania danych JSON:", e);
    return <div>Błąd w danych</div>;
  }

  //console.log(parsedData);

  return (
    <>
      {(loggedUser == "engineer") || (loggedUser == "admin")   || (loggedUser == "operator") ? (
        // Renderuj, gdy użytkownik jest zalogowany
        <>
         <Stack  direction="row"
  justifyContent="center"
  alignItems="baseline"
  spacing={2} >
          {Array.isArray(parsedData) && parsedData.map((item: any) => (
            <MsrtPointCard key={item.MsrtPoint.IdPoint} msrtPoint={item.MsrtPoint} hubs={item.Hubs} />
          ))}
        </Stack>
    </>
        
      ) : (
        // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
        <>
       <Stack sx={{ width: '100%' }} spacing={2} >
          <Alert variant="filled" severity="error">
            Please login as an engineer or  operator to see parameters page!
          </Alert>
        </Stack>
        </>
      )}


</>
    
  );
};


function mapStateToProps(state: AppState)  {
  return {
    loggedUser: state.loggedUser,
    data: state.structureAllObjectWithBasicInfoJson,
  };
}

function mapDispatchToProps(dispatch: any) {
  return {
    dispatch,
  };
}
type OwnProps = {
};


export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(ParametersPage);

