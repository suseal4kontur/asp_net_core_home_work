﻿using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using API.Users;
using View.Users;

namespace API.Auth
{
    public sealed class Authenticator : IAuthenticator
    {
        private readonly IUserRepository userRepository;
        private readonly ConcurrentDictionary<string, SessionState> sessions;

        public Authenticator(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            this.userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            sessions = new ConcurrentDictionary<string, SessionState>();
        }

        public async Task<SessionState> AuthenticateAsync(
            string login,
            string password,
            CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            cancellationToken.ThrowIfCancellationRequested();

            User user;

            try
            {
                user = await this.userRepository.GetAsync(login).ConfigureAwait(false);
            }
            catch (UserNotFoundException)
            {
                throw new AuthenticationException();
            }

            var currentHash = GetPasswordHash(password);

            if (!user.PasswordHash.Equals(currentHash))
            {
                throw new AuthenticationException();
            }

            var sessionId = Guid.NewGuid().ToString();
            var sessionState = new SessionState(sessionId, user.Id);

            while (!sessions.TryAdd(sessionId, sessionState))
            {
            }

            return sessionState;
        }

        public Task<SessionState> GetSessionAsync(string sessionId)
        {
            if (sessionId == null)
            {
                throw new ArgumentNullException(nameof(sessionId));
            }

            if (!sessions.TryGetValue(sessionId, out var sessionState))
            {
                throw new AuthenticationException();
            }

            return Task.FromResult(sessionState);
        }

        public Task<SessionState> DeleteSessionAsync(string sessionId)
        {
            if (sessionId == null)
            {
                throw new ArgumentNullException(nameof(sessionId));
            }

            SessionState session;
            while (!sessions.TryRemove(sessionId, out session))
            {
            }

            return Task.FromResult(session);
        }

        public void DeleteExpiredSessions()
        {
            foreach (var session in sessions)
            {
                if (sessions[session.Key].IsExpired())
                {
                    while (!sessions.TryRemove(session.Key, out _))
                    {
                    }
                }
            }
        }

        public static string GetPasswordHash(string password)
        {
            using var md5 = MD5.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(passwordBytes);
            var hash = BitConverter.ToString(hashBytes);
            return hash;
        }
    }
}
