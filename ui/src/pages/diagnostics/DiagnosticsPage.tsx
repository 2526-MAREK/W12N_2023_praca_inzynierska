
import MenuLogin from "../../components/common/MenuLogin";
import { Outlet } from "react-router-dom";
import * as React from 'react';


import IconButton from '@mui/material/IconButton';
import Popover from '@mui/material/Popover';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { DataGrid, GridColTypeDef } from '@mui/x-data-grid';
import { randomStatusOptions, randomPrice } from '@mui/x-data-grid-generator';


import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';
import { connect } from "react-redux";

const rows = [
  {
    id: 1,
    Number: '1',
    Date: '24.01.2071',
    Time: '12:21 PM',
    MessegeFromAplication: 'Connection With Mqtt is bad Connection With Mqtt is bad Connection With Mqtt is bad Connection With Mqtt is bad Connection With Mqtt is bad Connection With Mqtt is bad Connection With Mqtt is bad Connection',
  },
  {
    id: 2,
    Number: '2',
    Date: '24.01.2071',
    Time: '12:21 PM',
    MessegeFromAplication: 'Connection With Mqtt is bad',
  }
];



type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


export interface AppState {
  loggedUser: any;
}
type Props = PropsFromState & PropsFromDispatch;


const DiagnosticsPage = ({loggedUser}: Props) => {
  return (
    <>
      {(loggedUser == "admin") ? (
        // Renderuj, gdy użytkownik jest zalogowany
        <>
     <div>

      <Box
        sx={{
          height: 300,
          width: '100%',
          '& .font-tabular-nums': {
            fontVariantNumeric: 'tabular-nums',
          },
        }}
      >
        <DataGrid
          columns={[
            { field: 'Number'},
            { field: 'Date'},
            { field: 'Time'},
            { field: 'MessegeFromAplication' , flex: 2  },
          ]}
          rows={rows}
        />
      </Box>
    </div>
    </>
        
      ) : (
        // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
        <>
       <Stack sx={{ width: '100%' }} spacing={2}>
          <Alert variant="filled" severity="error">
            Please login as an admin to see  diagnostic page!
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
)(DiagnosticsPage);
