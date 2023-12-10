import * as React from 'react';
import { Box, Typography } from '@mui/material';
import { styled } from '@mui/material/styles';

interface RectangleComponentProps {
  x: number;
  y: number;
  content: string;
  unit: string;
  alarmStatus: 'no_alarm' | 'warning_alarm' | 'urgent_alarm';
  nameOfSensor: string;
}

const StyledRectangle = styled(Box, {
  shouldForwardProp: (prop: any) => !['alarmStatus', 'nameOfSensor'].includes(prop),
})<{ x: number; y: number, alarmStatus: string }>(({ theme, x, y, alarmStatus }) => ({
  position: 'absolute',
  top: `${y}px`,
  left: `${x}px`,
  width: '100px',
  height: '57px',
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'center', // Wycentrowanie zawartości w pionie
  alignItems: 'center', // Wycentrowanie poziome
  borderRadius: theme.shape.borderRadius,
  border: alarmStatus === 'no_alarm' ? `1px solid ${theme.palette.divider}` : 
  alarmStatus === 'warning_alarm' ? '3px solid orange' : 
  '4px solid red',
animation: alarmStatus === 'urgent_alarm' ? 'blinkAnimation 1s linear infinite' : 'none',

'@keyframes blinkAnimation': {
'0%': { borderColor: 'red' },
'50%': { borderColor: 'transparent' },
'100%': { borderColor: 'red' },
},
  background: theme.palette.background.paper,
  backdropFilter: 'blur(10px)',
  boxShadow: theme.shadows[1],
  padding: '8px 0', // Padding tylko w pionie
  cursor: 'pointer', // Zmiana kursora na 'pointer' przy najechaniu
  '&:hover': {
    boxShadow: theme.shadows[3], // Lekkie podświetlenie cienia przy najechaniu
    backgroundColor: theme.palette.action.hover, // Lekkie podświetlenie tła
  },
}));

const ContentBox = styled(Box)({
  display: 'flex',
  alignItems: 'flex-end', // Wyrównanie wartości i jednostki do dolnej linii
  gap: '4px', // Odstęp między wartością a jednostką
  marginLeft: '-8px', // Przesunięcie całej zawartości trochę do lewej
});

const NormalDisplayValueDevice: React.FC<RectangleComponentProps & { onClick: (event: React.MouseEvent<HTMLDivElement>) => void }> = ({ x, y, content, unit, alarmStatus, nameOfSensor, onClick }) => {
  return (
    <StyledRectangle x={x} y={y} alarmStatus={alarmStatus} onClick={onClick}>
        <Typography variant="caption" color="textPrimary" align="center">
        {nameOfSensor}
        </Typography>
        <ContentBox>
          <Typography variant="h5" color="textPrimary" component="span">
            {content}
          </Typography>
          <Typography variant="body2" color="textPrimary" component="span">
            {unit}
          </Typography>
        </ContentBox>
      </StyledRectangle>
    );
  };

export default NormalDisplayValueDevice;