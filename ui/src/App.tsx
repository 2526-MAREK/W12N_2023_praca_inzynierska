import { BrowserRouter, Route, Routes } from "react-router-dom";
import MainLayout from "./components/layout/MainLayout";
import { routes } from "./routes";
import initialState from "./redux/reducers/initialState";

import store from "./redux/configureStore";

import WebSocketManager  from "./api/WebSocketInitializeApi";
import WebSocketInitializeModelsStructureJsonApi from "./api/WebSocketInitializeModelsStructureJsonApi"
import closeWebSocket from "./api/WebSocketInitializeApi"

function App() {

  return (
    <BrowserRouter>
    <WebSocketManager />
    <WebSocketInitializeModelsStructureJsonApi />
      <Routes>
        <Route path="/" element={<MainLayout state={initialState.appState}/>}>
          {routes}
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
