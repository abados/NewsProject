import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import "./logoutButtonStyle.css";

export const LogoutButton = () => {
  const { logout } = useAuth0();

  return (
    <button
      className="btn btn-danger"
      onClick={() => logout({ returnTo: window.location.origin })}
    >
      Log Out
    </button>
  );
};
