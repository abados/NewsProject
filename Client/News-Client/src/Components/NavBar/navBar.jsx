import React from "react";
import HomeIcon from "@mui/icons-material/Home";
import SettingsIcon from "@mui/icons-material/Settings";
import EmojiEventsIcon from "@mui/icons-material/EmojiEvents";
import QuestionMarkIcon from "@mui/icons-material/QuestionMark";
import { Link } from "react-router-dom";
import { LogoutButton } from "../LogoutButton/logoutButton";

import "./style.css";

export const NavBar = () => {
  return (
    <div className="NavBar-bg">
      <ul>
        <li>
          <Link to="/">
            <HomeIcon fontSize="large" />
            <div className="section-name">Home</div>
          </Link>
        </li>
        <li>
          <Link to="/Settings">
            <SettingsIcon fontSize="large" />
            <div className="section-name">Settings</div>
          </Link>
        </li>
        <li>
          <Link to="/Popular">
            <EmojiEventsIcon fontSize="large" />
            <div className="section-name">Popular</div>
          </Link>
        </li>
        <li>
          <Link to="/Curious">
            <QuestionMarkIcon fontSize="large" />
            <div className="section-name">Curious</div>
          </Link>
        </li>
        <li>
          <LogoutButton />
        </li>
      </ul>
    </div>
  );
};
