using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Integration
{
    public static class TestUtilities
    {
        public static bool IsValidOAuth1Token(
            OAuth1Credentials token,
            bool shouldContainUserId)
        {
            if (shouldContainUserId && string.IsNullOrEmpty(token?.UserId))
            {
                return false;
            }
            return !string.IsNullOrEmpty(token?.ConsumerKey) &&
                   !string.IsNullOrEmpty(token?.ConsumerSecret) &&
                   !string.IsNullOrEmpty(token?.OAuthToken) &&
                   !string.IsNullOrEmpty(token?.OAuthSecret);
        }

        public static bool IsValidOAuth2Token(OAuth2Credentials token)
        {
            return !string.IsNullOrEmpty(token?.AccessToken) &&
                   !string.IsNullOrEmpty(token.TokenName) &&
                   !string.IsNullOrEmpty(token.ExpiresIn) &&
                   !string.IsNullOrEmpty(token.ClientId);
        }


        private static readonly Random random = new Random();

        public static int RandomNumber(int start, int end)
        {
            return random.Next(start, end);
        }

        public static string RandomString(int minimum, int maximum)
        {
            return RandomString(RandomNumber(minimum, maximum));
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.@/";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static T GetMemberValue<T>(this object instance, string memberName)
        {
            try
            {
                var bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static;

                var member = instance.GetType().GetField(memberName, bindingFlags);

                if (member != null)
                {
                    return (T)member.GetValue(instance);
                }
            }
            catch
            {

            }

            return default(T);
        }

        public static void SetPropertyValue(
            this object instance, 
            string propertyName, 
            object value)
        {
            try
            {
                var bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static;

                var property = instance.GetType().GetProperty(
                    propertyName, 
                    bindingFlags);

                property?.SetValue(instance, value);
            }
            catch(Exception e)
            {
                throw new Exception($"Property not set with message: {e.Message}");
            }
        }

        public static void SetMemberValue(
            this object instance,
            string memberName,
            object value)
        {
            try
            {
                var bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static;

                var member= instance.GetType().GetField(
                    memberName, 
                    bindingFlags);

                member?.SetValue(instance, value);
            }
            catch (Exception e)
            {
                throw new Exception($"Member not set with message: {e.Message}");
            }
        }

        private static List<int> _usedTestingPorts = new List<int>();

        private static readonly object _syncLock = new object();

        //Adapted from https://gist.github.com/jrusbatch/4211535
        public static int GetAvailablePort(int startingPort)
        {
            lock (_syncLock)
            {
                IPEndPoint[] endPoints;
                List<int> portArray = new List<int>();

                portArray.AddRange(_usedTestingPorts);

                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

                //getting active connections
                TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
                portArray.AddRange(from n in connections
                    where n.LocalEndPoint.Port >= startingPort
                    select n.LocalEndPoint.Port);

                //getting active tcp listners - WCF service listening in tcp
                endPoints = properties.GetActiveTcpListeners();
                portArray.AddRange(from n in endPoints
                    where n.Port >= startingPort
                    select n.Port);

                //getting active udp listeners
                endPoints = properties.GetActiveUdpListeners();
                portArray.AddRange(from n in endPoints
                    where n.Port >= startingPort
                    select n.Port);

                portArray.Sort();

                for (int i = startingPort; i < UInt16.MaxValue; i++)
                {
                    if (!portArray.Contains(i))
                    {
                        _usedTestingPorts.Add(i);
                        return i;
                    }
                }

                return 0;
            }
        }
    }
}
