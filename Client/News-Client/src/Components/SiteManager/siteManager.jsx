import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Routing } from "../Routing/routing";
import { NavBar } from "../NavBar/navBar";
import { Footer } from "../Footer/footer";

export const SiteManager = () => {
  const { user } = useAuth0();
  return (
    <>
      <NavBar />
      <Routing user={user} />
      <Footer></Footer>
    </>
  );
};
