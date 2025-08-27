import axios from "axios";
import { API_BASE_URL } from "../config";

const instance = axios.create({
  baseURL: API_BASE_URL,
});

instance.interceptors.request.use(
  (config) => {
    const user = JSON.parse(localStorage.getItem("user"));
    if (user && user.token) {
      config.headers["Authorization"] = "Bearer " + user.token;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default instance;