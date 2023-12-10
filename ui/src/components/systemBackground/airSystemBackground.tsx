// Komponent Trapez.jsx
import React from 'react';
import { styled } from '@mui/material/styles';

const AirSystemBackground = styled('div')(({ theme }) => ({
  width: '100px', // szerokość podstawy trapezu
  height: '0', // trapez będzie tworzony za pomocą obramowania
  borderBottom: '30px solid', // wysokość trapezu
  borderColor: theme.palette.primary.main, // kolor trapezu
  borderLeft: '25px solid transparent', // skos lewy
  borderRight: '25px solid transparent', // skos prawy
  position: 'absolute',
  top: '100%', // ustawienie pod komponentem NormalDisplayValueDevice
}));

export default AirSystemBackground;