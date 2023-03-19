import { useEffect, useState, React, useCallback } from "react";
import {
  GetCategories,
  UpdateFavoriteCategories,
} from "../../../services/category-services/category-services";
import { SwitchSelect } from "./switch-category-select/switchSelect";
import { useLocation, useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";

import "react-toastify/dist/ReactToastify.css";
import "../pages-css/pages.css";
import "./settingsPageStyle.css";

export const SettingsPage = ({ AuthID }) => {
  const [categoriesDict, setCategoriesDict] = useState({});
  const [allCategories, setAllCategories] = useState([]);
  const [selectedCategoryIDs, setSelectedCategoryIDs] = useState([]);
  const [hideChangesSaved, setHideChangesSaved] = useState(false);
  const [isFromNavigate, setIsFromNavigate] = useState(false);
  const [firstTimeNavigated, setFirstTimeNavigated] = useState(true);

  const navigate = useNavigate();
  const location = useLocation();
  let showSettingsMessage;

  // checking if we got to setting page by ourselves (for the first time)
  if (location.state !== null) {
    showSettingsMessage = location.state.showSettingsMessage;
  }

  const disabled = false;

  // we had a rerendaring issue that we fixed with making
  // an if statement that it will only happen only once
  if (
    showSettingsMessage &&
    selectedCategoryIDs.length === 0 &&
    firstTimeNavigated
  ) {
    setIsFromNavigate(showSettingsMessage);
    setFirstTimeNavigated(!firstTimeNavigated);
  }

  const AddIDToArray = useCallback((newSelectedCategoryIDs) => {
    setSelectedCategoryIDs(newSelectedCategoryIDs);
  }, []);

  const turnOffMessage = () => {
    setIsFromNavigate(!isFromNavigate);
  };

  const showMessage = () => {
    if (selectedCategoryIDs.length !== 0) {
      let stateChanged = !hideChangesSaved;
      setHideChangesSaved(stateChanged);
      console.log(stateChanged);
      UpdateFavoriteCategories(AuthID, selectedCategoryIDs);
      if (selectedCategoryIDs.length > 0 && !stateChanged) {
        navigate("/");
      }
    } else {
      toast("Please pick some categories!");
    }
  };

  useEffect(() => {
    // declare the data fetching function
    const handleCategories = async () => {
      const myDict = await GetCategories(AuthID);
      setCategoriesDict(myDict);
      setAllCategories(myDict["All"]);
      if (myDict["Favorite"]) {
        setSelectedCategoryIDs(
          myDict["Favorite"].map((Category) => {
            return Category.id;
          })
        );
      }
    };
    //Call the funciton in specific order of execution
    const getCategories = async () => {
      await handleCategories();
    };

    // call the function
    getCategories()
      // make sure to catch any error
      .catch(console.error);
  }, []);

  return (
    <div className="settings-page-container">
      <h1>All Categories</h1>
      <div className="option-container">
        {allCategories
          ? allCategories.map((Category) => {
              return (
                <div
                  className="option"
                  key={`${Category.id}_${selectedCategoryIDs.length}`}
                >
                  <SwitchSelect
                    Category={Category}
                    AddIDToArray={AddIDToArray}
                    selectedCategoryIDs={selectedCategoryIDs}
                    disabled={disabled}
                  />
                </div>
              );
            })
          : null}
      </div>

      <div className="categories-changed-message" hidden={!hideChangesSaved}>
        <span>Categories selection have been changed to:</span>
        <span>
          {" "}
          {selectedCategoryIDs !== undefined && categoriesDict["All"]
            ? categoriesDict["All"]
                .filter((category) => selectedCategoryIDs.includes(category.id))
                .map((category) => category.name)
                .join(", ")
            : ""}
        </span>
        <button
          type="button"
          className="btn btn-danger my-btn"
          onClick={() => {
            showMessage();
          }}
        >
          Go to Home-page
        </button>
      </div>
      <div className="welcome-message" hidden={!isFromNavigate}>
        <span>
          We are welcoming you to our website, in order to see articles, please
          choose your favorite categories (3 max)
        </span>
        <button
          type="button"
          className="btn btn-primary my-btn"
          onClick={() => {
            turnOffMessage();
          }}
        >
          Close
        </button>
      </div>
      <div className="accept-btn">
        <button
          type="button"
          className="btn btn-primary my-btn"
          onClick={() => {
            showMessage();
          }}
        >
          Accept
        </button>
      </div>
      <ToastContainer />
    </div>
  );
};
