import React from "react";
import { Route, Routes } from "react-router-dom";
import {
  HomePage,
  SettingsPage,
  PopularPage,
  CuriousPage,
} from "../Pages/index";
export const Routing = (AuthID) => {
  return (
    <Routes className="content">
      <Route
        path="/"
        element={<HomePage AuthID={AuthID.user.sub} />}
      ></Route>
      <Route
        path="/Settings"
        element={<SettingsPage AuthID={AuthID.user.sub} />}
      ></Route>
      <Route
        path="/Popular"
        element={<PopularPage AuthID={AuthID.user.sub} />}
      ></Route>
      <Route
        path="/Curious"
        element={<CuriousPage AuthID={AuthID.user.sub} />}
      ></Route>
    </Routes>
  );
};
