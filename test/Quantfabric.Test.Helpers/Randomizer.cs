using System;
using System.Linq;

namespace Quantfabric.Test.Helpers
{
    public class Randomizer
    {
        private static readonly Random random = new Random();

        public int RandomNumber(int start, int end)
        {
            return random.Next(start, end);
        }

        public string RandomString(int minimum, int maximum)
        {
            return RandomString(RandomNumber(minimum, maximum));
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-.@/";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
