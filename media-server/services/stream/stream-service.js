const streamProxyService = require("./stream-proxy-service.js");

const publishers = require("node-media-server/src/node_core_ctx.js").publishers;
const sessions = require("node-media-server/src/node_core_ctx.js").sessions;

const usernameSessions = new Map();

const blockedStreamPaths = new Set();

async function getUsernameAsync(streamKey) {
  return await streamProxyService.getUsernameAsync(streamKey);
}

async function stopStreamAsync(username) {
  const status = await streamProxyService.stopStreamAsync(username);
  return status === 204;
}

function setNameSession(streamId, username, streamPath) {
  usernameSessions.set(username, { streamId, streamPath });
}

function getStreamPath(username) {
  const session = usernameSessions.get(username);

  return session?.streamPath ?? "";
}

function blockStreamPath(streamPath) {
  blockedStreamPaths.add(streamPath);
}

function removeBlockedStreamPath(streamPath) {
  blockedStreamPaths.delete(streamPath);
}

function isStreamPathBlocked(streamPath) {
  return blockedStreamPaths.has(streamPath);
}

module.exports = {
  getUsernameAsync,
  stopStreamAsync,
  setNameSession,
  getStreamPath,
  blockStreamPath,
  removeBlockedStreamPath,
  isStreamPathBlocked,
};
