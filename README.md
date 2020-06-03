# WebAPI for Stationeers

Provies a RESTful API for reading and writing data to a live Stationeers game.

# Installation

Requires [BepInEx 5.0.1](https://github.com/BepInEx/BepInEx/releases) or later.

1. Install BepInEx in the Stationeers steam folder.
2. Launch the game, reach the main menu, then quit back out.
3. In the steam folder, there should now be a folder BepInEx/Plugins
4. Create a folder `stationeers-webapi` in the BepInEx/Plugins folder.
5. Extract the release zip file to this folder.

# Compatibility

This mod should be compatible with both standard Stationeers game installs and the Stationeers Dedicated Server.

# Usage

Make HTTP requests to `localhost:4444`.
Expect responses to be `application/json`

# Configuration

Create a file called `config.json` inside the `stationeers-webapi` folder under `BepInEx/Plugins`.
This file should be a json object with the following properties.

- `enabled` (bool): Specify whether the mod should be enabled or disabled. Set to `true` to enable.
- `password` (string, optional): The password to invoke endpoints. Sent as a `password` query parameter.
- `port` (number): The port to use.

# Security

A password can be configured to protect all endpoints. However, the password and requests are currently sent http / plaintext.

To specify the password, send it as the `password` query parameter. eg `GET /devices?password=mypassword`

## Supported Requests

### GET /server

Gets information about the server.

### POST /server

#### Body

- `name` (string, optional): Sets the server name.
- `password` (string, optional): Sets the server password
- `maxPlayers` (int, optional): Sets the max players allowed on the server.
- `startingCondition` (string, optional): Sets the starting condition for new players.
- `respawnCondition` (string, optional): Sets the respawn condition for new players.

### GET /server/starting-conditions

Gets an array of valid starting conditions on this server.

### GET /server/respawn-conditions

Gets an array of valid respawn conditions on this server.

### POST /server/message

Sends a chat message to the server

#### Body

- `message` (string): The message to send to the server.

### GET /players

Gets a list of all players on the server

### POST /players/:steamId/kick

Kick a player from the server.
steamId must be a steamId of a player on the server.

#### Body

- `reason` (string, optional): The reason message for the kick. Can be empty.

### POST /players/:steamId/ban

Bans a player from the server.
steamId should be a valid steamId, but the player does not have to be on the server. If they are on the server, they will be kicked.

#### Body

- `hours` (int): The number of hours to ban the player for.
- `reason` (string, optional): The reason message for the ban. Can be empty.

### GET /bans

Gets an array of all bans.

### GET /bans/:steamId

Gets a ban by steam id.

### DELETE /bans/:steamId

Removes a ban by steam id.

### GET /chat

Gets an array of chat messages currently known to the server.

### GET /things

Gets an array of all things.

### GET /things/:thingId

Gets a thing by its id.

### POST /things/:thingId

Change properties of a thing.

#### Body

- `customName` (string, optional): Change the labeler-given name of a device.
- `accessState` (int, optional): Set the bitmask of allowed access card colors.

### GET /devices

Gets an array of all device things.

### GET /devices/:deviceId

Gets a device by reference id.

### POST /devices/:deviceId

Change properties of a device.

#### Body

- `customName` (string, optional): Change the labeler-given name of a device.
- `accessState` (int, optional): Set the bitmask of allowed access card colors.

### GET /devices/:deviceId/logic

Gets all readable logic values for a device.

### GET /devices/:deviceId/logic/:logicType

Gets a readable logic value by logic type.

### POST /devices/:deviceId/logic/:logicType

Writes a writable logic value by logic type.

#### Body

- `value` (number): The value to write to the logic type.

### GET /items

Gets an array of all item things.

# Compiling from source

This mod can be compiled using the .net SDK tools. Visual studio will do nicely, but any text editor will do as long as the .net sdk is installed.

## Dependencies

As this mod references many game dlls, you need to give it copies of these dlls for it to compile properly. The project is set up to expect these in the `/externals` folder

Copy the following files from `Stationeers/rocketstation_Data/Managed` to a folder called `/externals` in the root of the source directory:

- `Assembly-CSharp.dll`
- `Assembly-CSharp-firstpass.dll`
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll`
- `com.unity.multiplayer-hlapi.Runtime.dll`

Copy `BepInEx.dll` from your BepInEx install to the `/externals` folder.

Download JSON.net from [here](https://github.com/JamesNK/Newtonsoft.Json/releases) (version 12 is recommended). In the zip file, copy `/Bin/netstandard2.0/Newtonsoft.Json.dll` to the `/externals` folder.

## Compiling

Assuming you have installed the .net sdk properly, the project can be built with `dotnet build` from the command line.

# TODO

- More endpoints
  - Endpoints for derived from Things
    - DynamicThing ( => Item)
    - Structure ( => Device)
  - Endpoint for transmitters (special class of ILogicable seperate from devices)
  - Endpoint for fabricators / manufacturing
    - Get recipes
    - post recipe to facbicator
- Test chat endpoint on dedicated servers
- Strip colors from chat name. Probably still include them in message.
