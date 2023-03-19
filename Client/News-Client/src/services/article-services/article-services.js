import axios from "axios";

const URLServer = "https://localhost:7137/Api/News/";

export const GetTopArticles = async (AuthID) => {
  try {
    let endpointCategories = `${URLServer}GetTopArticles/${AuthID}`;
    let response = await axios.get(endpointCategories);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const UpdateNumberOfClicks = async (AuthID, ArticleID) => {
  try {
    console.log(AuthID, ArticleID);
    let endpointCategories = `${URLServer}UpdateNumberOfClicks/${AuthID}/${ArticleID}`;
    let response = await axios.post(endpointCategories);
    console.log(response.data);
  } catch (error) {
    throw error;
  }
};

export const GetPopularArticles = async (AuthID) => {
  try {
    let endpointCategories = `${URLServer}GetPopularArticles/${AuthID}`;
    let response = await axios.get(endpointCategories);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const GetCuriousArticles = async (AuthID) => {
  try {
    console.log("Yo " + AuthID);
    let endpointCategories = `${URLServer}GetCuriousArticles/${AuthID}`;
    let response = await axios.get(endpointCategories);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};
