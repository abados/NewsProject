import "./App.css";
import { useAuth0 } from "@auth0/auth0-react";
import { SiteManager } from "./Components/SiteManager/siteManager";
import { LoginPage } from "./Components/Pages/LoginPage/loginPage";

function App() {
  const { isAuthenticated, isLoading } = useAuth0();

  if (!isLoading) {
    if (isAuthenticated) {
      return (
        <>
          <SiteManager className="whole-page" />
        </>
      );
    } else {
      return (
        <div className="loginPage">
          <LoginPage />
        </div>
      );
    }
  } else {
    <h1>loading</h1>;
  }
}

export default App;
