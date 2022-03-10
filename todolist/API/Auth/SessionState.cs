using System;
using Newtonsoft.Json;

namespace API.Auth
{
    public class SessionState
    {
        private const long Minute = 60;
        private const long TimeExpire = 30 * Minute;

        public SessionState(string sessionId, string userId)
        {
            this.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Expire = DateTimeOffset.Now.ToUnixTimeSeconds() + TimeExpire;
        }

        private string SessionId { get; }

        private string UserId { get; }

        [JsonIgnore]
        private long Expire { get; set; }

        public void UpdateExpireTime()
        {
            this.Expire += TimeExpire;
        }

        public bool IsExpired()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds() - Expire >= TimeExpire;
        }
    }
}
