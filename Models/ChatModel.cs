
using System.Linq;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class ChatModel
    {
        public static ChatPayload[] GetChat()
        {
            var chatPanelText = ChatPanel.Instance.Text.text;
            return chatPanelText.Split('\n').Select(message =>
            {
                var parts = message.Split(':');
                return new ChatPayload()
                {
                    displayName = parts[0],
                    message = string.Join(":", parts.Skip(1))
                };
            }).ToArray();
        }
    }
}