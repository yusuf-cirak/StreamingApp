const config = require("./config.js");

const streamService = require("./services/stream/stream-service.js");

const NodeMediaServer = require("node-media-server");

const nms = new NodeMediaServer(config);

nms.on("prePlay", (id, StreamPath, args) => {
  const username = StreamPath.split("/")[2];

  const session = nms.getSession(id);

  session["playStreamPath"] = streamService.getStreamPath(username);
});

nms.on("prePublish", async (id, StreamPath, args) => {
  const session = nms.getSession(id);

  const streamKey = StreamPath.split("/")[2];

  const usernameResult = await streamService.getUsernameAsync(streamKey);

  if (usernameResult.status === 200) {
    const username = usernameResult.data;
    streamService.setNameSession(id, username, StreamPath);
  } else {
    session.reject();
  }
});
nms.on("donePublish", async (id, StreamPath, args) => {
  const streamKey = StreamPath.split("/")[2];

  await streamService.stopStreamAsync(streamKey);
});

nms.run();
