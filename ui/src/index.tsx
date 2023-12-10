import { CssBaseline } from '@mui/material';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { Provider as ReduxProvider } from "react-redux";
import App from './App';
import  configureStore  from './redux/configureStore';
import reportWebVitals from './reportWebVitals';
import initialState from './redux/reducers/initialState';

//jeśli mamy aplikację rendorowana na serwerze, to initialState może być pobrane z serwera, np wszystkie stany które są pamiętane w bazie danych
const store = configureStore(initialState);
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

// Rejestracja Firebase Messaging Service Worker
/*if ('serviceWorker' in navigator) {
  navigator.serviceWorker.register('/firebase-messaging-sw.js')
    .then(function (registration) {
      console.log('Firebase Messaging Service Worker registered with scope: ', registration.scope);
    })
    .catch(function (err) {
      console.log('Firebase Messaging Service Worker registration failed: ', err);
    });
}*/

root.render(
  <React.StrictMode>
    <ReduxProvider store={store}>
      <CssBaseline />
      <App />
      </ReduxProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
