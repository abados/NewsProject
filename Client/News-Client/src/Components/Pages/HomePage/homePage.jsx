import React, { useEffect, useState } from "react";
import { GetTopArticles } from "../../../services/article-services/article-services";
import { useNavigate } from "react-router-dom";
import { ClockLoader } from "react-spinners";
import { Article } from "../../Article-Component/article.component";
import "../pages-css/pages.css";
import "./homePage.css";

export const HomePage = ({ AuthID }) => {
  const [articlesDict, setArticlesDict] = useState(null);
  const [isNavigationTriggered, setIsNavigationTriggered] = useState(false);
  const navigate = useNavigate();

  const handleGetArticles = async () => {
    let articles = await GetTopArticles(AuthID);
    setArticlesDict(articles);
  };

  useEffect(() => {
    handleGetArticles();
  }, [AuthID]);

  useEffect(() => {
    const checkIfDictHasData = () => {
      if (
        articlesDict === "No categories selected yet" &&
        !isNavigationTriggered
      ) {
        navigate("/Settings", { state: { showSettingsMessage: true } });
        setIsNavigationTriggered(true);
      }
    };
    checkIfDictHasData();
  }, [articlesDict, navigate, isNavigationTriggered]);

  return (
    <div className="page-container">
      {articlesDict === null ? (
        <ClockLoader
          color="#36d7b7"
          size={86}
        />
      ) : articlesDict !== "No categories selected yet" ? (
        Object.keys(articlesDict).map((categoryName) => (
          <div
            className="option-container-home"
            key={categoryName}
          >
            <h2 className="headline-home">{categoryName}</h2>
            <div className="option-home">
              {articlesDict[categoryName].map((article) => (
                <Article
                  key={article.ID}
                  title={article.Title}
                  link={article.Link}
                  image={article.Image}
                  description={article.Description}
                  AuthID={AuthID}
                  ID={article.ID}
                />
              ))}
            </div>
          </div>
        ))
      ) : null}
    </div>
  );
};
