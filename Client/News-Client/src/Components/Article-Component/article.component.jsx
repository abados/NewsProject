import React from "react";
import { UpdateNumberOfClicks } from "../../services/article-services/article-services";
import "./Article.component.css";

export const Article = ({ title, link, image, description, ID, AuthID }) => {
  return (
    <div className="card my-card-home">
      <div className="title-home">{title}</div>
      <img
        src={image}
        className="card-img-top my-img-home"
        alt="..."
      />
      <div className="card-body my-card-body-home">
        <p className="text-home">{description}</p>
        <a
          href={link}
          rel="noreferrer"
          target="_blank"
          className="btn btn-primary"
          onClick={() => UpdateNumberOfClicks(AuthID, ID)}
        >
          Enter article
        </a>
      </div>
    </div>
  );
};
