import { Outlet } from "react-router-dom";
import {Card, Stack} from '@mui/material';
import Box from '@mui/material/Box';
import ParametersPage from "../parameters/ParametersPage";
import AlarmsPage from "../alarms/AlarmsPage";
import TrendsPage from "../trends/TrendsPage";
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';

import * as React from 'react';

const card = (
  <React.Fragment>
    <CardContent>
    <TrendsPage />
    </CardContent>
  </React.Fragment>
);

const cardParameters = (
  <React.Fragment>
    <CardContent>
    <ParametersPage />
    </CardContent>
  </React.Fragment>
);
const cardAlarms = (
  <React.Fragment>
    <CardContent>
    <AlarmsPage />
    </CardContent>
  </React.Fragment>
);

const DashboardPage = () => {
  return (

    <Stack> 
      
      <Box sx={{ minWidth: '100%', width: '100%', minHeight: '200px', marginTop: '20px' }}>
          <Card variant="outlined" sx={{ height: '100%', width: '100%' }}>
            {card}
          </Card>
        </Box>
      <Box sx={{ minWidth: '100%', width: '100%', minHeight: '200px', marginTop: '20px' }}>
        <Card variant="outlined" sx={{ height: '100%', width: '100%' }}>
          {cardAlarms}
        </Card>
      </Box>
      <Box sx={{ minWidth: '100%', width: '100%', minHeight: '200px', marginTop: '20px' }}>
  <Card variant="outlined" style={{ height: '100%', width: '100%' }}>
    {cardParameters}
  </Card>
</Box>
    </Stack>

  );
};

export default DashboardPage;