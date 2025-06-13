using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public static class StringBuilderExtensions
    {
        private static readonly Stack<StringBuilder> StrBuilderPool = new ();

        public static StringBuilder Get()
        {
            lock (StrBuilderPool)
            {
                return StrBuilderPool.Count == 0 ? new StringBuilder() : StrBuilderPool.Pop();
            }
        }

        public static void Release(this StringBuilder sb)
        {
            if (sb == null) return;

            lock (StrBuilderPool)
            {
                sb.Clear();
                StrBuilderPool.Push(sb);
            }
        }
    }
}