import { Outlet } from "react-router-dom";
import React, { useEffect, useState } from 'react';
//npm install chart.js@^4.1.1 react-chartjs-2
//npm install --save-dev @faker-js/faker
import 'chart.js';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
  } from 'chart.js';
  import { Line } from 'react-chartjs-2';
  import { faker } from '@faker-js/faker';

  
  import Box from '@mui/material/Box';
  import TextField from '@mui/material/TextField';
  import MenuItem from '@mui/material/MenuItem';


import dayjs from 'dayjs';
import { DemoContainer, DemoItem } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';


import * as mstrsActions from "../../redux/actions/mstrsActions";
import {  MstrsState } from "../../redux/reducers/mstrsReducer";
import { ApiMstrsServiceCallsInProgressState } from "../../redux/reducers/msrtsServiceApiReducer";
import { connect } from "react-redux";


import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';

  //This option can be dynamic in feature
  const currencies = [
    {
      value: 'tempSensor_1',
      label: 'airSystemPoint_1/tempSensor_1',
    },
    {
      value: 'pressSensor_1',
      label: 'airSystemPoint_1/pressSensor_1',
    },
    {
      value: 'pressSensor_5',
      label: 'collingSystemPoint_1/pressSensor_5',
    },
    {
      value: 'tempSensor_5',
      label: 'collingSystemPoint_1/tempSensor_5',
    },
  ];

  interface ChartData {
    labels: string[];
    datasets: Array<{
      label: string;
      data: number[]; // Załóżmy, że data to liczby
      borderColor: string;
      backgroundColor: string;
    }>;
  }

  

  ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
  );

  




  type PropsFromState = ReturnType<typeof mapStateToProps>;
type PropsFromDispatch = ReturnType<typeof mapDispatchToProps>;


type Props = PropsFromState & PropsFromDispatch;

export interface AppState {
  mstrs: MstrsState;
  apiMstrsServiceCallsInProgress: ApiMstrsServiceCallsInProgressState;
  loading: boolean;
  loggedUser: any;
}


  const TrendsPage = ({mstrs, loading, loadMstrs, loggedUser}: Props) => {



    const now = dayjs();
    const twoHourAgo = now.subtract(2, 'hour');
  const oneHourAgo = now.subtract(1, 'hour');

    const [chartTitle, setChartTitle] = useState('Wybierz sensor'); 
    const [selectedSensor, setSelectedSensor] = useState('tempSensor_1');
    const [startTime, setStartTime] = useState(twoHourAgo);
    const [endTime, setEndTime] = useState(oneHourAgo);



    const handleSensorChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      setSelectedSensor(event.target.value);
      loadData(selectedSensor, startTime, endTime);

      const selectedLabel = currencies.find(c => c.value === event.target.value)?.label;
      setChartTitle(selectedLabel || 'Wybrany sensor');
    };
    
    const options = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top' as 'top',
        },
        title: {
          display: true,
          text: chartTitle,
        },
      },
    };

    const [chartData, setChartData] = useState<ChartData>({
      labels: [],
      datasets: [
        {
          label: 'Pomiary',
          data: [],
          borderColor: 'rgb(255, 99, 132)',
          backgroundColor: 'rgba(255, 99, 132, 0.5)',
        },
      ],
    });

  

    



    const loadData = async (selectedSensor: string, startTime: any, endTime: any) => {
      const fetchAlarms = async () => {
        try {
          await loadMstrs(selectedSensor, startTime, endTime);
        } catch (error) {
          alert("Loading alarms failed: " + error);
        }
      };

      console.log(mstrs);

        fetchAlarms();


      const dataFromDB = mstrs; // Twoja funkcja do pobierania danych
      const labels = dataFromDB.map(item => new Date(item.DataTimeMs).toLocaleString());
      const data = dataFromDB.map(item => item.MsrtValue);

      setChartData({
        ...chartData,
        labels: labels,
        datasets: [{ ...chartData.datasets[0], data: data }]
      });
    };

    const handleStartAccept = async (newValue: any) => {
      setStartTime(newValue);
      console.log('Czas rozpoczęcia:', newValue.format());
    
      //if (newValue && endTime) {
        // Możesz dodać dodatkową logikę, aby sprawdzić, czy endTime jest ustawione
        await loadData(selectedSensor, newValue, endTime);
      //}
    };
    
    const handleEndAccept = async (newValue: any) => {
      setEndTime(newValue);
      console.log('Czas zakończenia:', newValue.format());
    
      //if (newValue && startTime) {
        // Możesz dodać dodatkową logikę, aby sprawdzić, czy startTime jest ustawione
        await loadData(selectedSensor, startTime, newValue);
      //}
    };

  
    


    /*useEffect(() => {

      if (selectedSensor && startTime && endTime) {
        try {
          console.log(`Loading Mstrs with sensor: ${selectedSensor}, start: ${startTime}, end: ${endTime}`);
          loadData(selectedSensor, startTime.format(), endTime.format());
        } catch (error) {
          alert("Loading alarms failed: " + error);
        }
      }

    }, [mstrs.length, loadMstrs, selectedSensor, startTime, endTime]);*/

    useEffect(() => {

      //loadData(selectedSensor, startTime, endTime);

    }, [selectedSensor, loadMstrs, mstrs]);
  

    return (
<>
      {(loggedUser == "engineer") || (loggedUser == "admin")  ? (
        // Renderuj, gdy użytkownik jest zalogowany
        <>
    <Box
          component="form"
          sx={{
            '& .MuiTextField-root': { m: 1, width: '25ch' },
          }}
          noValidate
          autoComplete="off"
        >
    <div style={{ display: 'flex', justifyContent: 'center', marginTop: '20px' }}>
    <TextField
          id="outlined-select-currency"
          select
          label="Select"
          value={selectedSensor} // Użyj stanu selectedSensor
          onChange={handleSensorChange} // Dodaj obsługę zmiany
          helperText="Please select your channel identifier"
          style={{ width: '20%' }}
        >
          {currencies.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </TextField>
    </div>
            <div style={{ display: 'flex', justifyContent: 'center', marginTop: '20px' }}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <div style={{ marginRight: '10px' }}> {/* Stosowanie stylu bezpośrednio na kontenerze */}
          <DemoItem label="From">
            <DateTimePicker
              value={startTime}
              onChange={(newValue: any) => setStartTime(newValue)}
              onAccept={handleStartAccept}
            />
          </DemoItem>
        </div>
        
        <div> {/* Drugi DateTimePicker bez dodatkowego marginesu */}
          <DemoItem label="To">
            <DateTimePicker
              value={endTime}
              onChange={(newValue: any) => setEndTime(newValue)}
              onAccept={handleEndAccept}
            />
          </DemoItem>
        </div>
      </LocalizationProvider>
    </div>
      
            <Line options={options} data={chartData} />
          </Box>
        </>
        
      ) : (
        // Renderuj formularz logowania, gdy użytkownik nie jest zalogowany
        <>
        <Stack sx={{ width: '100%' }} spacing={2}>
          <Alert variant="filled" severity="error">
            Please login as an engineer to see trend page!
          </Alert>
        </Stack>
        </>
      )}


</>
    );
  };


  function mapStateToProps(state: AppState)  {
    return {
      mstrs:
        state.mstrs.length === 0
          ? []
          : state.mstrs.map((mstr: any) => {
              return {
                ...mstr,
              };
            }),
      loading: state.apiMstrsServiceCallsInProgress > 0,
      loggedUser : state.loggedUser,
    };
  }
  
  function mapDispatchToProps(dispatch: any) {
    return {
      dispatch,
      loadMstrs: (channelIdentifier: string, startDate: string, endDate: string) => dispatch(mstrsActions.loadMstrs(channelIdentifier, startDate, endDate)),
    };
  }
  type OwnProps = {};
  
  export default connect<PropsFromState, PropsFromDispatch,OwnProps, AppState>(
    mapStateToProps,
    mapDispatchToProps
  )(TrendsPage);

