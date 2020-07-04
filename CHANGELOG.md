### 2.0.0

- Switch to Ceen http library to allow for ssl security.
- Disable builtin rcon server; default to the rcon server port.
- Make port configurable.
- Nest all endpoints under /api
- Add steam openid authentication.
- `authenticationMode` config option:
  - `none`: No authentication.
  - `steam`: Users must authenticate through steam.
- Fix crashes when dealing with server name and password.
- Fix kick/ban being GET requests, should be POST.
- Add ping, score, and playtime to player payload.
- Add GET bans and DELETE ban.
- Fix duplicate items in Devices and Items.
- Change device logicValues to logicTypes; value is now `{value, writable}`.
- GET /thing and GET/POST /thing/:thingId endpoints.
- Rename /server endpoint to /settings
- Added /settings properties
  - maxPlayers
  - startingCondition
  - respawnCondition
- Added /settings/starting-conditions to get all starting conditions.
- Added /settings/respawn-conditions to get all respawn conditions.
- Added /atmospheres endpoint for reading atmospheric data.
- Added /pipe-networks endpoint for reading pipe networks.
- Added /saves endpoint for creating new saves.
- Rework endpoint server to support attribute based controllers
- Make route and controller API public to allow other mods to add their own endpoints.

### 1.2.0

- Show thing prefabHash and prefabName.
- Show thing reference ids in slots.
- Show thing health.
- Show thing access state.
- Support writing accessState to devices.
- Items endpoint:
  - item parent slotId and referenceId.
  - Item quantity text.
- Server endpoint
  - Get name, password, maxPlayers
  - Set name, password
  - Send chat messages
- Players endpoint
  - Get player names, steamIds
  - Kick players, ban steamIds

### 1.1.0

- Include string reference id for javascript consumption.
- Support device renaming.
- Support logic slot values.

### 1.0.0

- Initial release.
- Support device listing.
- Support device logic listing.
- Support writing new logic values to devices.
