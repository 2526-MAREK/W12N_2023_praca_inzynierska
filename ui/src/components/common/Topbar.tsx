import { AppBar, Toolbar, Typography } from "@mui/material";
import colorConfigs from "../../configs/colorConfigs";
import sizeConfigs from "../../configs/sizeConfigs";

import { connect } from "react-redux";

type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


export interface AppState {
  appState: any;
}
type Props = PropsFromState & PropsFromDispatch;


const Topbar = ({appState}: Props) => {


  let topBarText = "";
  if(appState === "dashboard") {
    topBarText = "Dashboard";
  }


  if(appState ===  "systems.main") {
    topBarText = "Main System";
  }

  if(appState ===  "systems.airsystem") {
    topBarText = "Air System";
  }


  if(appState ===  "systems.coolingsystem") {
    topBarText = "Cooling System";
  }



  if(appState ===  "parameters") {
    topBarText = "Parameters";
  }


  if(appState ===  "alarms") {
    topBarText = "Alarms";
  }


  if(appState ===  "trends") {
    topBarText = "Trends";
  }

  if(appState ===  "diagnostics") {
    topBarText = "Diagnostics";
  }




  return (
    <AppBar
      position="fixed"
      sx={{
        width: `calc(100% - ${sizeConfigs.sidebar.width})`,
        ml: sizeConfigs.sidebar.width,
        boxShadow: "unset",
        backgroundColor: colorConfigs.topbar.bg,
        color: colorConfigs.topbar.color
      }}
    >
      <Toolbar>
        <Typography variant="h6">
          {topBarText}
        </Typography>
      </Toolbar>
    </AppBar>
  );
};


function mapStateToProps(state: AppState)  {
  return {
    appState: state.appState,
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
)(Topbar);


