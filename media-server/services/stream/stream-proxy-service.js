const axios = require("../../http/axios.js");

const STREAMS_URL = "/streams";

async function getUsernameAsync(streamKey) {
  const response = await axios.post(STREAMS_URL, { streamKey });

  return response;
}

async function stopStreamAsync(username) {
  const response = await axios.patch(STREAMS_URL, { username });

  return response.status;
}

module.exports = {
  getUsernameAsync,
  stopStreamAsync,
};
