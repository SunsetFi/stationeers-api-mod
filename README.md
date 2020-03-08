# WebAPI for Stationeers

Provies a RESTful API for reading and writing data to a live Stationeers game.

# Installation

Requires [BepInEx 5.0.1](https://github.com/BepInEx/BepInEx/releases) or later.

# Usage

Make HTTP requests to `localhost:4444`.
Expect responses to be `application/json`

## Supported Requests

### GET /devices

Gets an array of all devices

### GET /devices/:deviceId

Gets a device by reference id.

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
