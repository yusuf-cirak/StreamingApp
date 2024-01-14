import axiosInstance from "../../http/axios.js";

const STREAMS_URL = "/streams";

async function getUsernameAsync(streamKey) {
  const response = await axiosInstance.post(STREAMS_URL, { streamKey });

  return response;
}

async function stopStreamAsync(username) {
  const response = await axiosInstance.patch(STREAMS_URL, { username });

  return response.status;
}

export default { getUsernameAsync, stopStreamAsync };
