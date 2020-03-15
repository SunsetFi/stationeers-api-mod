### vNext

- Switch to Ceen http library for future ssl security.
- Make port configurable.
- Add steam openid authentication.
- Fix crashes when dealing with server name and password.
- Fix kick/ban being GET requests, should be POST.
- Add player ping / score / playtime to player payload.
- Add GET bans and DELETE ban

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
