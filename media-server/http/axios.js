import axios from "axios";
import settings from "../settings/app-settings.json" assert { type: "json" };
import secrets from "../settings/secrets.json" assert { type: "json" };

const axiosInstance = axios.create({
  baseURL: settings.streamingApiUrl,
  headers: { "x-api-key": secrets.apiKey },
  validateStatus: function (status) {
    return true;
  },
});

export default axiosInstance;
