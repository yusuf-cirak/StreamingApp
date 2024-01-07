const axiosLib = require("axios");

const streamingApiUrl =
  require("../settings/app-settings.json").streamingApiUrl;

const apiKey = require("../settings/secrets.json").apiKey;

const axiosInstance = axiosLib.create({
  baseURL: streamingApiUrl,
  headers: { "x-api-key": apiKey },
  validateStatus: function (status) {
    return true;
  },
});

module.exports = axiosInstance;
