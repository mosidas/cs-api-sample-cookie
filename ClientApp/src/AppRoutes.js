import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Login } from "./components/Login";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/login',
    element: <Login />
  }
];

export default AppRoutes;
