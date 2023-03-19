import React, { useEffect, useState } from "react";
import { ClockLoader } from "react-spinners";
import { GetPopularArticles } from "../../../services/article-services/article-services";
import { Article } from "../../Article-Component/article.component";
import "./popular-page.css";

export const PopularPage = ({ AuthID }) => {
  const [articlesDict, setArticlesDict] = useState(null);
  const handleGetArticles = async () => {
    let articles = await GetPopularArticles(AuthID);
    setArticlesDict(articles);
  };

  useEffect(() => {
    handleGetArticles();
  }, []);

  return (
    <div className="popular-page-container">
      {articlesDict === null ? (
        <ClockLoader color="#36d7b7" size={86} />
      ) : articlesDict !== null ? (
        <div className="container">
          <h1>Popular articles of your favorire categories</h1>

          {Object.keys(articlesDict).map((categoryName) => (
            <div className="option-container-popular" key={categoryName}>
              <h2 className="headline-home">{categoryName}</h2>
              <div className="option-home">
                {articlesDict[categoryName].map((article) => (
                  <Article
                    key={article.id}
                    title={article.title}
                    link={article.link}
                    image={article.image}
                    description={article.description}
                    AuthID={AuthID}
                    ID={article.ID}
                  />
                ))}
              </div>
            </div>
          ))}
        </div>
      ) : (
        <>
          {console.log(articlesDict)}
          <h1>No popular articles for those categories</h1>
        </>
      )}
    </div>
  );
};
