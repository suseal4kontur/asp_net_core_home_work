namespace View.Users
{
    using System;

    public class User
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
