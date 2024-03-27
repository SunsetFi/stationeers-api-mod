
using Assets.Scripts;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.PlayerInfo;
using UnityEngine;

namespace StationeersWebApi.Payloads
{
    public class PlayerPayload
    {
        public string clientId { get; set; }

        public string playerName { get; set; }

        public int playTime { get; set; }
        public int ping { get; set; }

        public int score { get; set; }

        public Vector3Payload location { get; set; }

        public static PlayerPayload FromPlayerConnection(Client client)
        {
            var human = Human.AllHumans.Find(x => x.OrganBrain.ClientId == client.ClientId);
            var payload = new PlayerPayload
            {
                clientId = client.ClientId.ToString(),
                location = Vector3Payload.FromVector3(human != null ? human.transform.position : Vector3.zero)
            };

            // ClientId and SteamId seem to be the same... PlayerInfoManager sets SteamId from NetworkManager.LocalClientId.
            if (PlayerInfoManager.PlayerDictionary.TryGetValue(client.ClientId, out var playerDetail))
            {
                payload.playerName = playerDetail.PlayerName;
                payload.ping = playerDetail.PingMs;
                payload.score = playerDetail.Score;
                payload.playTime = PlayerInfoManager.Instance.GetPlayTime(playerDetail.StartPlayTime);
            }

            return payload;
        }
    }
}