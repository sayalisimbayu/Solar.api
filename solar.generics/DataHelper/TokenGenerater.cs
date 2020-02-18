using System;
using System.Collections.Generic;
using System.Text;

namespace solar.generics.DataHelper
{
    public static class TokenGenerater
    {
        public static string generateToken(int digitCount)
        {
            return String.Join("", randomPermutation(0, digitCount));
        }
        public static IList<int> randomPermutation(int min, int max)
        {
            Random r = new Random();
            SortedList<int, int> perm = new SortedList<int, int>();
            for (int i = min; i <= max; i++)
            {
                var nextVal = r.Next();
                if (!perm.ContainsKey(nextVal))
                    perm.Add(nextVal, i);
            }
            return perm.Values;
        }
        public static string RepeatForLoop(this string s, int n)
        {
            var result = s;
            for (var i = 0; i < n - 1; i++)
            {
                result += s;
            }
            return result;
        }
    }
}
