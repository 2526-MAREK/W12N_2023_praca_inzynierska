import { handleResponse, handleError } from "./ApiUtils";
import axios from 'axios';

//http://localhost:5193/api/Msrts/tempSensor_1?startDate=2023-11-23T12%3A00%3A00&endDate=2023-11-23T12%3A59%3A59.999
//startDate=2023-11-23T19%3A00%3A57.381Z&endDate=2023-11-23T20%3A00%3A57.381Z
const baseUrl = 'http://localhost:5193/api/Msrts/';

/*tempSensor_1
 startDate
2023-11-23T19:10:58+01:00
endDate
2023-11-23T20:10:58+01:00*/
export function getMstrs(channel: string, startDateInput: any, endDateInput: any) {
  //console.log("halo1")
  console.log("channel")
  console.log(channel)

  console.log("startDate")
  console.log(startDateInput)

  console.log("endDate")
  console.log(endDateInput)



// Konwersja na obiekty Date
let startDate = new Date(startDateInput);
let endDate = new Date(endDateInput);

function adjustDateForTimezone(date: any, offset: any) {
    return new Date(date.getTime() - (offset * 60 * 60 * 1000));
}

// Konwersja z powrotem na stringi
let adjustedStartDate = adjustDateForTimezone(startDate, -1);
let adjustedEndDate = adjustDateForTimezone(endDate, -1);

let startDateStr = adjustedStartDate.toISOString();
let endDateStr = adjustedEndDate.toISOString();
// Kodowanie URI
let startDateEncoded = encodeURIComponent(startDateStr);
let endDateEncoded = encodeURIComponent(endDateStr);

// Finalny format URL
let url = `startDate=${startDateEncoded}&endDate=${endDateEncoded}`;


    return axios.get(baseUrl + channel + '?' + url)
      .then(handleResponse)
      .catch(handleError);
  }
