import DashboardPage from "../pages/dashboard/DashboardPage";
import HomePage from "../pages/home/HomePage";
import { RouteType } from "./config";
import AlarmsPage from "../pages/alarms/AlarmsPage";
import ComponentPageLayout from "../pages/component/ComponentPageLayout";
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined';
import AppsOutlinedIcon from '@mui/icons-material/AppsOutlined';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import FormatListBulletedOutlinedIcon from '@mui/icons-material/FormatListBulletedOutlined';
import FileDownloadOutlinedIcon from '@mui/icons-material/FileDownloadOutlined';
import AlertPage from "../pages/component/AlertPage";
import ButtonPage from "../pages/component/ButtonPage";
import ParametersPage from "../pages/parameters/ParametersPage";


import MainPageLayout from "../pages/systems/MainPageLayout";
import MainPage from "../pages/systems/MainPage";
import CoolingSystemPage from "../pages/systems/CoolingSystemPage";
import AirSystemPage from "../pages/systems/AirSystemPage";

import TrendsPage from "../pages/trends/TrendsPage";
import DiagnosticsPage from "../pages/diagnostics/DiagnosticsPage";


const appRoutes: RouteType[] = [
  {
    index: true,
    element: <HomePage />,
    state: "home"
  },
  {
    path: "/dashboard",
    element: <DashboardPage />,
    state: "dashboard",
    sidebarProps: {
      displayText: "Dashboard",
      icon: <DashboardOutlinedIcon />
    }
  },
    
  {
    path: "/systems",
    element: <MainPageLayout />,
    state: "systems",
    sidebarProps: {
      displayText: "Systems",
      icon: <FileDownloadOutlinedIcon />
    },
    child: [
      {
        index: true,
        element: <MainPage />,
        state: "systems.index"
      },
      {
        path: "/systems/main",
        element: <MainPage />,
        state: "systems.main",
        sidebarProps: {
          displayText: "Main"
        }
      },
      {
        path: "/systems/airsystem",
        element: <AirSystemPage />,
        state: "systems.airsystem",
        sidebarProps: {
          displayText: "Air System"
        }
      },
      {
        path: "/systems/coolingsystem",
        element: <CoolingSystemPage />,
        state: "systems.coolingsystem",
        sidebarProps: {
          displayText: "Cooling System"
        }
      }
    ]
  },
  {
    path: "/parameters",
    element: <ParametersPage />,
    state: "parameters",
    sidebarProps: {
      displayText: "Parameters",
      icon: <ArticleOutlinedIcon />
    }
  },
  {
    path: "/alarms",
    element: <AlarmsPage />,
    state: "alarms",
    sidebarProps: {
      displayText: "Alarms",
      icon: <FormatListBulletedOutlinedIcon />
    }
  },
  {
    path: "/trends",
    element: <TrendsPage />,
    state: "trends",
    sidebarProps: {
      displayText: "Trends",
      icon: <FormatListBulletedOutlinedIcon />
    }
  },
  {
    path: "/diagnostics",
    element: <DiagnosticsPage />,
    state: "diagnostics",
    sidebarProps: {
      displayText: "Diagnostics",
      icon: <FormatListBulletedOutlinedIcon />
    }
  }
];

export default appRoutes;