import { initializeApp } from 'firebase/app';
import { getMessaging, isSupported, onMessage, getToken as getFcmToken, Messaging } from 'firebase/messaging';

//importScripts('https://www.gstatic.com/firebasejs/8.2.0/firebase-app.js');
//importScripts('https://www.gstatic.com/firebasejs/8.2.0/firebase-messaging.js');

const firebaseConfig = {
    apiKey: "AIzaSyCJhqbUKnAAlldOYcRr-nruK5qGDKTlG6s",
    authDomain: "testingenvironmentapp.firebaseapp.com",
    projectId: "testingenvironmentapp",
    storageBucket: "testingenvironmentapp.appspot.com",
    messagingSenderId: "208239815483",
    appId: "1:208239815483:web:695541b358c2e4d06b251e"
  };

  const app = initializeApp(firebaseConfig);

  let messaging: Messaging | null = null;
  
  const initMessaging = async () => {
    if (await isSupported()) {
      messaging = getMessaging(app);
    }
  };
  
  initMessaging();
  
  export const fetchToken = async (setTokenFound: (found: boolean) => void) => {
    if (!messaging) {
      setTokenFound(false);
      return;
    }
  
    try {
      const currentToken = await getFcmToken(messaging, { vapidKey: 'BKU_irP5GOxlP9HwosRAPJH_7vAKvECsvXP8JNe1QsvZG6EqntsxzUiXM422TYQ6OsTVhfXSxS8GoNhuVgGwP54' });
      if (currentToken) {
        console.log('Current token for client: ', currentToken);
        setTokenFound(true);
        // Możesz tu wysłać token do Twojego serwera API, jeśli to potrzebne
      } else {
        console.log('No registration token available. Request permission to generate one.');
        setTokenFound(false);
        // Tu możesz zaimplementować logikę proszenia użytkownika o zgodę na powiadomienia
      }
    } catch (err) {
      console.log('An error occurred while retrieving token. ', err);
      setTokenFound(false);
      // Tu możesz obsłużyć błędy
    }
  };
  
  export const askForPermissionToReceiveNotifications = async () => {
    try {
      const permission = await Notification.requestPermission();
      if (permission !== "granted") {
        throw new Error(`Permission not granted: ${permission}`);
      }
    } catch (error) {
      console.error("Could not get permission for notifications", error);
    }
  };
  export { messaging };