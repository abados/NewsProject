import React, { useEffect, useState } from "react";
import { ClockLoader } from "react-spinners";
import { GetCuriousArticles } from "../../../services/article-services/article-services";
import { Article } from "../../Article-Component/article.component";
import "../pages-css/pages.css";

export const CuriousPage = ({ AuthID }) => {
  const [articlesList, setArticlesDict] = useState([]);
  const handleGetArticles = async () => {
    let articles = await GetCuriousArticles(AuthID);
    setArticlesDict(articles);
  };

  useEffect(() => {
    handleGetArticles();
  }, [AuthID]);

  return (
    <div className="page-container">
      {articlesList === null ? (
        <ClockLoader
          color="#36d7b7"
          size={86}
        />
      ) : (
        <>
          <h2 className="headline-home">Random articles you never saw</h2>
          <div className="option-container-home">
            <div className="option-home">
              {articlesList.map((article) => (
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
        </>
      )}
    </div>
  );
};
