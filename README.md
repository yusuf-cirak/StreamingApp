<!-- @format -->

# Streaming App

Streaming App is a live streaming application like Twitch.

## Table of Contents

- [Architecture Diagram](#architecture-diagram)
- [Features](#features)

## Architecture Diagram

![Architecture Diagram](./.docs/architecture-diagram.png)

## Features

Streaming App includes the following features:

- **Live streaming:** Users can live stream by using their "stream key" via OBS.

- **Stream chat:** Stream chat is available when streamer is online or offline.

- **Following streamers:** Users can follow streamers.

- **Recommended streamers:** Users will get recommendations based on the current live streamers and the streamers they follow.

- **Uploading profile or stream pictures:** Users can upload pictures for their profiles or their stream thumbnails.

- **Stream Moderators:** Users can assign other users as a moderator of their stream with the operations they want.

- **Blocking users from stream:** Users can be blocked-unblocked from a stream by streamer or moderators.

- **Chat Options:** Streamers or authorized moderators can make changes about the stream options. Options includes: Chat delay second, must be follower to use chat, enable or disable chat.

- **Real-time notifications:** Users will receive real-time notifications when in stream chat, when stream is started or ended, chat options changed, when blocked or unblocked from stream, when assigned as moderator or removed from moderators.

## Git hooks

This repo contains a pre-push hook that blocks pushes when the backend fails to build.

Setup (one-time per clone):

1. Tell Git to use the versioned hooks directory
	- `git config core.hooksPath .githooks`
2. Ensure the script is executable (on Windows with Git Bash it runs without chmod, but it's fine to set it):
	- `chmod +x .githooks/pre-push`

Behavior:

- On `git push`, it runs `dotnet build StreamingApp.sln -c Release`.
- If the build fails, the push is aborted.
- To temporarily skip (CI or emergencies): set environment variable `SKIP_PRE_PUSH_BUILD=1` for that push.

Examples (Git Bash):

- Enable hooks: `git config core.hooksPath .githooks`
- Push normally: `git push`
- Skip once: `SKIP_PRE_PUSH_BUILD=1 git push`
