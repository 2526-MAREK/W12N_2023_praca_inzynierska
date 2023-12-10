import { handleResponse, handleError } from "./ApiUtils";
import axios from 'axios';


const baseUrl = 'http://localhost:5193/api/Login/Registration';

export function addRegistration() {
  //console.log("halo1")
    return axios.get(baseUrl)
      .then(handleResponse)
      .catch(handleError);
  }
