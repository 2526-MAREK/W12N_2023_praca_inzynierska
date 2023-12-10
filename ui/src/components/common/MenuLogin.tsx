import React, {useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Button, Typography } from '@mui/material';
import Divider from '@mui/material/Divider';
import Paper from '@mui/material/Paper';
import MenuList from '@mui/material/MenuList';
import MenuItem from '@mui/material/MenuItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemIcon from '@mui/material/ListItemIcon';
import ContentCut from '@mui/icons-material/ContentCut';
import Cloud from '@mui/icons-material/Cloud';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import AccountCircle from '@mui/icons-material/AccountCircle';
import Input from '@mui/material/Input';
import {Stack} from '@mui/material';
import axios from 'axios';


import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';

import { connect } from "react-redux";
import * as loggedUserActions from "../../redux/actions/loggedUserActions";

const currencies = [
    {
      value: 'admin',
      label: 'Admin',
    },
    {
      value: 'engineer',
      label: 'Engineer',
    },
    {
      value: 'operator',
      label: 'Operator',
    },
  ];

  type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;

type OwnProps = {
  onRoleSelected: (role: string) => void; // Dodaj tę linijkę
};

type Props = PropsFromState & PropsFromDispatch& OwnProps;

export interface AppState {
  loggedUser: string;
}


const MenuLogin = ({setLoggedUser, loggedUser, onRoleSelected}: Props) => {
  const [openLogin, setOpenLogin] = React.useState(false);
  const [openRegister, setOpenRegister] = React.useState(false);

  const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState('');

    const [isLoggedIn, setIsLoggedIn] = useState(loggedUser);


    const  handleLogOut = (value: any) => {    
      setLoggedUser("guest");
      setIsLoggedIn("guest");
      onRoleSelected("guest");
    }


    const handleLoginChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      setLogin(event.target.value);
  }

  const handlePasswordChange = (value: any) => {    
    setPassword(value);
  }

  const handleRoleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRole(event.target.value);
  }

  const handleRegister = () => {
    /*console.log(login);
    console.log(password);
    console.log(role);*/

    const data = {
      Login: login,
      Password: password,
      Role: role
  }

  const url = 'http://localhost:5193/api/Login/Registration';
  axios.post(url, data).then((result) => {
      alert(result.data);
  }).catch((error) => {
      alert(error);
  });
}

const handleLogin = () => {
  const data = {
      Login: login,
      Password: password
      
  }

  const url = 'http://localhost:5193/api/Login/Login';
        axios.post(url, data).then((result) => {
            alert(result.data);
            if(result.data != "User is Invalid"){
              let splitStr = result.data.split("is");
              let resultStr = splitStr[3].trim();

              let splitStr2 = resultStr.split("_");
              const loggedUserTemp = splitStr2[0].trim();
              setLoggedUser(loggedUserTemp);
              setIsLoggedIn(loggedUserTemp);
              console.log(loggedUserTemp);
              const nameOfLoggedUserTemp = splitStr2[1].trim();
              onRoleSelected(nameOfLoggedUserTemp);
            }
        }).catch((error) => {
            if (error.response) {
                console.error('Validation errors:', error.response.data.errors);
                // Możesz również wyświetlić alert lub wyświetlić te błędy w interfejsie użytkownika
            } else if (error.request) {
                // The request was made but no response was received
                console.error('Request data', error.request);
            } else {
                // Something happened in setting up the request and triggered an Error
                console.error('Error message', error.message);
            }
            alert(error);
        });
    }



  const handleClickOpenLogin = () => {
    setOpenLogin(true);
  };

  const handleCloseLogin = () => {
    setOpenLogin(false);
  };

  const handleClickOpenRegister = () => {
    setOpenRegister(true);
  };

  const handleCloseRegister = () => {
    setOpenRegister(false);
  };

  const [showPassword, setShowPassword] = React.useState(false);

  const handleClickShowPassword = () => setShowPassword((show) => !show);

  const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  return (
    <Paper sx={{ width: 320, maxWidth: '100%' }}>
      <MenuList>
        <MenuItem onClick={handleClickOpenLogin}>
          <ListItemIcon>
            <ContentCut fontSize="small" />
          </ListItemIcon>
          <ListItemText>Login</ListItemText>
        </MenuItem>
        {loggedUser == 'admin' && (
  <>
    <Divider />
    <MenuItem onClick={handleClickOpenRegister}>
      <ListItemIcon>
        <Cloud fontSize="small" />
      </ListItemIcon>
      <ListItemText>Register</ListItemText>
    </MenuItem>
  </>
)}
      </MenuList>



      <Dialog open={openLogin} onClose={handleCloseLogin}>
  {isLoggedIn != "guest" ? (
    // Renderuj, gdy użytkownik jest zalogowany
    <>
    <DialogTitle>logged user is an {isLoggedIn}</DialogTitle>
    <DialogContent>
      <DialogContentText>
      </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleCloseLogin}>Cancel</Button>
        <Button onClick={handleLogOut}>Log Out</Button>
      </DialogActions>
    </>
    
  ) : (
    // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
    <>
      <DialogTitle>Login</DialogTitle>
      <DialogContent>
        <DialogContentText>
          <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
            <AccountCircle sx={{ color: 'action.active', mr: 1, my: 0.5 }} />
            <TextField
              id="input-with-sx"
              label="Login"
              variant="standard"
              onChange={handleLoginChange}
            />
          </Box>
          <FormControl sx={{ m: 1, width: '25ch' }} variant="standard">
            <InputLabel htmlFor="standard-adornment-password">Password</InputLabel>
            <Input
              id="standard-adornment-password"
              type={showPassword ? 'text' : 'password'}
              endAdornment={
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle password visibility"
                    onClick={handleClickShowPassword}
                    onMouseDown={handleMouseDownPassword}
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              }
              onChange={e => handlePasswordChange(e.target.value)}
            />
          </FormControl>
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleCloseLogin}>Cancel</Button>
        <Button onClick={handleLogin}>Login</Button>
      </DialogActions>
    </>
  )}
</Dialog>



      <Dialog open={openRegister} onClose={handleCloseRegister}>
        <DialogTitle>Register</DialogTitle>
        <DialogContent>
          <DialogContentText>
            <Stack spacing={4}>
          <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
        <AccountCircle sx={{ color: 'action.active', mr: 1, my: 0.5 }} />
        <TextField id="input-with-sx" 
        label="Login" 
        variant="standard" 
        onChange={handleLoginChange} // Dodaj obsługę zmiany
        />
      </Box>
      
      <FormControl sx={{ m: 1, width: '25ch' }} variant="standard">
          <InputLabel htmlFor="standard-adornment-password">Password</InputLabel>
          <Input
            id="standard-adornment-password"
            type={showPassword ? 'text' : 'password'}
            endAdornment={
              <InputAdornment position="end">
                <IconButton
                  aria-label="toggle password visibility"
                  onClick={handleClickShowPassword}
                  onMouseDown={handleMouseDownPassword}
                >
                  {showPassword ? <VisibilityOff /> : <Visibility />}
                </IconButton>
              </InputAdornment>
            }
            onChange={e => handlePasswordChange(e.target.value) }
          />
        </FormControl>
        <TextField
          id="standard-select-currency"
          select
          label="Select"
          defaultValue="Operator"
          helperText="Please select your role"
          variant="standard"
          onChange={handleRoleChange} // Dodaj obsługę zmiany
        >
          {currencies.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </TextField>
        </Stack>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseRegister}>Cancel</Button>
          <Button onClick={handleRegister}>Register</Button>
        </DialogActions>
      </Dialog>
    </Paper>
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
    setLoggedUser: (loggedUser: any) => dispatch(loggedUserActions.setLoggedUser(loggedUser)),
  };
}

export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
  mapStateToProps,
  mapDispatchToProps
)(MenuLogin);