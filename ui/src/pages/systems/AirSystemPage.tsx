import SystemPageComponent from "../../components/system/SystemPageComponent";
import AirSystemSvg from "../svg/AirSystemSvg";

type SensorPosition = {
  x: number;
  y: number;
};

type Props = {};


const AirSystemPage = (props: Props) => {
  const sensorPositionsAir: Record<string, SensorPosition> = {
    pressSensor_1: { x: 1630, y: 660 },
    tempSensor_1: { x: 1630, y: 760 },



    pressSensor_2: { x: 1110, y: 290 },
    tempSensor_2: { x: 1110, y: 375 },

    controller_1: { x: 1370, y: 250 },
    rpmSensor_1: { x: 870, y: 670 },
    rpmSensor_2: { x: 730, y: 670 },

    pressSensor_3: { x: 530, y: 290 },
    tempSensor_3: { x: 530, y: 375 },

    pressSensor_4: { x: 350, y: 660 },
    tempSensor_4: { x: 350, y: 760 },
    // Add more sensors as needed
  };

  return <SystemPageComponent sensorPositions={sensorPositionsAir} 
  systemPointKey="airSystemPoint_1" 
  SvgComponent={AirSystemSvg}
  />;
};


export default AirSystemPage;