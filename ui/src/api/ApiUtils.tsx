export async function handleResponse(response: any) {
  if (response.status >= 200 && response.status < 300) {
    return response.data; // Jeśli używasz axios, to może być po prostu response.data bez oczekiwania.
  }
  
  // Tu możesz dodać obsługę różnych kodów błędów i logowanie ich dla lepszego zrozumienia problemu
  console.error(`HTTP Error: ${response.status}`);

  const error = await response.text();
  throw new Error(error || "Network response was not ok.");
}

//OLD
// export async function handleResponse(response) {
//   if (response.ok) return response.data;
//   if (response.status === 400) {
//     // So, a server-side validation error occurred.
//     // Server side validation returns a string error message, so parse as text instead of json.
//     const error = await response.text();
//     throw new Error(error);
//   }
//   throw new Error("Network response was not ok.");
// }

// In a real app, would likely call an error logging service.
export function handleError(error: any) {
  // eslint-disable-next-line no-console
  console.error("API call failed. " + error);
  throw error;
}
