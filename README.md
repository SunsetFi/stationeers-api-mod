# WebAPI for Stationeers

Provies a RESTful API for reading and writing data to a live Stationeers game.

# Installation

Requires [BepInEx 5.0.1](https://github.com/BepInEx/BepInEx/releases) or later.

1. Install BepInEx in the Stationeers steam folder.
2. Launch the game, reach the main menu, then quit back out.
3. In the steam folder, there should now be a folder BepInEx/Plugins
4. Create a folder `stationeers-webapi` in the BepInEx/Plugins folder.
5. Extract the release zip file to this folder.

# Usage

Make HTTP requests to `localhost:4444`.
Expect responses to be `application/json`

## Supported Requests

### GET /devices

Gets an array of all devices

### GET /devices/:deviceId

Gets a device by reference id.

### POST /devices/:deviceId

Modifies device properties

#### Supported properties

- `customName`: Change the labeler-given name of a device.

### GET /devices/:deviceId/logic

Gets all readable logic values for a device.

### GET /devices/:deviceId/logic/:logicType

Gets a readable logic value by logic type.

### POST /devices/:deviceId/logic/:logicType

Writes a writable logic value by logic type.

#### Body

JSON object with the following properties:

- value (number): The value to write to the logic type.

Example:

```json
{
  "value": 42
}
```

# TODO

- Make port configurable
- More endpoints
  - Endpoints for Things in general?
    - Things derived from Things
      - DynamicThing ( => Item)
      - Structure ( => Device)
  - Endpoint for items `public static readonly List<Item> AllItems;`
  - Endpoint for posting chat messages
  - Devices with slots should give the referenceId of items in their slots.
