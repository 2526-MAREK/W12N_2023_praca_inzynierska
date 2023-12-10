import { handleResponse, handleError } from "./ApiUtils";
import axios from 'axios';


const baseUrl = 'http://localhost:5193/api/Alarm';

export function getAlarms() {
  //console.log("halo1")
    return axios.get(baseUrl)
      .then(handleResponse)
      .catch(handleError);
  }
