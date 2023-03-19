import axios from "axios";

const URLServer = "https://localhost:7137/Api/News/";

export const GetCategories = async (AuthID) => {
  try {
    console.log("Auth id" + AuthID);
    let endpointCategories = `${URLServer}GetCategories/${AuthID}`;
    let response = await axios.get(endpointCategories);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const UpdateFavoriteCategories = async (AuthID, FavoriteCategories) => {
  try {
    let endpointCategories = `${URLServer}UpdateFavoriteCategories/${AuthID}/${FavoriteCategories}`;
    let response = await axios.post(endpointCategories);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};
