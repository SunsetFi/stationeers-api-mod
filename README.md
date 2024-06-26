# StationeersWebApi for Stationeers

Provies a RESTful API for reading and writing data to a live Stationeers game.

# Installation

Requires [BepInEx 5.0.1](https://github.com/BepInEx/BepInEx/releases) or later.

1. Install BepInEx in the Stationeers steam folder.
2. Launch the game, reach the main menu, then quit back out.
3. In the steam folder, there should now be a folder BepInEx/Plugins
4. Create a folder `StationeersWebApi` in the BepInEx/Plugins folder.
5. Extract the release zip file to this folder.

# Compatibility

This mod has been tested against Stationeers 0.2.4889.

# Usage

Make HTTP requests to your server's port.
Expect responses to be `application/json`

# Configuration

Create a file called `config.json` inside the `stationeers-StationeersWebApi` folder under `BepInEx/Plugins`.
This file should be a json object with the following properties.

- `enabled` (bool): Specify whether the mod should be enabled or disabled. Set to `true` to enable.
- `port` (number): The port to use. Defaults to your server's game port.
- `passwordAuthentication`: Settings for unsecure plaintext password authentication.
  - `enabled`: Set to true to enable this authentication mechanism.
  - `password`: The password to require.
- `jwtSecret` (string, optional): The encryption key to encode the user's login token. If unset, a random key will be used.
  - Note: Set this if you want to remember logins across server restarts.

# Security

Currently, only HTTP is supported. This is an unsecure protocol, so the authentication mechanism is vulnurable to man in the middle attacks.

HTTPS support has been abandoned for the time being as Stationeers is now on a unity version that is incompatible with the previous secure web request library being used.

## Authentication

Authentication controls how the mod determines who is and is not allowed to execute commands on the server. Multiple authentication mechanisms can be
enabled at the same time and used concurrently.

### Anonymous Authentication

If neither password nor steam authentication is enabled, the server will allow anonymous users access without any authentication.
This is not a discrete mode, but instead only applies when no other mode is available.

### Password Authentication

Password authentication accepts a plaintext (unsecured) password. At the moment, there is only support for one password, and it will grant access to all api endpoints.
To enable password authentication, set the configuration option`passwordAuthentication.enabled` to `true`, and set your password in `passwordAuthentication.password`.

To login with a password, make a request to `/api/login/password?password=foobar`, where `foobar` is the configured password.

## Login Tokens

For all authentication methods that require a login, the user must log in using the appropriate endpoint. On a successful login, the mod will return a login token under the `authorization` property.
This token must be sent with all requests in one of two ways:

- In an `Authorization` cookie.
- In an `Authorization` header, in the form `Bearer <token>`.

Failure to send this token, or sending an invalid token, will result in the command being rejected.

These tokens uniquely identify your user to the system, and connect your login with your future requests. They are valid for 1 hour, after which you must log in again.
These tokens are encrypted. By default, the encryption key is randomly generated each time the server starts up. You may choose to use a persistent key by specifying the `jwtSecret` configuration property. However, if a third party obtains this key, they will be able to generate forged login requests and bypass security. It is recommended that this parameter remains empty so a random encrpytion key will be used every server restart.

For additional security, tokens are bound to the ip address of the client when the token was issued. If the client's ip changes, they will need to login again. This is to provide a small amount of protection against the case where a token is intercepted by a malacious third party, as can happen when using the http protocol.

## Supported Requests

### GET /api/status

Gets the general server status.

### GET /api/settings

Gets game and server settings.

### POST /api/settings

#### Body

- `name` (string, optional): Sets the server name.
- `password` (string, optional): Sets the server password
- `maxPlayers` (int, optional): Sets the max players allowed on the server.
- `startingCondition` (string, optional): Sets the starting condition for new players.
- `respawnCondition` (string, optional): Sets the respawn condition for new players.

### GET /api/settings/starting-conditions

Gets an array of valid starting conditions on this server.

### GET /api/settings/respawn-conditions

Gets an array of valid respawn conditions on this server.

### GET /api/players

Gets a list of all players on the server

### POST /api/players/:steamId/kick

Kick a player from the server.
steamId must be a steamId of a player on the server.

#### Body

- `reason` (string, optional): The reason message for the kick. Can be empty.

### POST /api/players/:steamId/ban

Bans a player from the server.
steamId should be a valid steamId, but the player does not have to be on the server. If they are on the server, they will be kicked.

#### Body

- `hours` (int): The number of hours to ban the player for.
- `reason` (string, optional): The reason message for the ban. Can be empty.

### GET /api/things

Gets an array of all things.

#### Query Params

- `prefabName` (optional): Only return things with the given prefab name.
- `prefabHash` (optional): Only return things with the given prefab hash.

### GET /api/things/:thingId

Gets a thing by its id.

### POST /api/things/:thingId

Change properties of a thing.

#### Body

- `customName` (string, optional): Change the labeler-given name of a device.
- `accessState` (int, optional): Set the bitmask of allowed access card colors.

### GET /api/devices

Gets an array of all device things.

### POST /api/devices/query

Queries devices.

Each body property takes an array of values, using `OR` logic. A device will match if it matches any item
in the list.

By default, all body properties are treated with `OR` logic. That is, if a device matches any of the properties,
it will be included. This can be changed to `AND` logic by setting `matchIntersection` to true, which will include a device only if it matches at least one value for every given body property.

#### Body

- `referenceIds` (string array, optional): An array of reference ids to filter by.
- `prefabNames` (string array, optional): An array of prefab names to filter by.
- `prefabHashes` (int array, optional): An array of prefab hashes to filter by.
- `displayNames` (string array, optional): An array of display names to filter by.
- `dataNetworkIds` (string array, optional): An array of data network ids to filter by.
- `matchIntersection` (boolean, optional): Whether to require an intersection of all filters per device.

#### Query Params

- `prefabName` (optional): Only return devices with the given prefab name.
- `prefabHash` (optional): Only return devices with the given prefab hash.

### GET /api/devices/:deviceId

Gets a device by reference id.

### POST /api/devices/:deviceId

Change properties of a device.

#### Body

- `customName` (string, optional): Change the labeler-given name of a device.
- `accessState` (int, optional): Set the bitmask of allowed access card colors.

### GET /api/devices/:deviceId/logic

Gets all readable logic values for a device.

### GET /api/devices/:deviceId/logic/:logicType

Gets a readable logic value by logic type.

### POST /api/devices/:deviceId/logic/:logicType

Writes a writable logic value by logic type.

#### Body

- `value` (number): The value to write to the logic type.

### GET /api/cable-networks

Gets all cable networks

### GET /api/cable-networks/:referenceId

Gets a cable network by its reference id

### GET /api/cable-networks/:referenceId/devices

Gets all devices in a cable network

### POST /api/cable-networks/:referenceId/devices/query

Queries devices on the given cable network.

Each body property takes an array of values, using `OR` logic. A device will match if it matches any item
in the list.

By default, all body properties are treated with `OR` logic. That is, if a device matches any of the properties,
it will be included. This can be changed to `AND` logic by setting `matchIntersection` to true, which will include a device only if it matches at least one value for every given body property.

#### Body

- `referenceIds` (string array, optional): An array of reference ids to filter by.
- `prefabNames` (string array, optional): An array of prefab names to filter by.
- `prefabHashes` (int array, optional): An array of prefab hashes to filter by.
- `displayNames` (string array, optional): An array of display names to filter by.
- `dataNetworkIds` (string array, optional): An array of data network ids to filter by.
- `matchIntersection` (boolean, optional): Whether to require an intersection of all filters per device.

### GET /api/items

Gets an array of all item things.

### GET /api/atmospheres

Gets an array of all atmosphere cells.

### GET /api/pipe-networks

Gets an array of all pipe networks.

### POST /api/saves

Save the game

### GET /api/stationpedia/ic/instructions

Gets a dictionary of IC10 instructions.

### GET /api/stationpedia/logic/slottypes

Gets a dictionary of possible logic slot types.

### GET /api/stationpedia/logic/types

Gets a dictionary of possible logic types.

### GET /\*

For any GET request NOT in the /api/ folder, content will be served from the `web-content` folder of the plugin's folder.
For exact paths, the file will be returned if it exists. If the path is a directory, an `index.htm` or `index.html` file in that directory will be returned if it exists, or a directory listing will be used as a fallback.

# Compiling from source

This mod can be compiled using the .net SDK tools. Visual studio will do nicely, but any text editor will do as long as the .net sdk is installed.

## Dependencies

This mod makes use of both nuget dependencies, and takes dependencies on game files.

To properly work with this mod, the `GameDir` environment variable must be set to your stationeers game directory. Without this, the project
will not build. You should set this env var in the terminal that launches VSCode or Visual Studio in order to get proper intellisense support.

## Compiling

This project loads its dependencies from the Stationeers directory directly. To tell the project what directory to use, set the `StationeersGameDir` environment variable to your stationeers folder.

Assuming you have installed the .net sdk properly, the project can be built with `dotnet build` from the command line.

The compiled output will be dropped into the BepInEx plugins directory under the StationeersWebApi folder.
