
using Assets.Scripts;
using Assets.Scripts.PlayerInfo;

namespace StationeersWebApi.Payloads
{
    public class PlayerPayload
    {
        public string steamName { get; set; }
        public string steamId { get; set; }

        public string playerName { get; set; }

        public int playTime { get; set; }
        public int ping { get; set; }

        public int score { get; set; }

        public Vector3Payload location { get; set; }

        public static PlayerPayload FromPlayerConnection(PlayerConnection connection)
        {
            var payload = new PlayerPayload();

            PlayerDetail playerDetail;
            if (PlayerInfoManager.Instance.GetPlayer(connection.SteamId, out playerDetail))
            {
                payload.playerName = playerDetail.PlayerName;
                payload.ping = playerDetail.PingMs;
                payload.score = playerDetail.Score;
                payload.playTime = PlayerInfoManager.Instance.GetPlayTime(playerDetail.StartPlayTime);
            }

            payload.location = Vector3Payload.FromVector3(connection.Brain.ParentHuman.transform.position);

            payload.steamName = connection.SteamName;
            payload.steamId = connection.SteamId.ToString();
            return payload;
        }
    }
}