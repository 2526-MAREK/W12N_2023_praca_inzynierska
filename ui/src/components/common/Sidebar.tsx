import React, { useState, useRef } from 'react';
import { Avatar, Drawer, List, Stack, Toolbar,  Typography, Box } from "@mui/material";
import assets from "../../assets";
import colorConfigs from "../../configs/colorConfigs";
import sizeConfigs from "../../configs/sizeConfigs";
import appRoutes from "../../routes/appRoutes";
import SidebarItem from "./SidebarItem";
import SidebarItemCollapse from "./SidebarItemCollapse";
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { IconButton, Popover } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';

import MenuLogin from './MenuLogin';
import Divider from '@mui/material/Divider';

import { connect } from "react-redux";
/*type Props = {
  state: string,
};*/

type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;
type OwnProps = {
  state: string, // Dodaj tę linijkę
};

type Props = PropsFromState & PropsFromDispatch& OwnProps;

export interface AppState {
  loggedUser: string;
}

const Sidebar = ({ loggedUser, state}: Props) => {

  const [anchorEl, setAnchorEl] = useState(null);
  const ref = useRef();

  const [nameOfLoggedUser, setNameOfLoggedUser] = useState('guest');


  const handleStringFromMenuLogin = (stringValue: any) => {
    // Robisz coś ze stringiem, na przykład aktualizujesz stan
    console.log("Wybrana rola:", stringValue);

    
    setNameOfLoggedUser(stringValue);
  };

  const handleClick = (event: any) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };


  return (
    <Drawer
      variant="permanent"
      sx={{
        width: sizeConfigs.sidebar.width,
        flexShrink: 0,
        display: 'flex',
        flexDirection: 'column',
        height: '100vh',
        "& .MuiDrawer-paper": {
          width: sizeConfigs.sidebar.width,
          boxSizing: "border-box",
          borderRight: "0px",
          backgroundColor: colorConfigs.sidebar.bg,
          color: colorConfigs.sidebar.color,
          display: 'flex',
          flexDirection: 'column', // Ustaw kierunek na kolumnę
        }
      }}
    >
      <List disablePadding style={{ flexGrow: 1 }}>
        <Toolbar sx={{ marginBottom: "20px" }}>
          <Stack
            sx={{ width: "100%" }}
            direction="row"
            justifyContent="center"
          >
            <Avatar src={assets.images.logo} />
          </Stack>
        </Toolbar>
        {appRoutes.map((route, index) => (
          route.sidebarProps ? (
            route.child ? (
              <SidebarItemCollapse state={state} item={route} key={index} />
            ) : (
              <SidebarItem state={state} item={route} key={index} />
            )
          ) : null
        ))}
      </List>

      {/* Dodaj napis i Avatary na sam dół strony */}
      <Stack 
        direction="row" 
        justifyContent="space-between" 
        sx={{ padding: '10px' }}
      >
        <Stack direction="row" spacing={2}>
        <Avatar>H</Avatar>
        <Stack spacing={0} >
          <Typography variant="subtitle1" gutterBottom sx={{ marginBottom: '0px' }}>
            {nameOfLoggedUser}
          </Typography>
          <Typography variant="body2" gutterBottom color="primary" >
            {loggedUser}
          </Typography>
          </Stack>
        </Stack>


        <PopupState variant="popover" popupId="demo-popup-popover">
      {(popupState) => (
        <div>
          <IconButton {...bindTrigger(popupState)} color="primary" aria-label="more options">
            <MoreVertIcon />
          </IconButton>
          <Popover
            {...bindPopover(popupState)}
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'center',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'center',
            }}
          >
           <MenuLogin onRoleSelected={handleStringFromMenuLogin} />
            
          </Popover>
        </div>
      )}
    </PopupState>


      </Stack>
      {/* Dodaj puste miejsce pod napisem i avatarami */}
      <Box sx={{ height: '20px' }} />
    </Drawer>
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

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(Sidebar);

