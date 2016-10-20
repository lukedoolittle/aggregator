using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Quantfabric.Test.Integration
{
    public static class TestUtilities
    {
        private static readonly object _syncLock = new object();

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
