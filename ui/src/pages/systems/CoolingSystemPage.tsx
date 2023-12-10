import SystemPageComponent from "../../components/system/SystemPageComponent";
import CoolingSystemSvg from "../svg/CoolingSystemSvg";

type SensorPosition = {
  x: number;
  y: number;
};

type Props = {};


const CoolingSystemPage = (props: Props) => {
  const sensorPositionsCooling: Record<string, SensorPosition> = {
    pressSensor_5: { x: 360, y: 180 },
    tempSensor_5: { x: 1130, y: 250 },
    // Add more sensors as needed
  };

  return <SystemPageComponent sensorPositions={sensorPositionsCooling} 
  systemPointKey="collingSystemPoint_1" 
  SvgComponent={CoolingSystemSvg} />;
};


export default CoolingSystemPage;