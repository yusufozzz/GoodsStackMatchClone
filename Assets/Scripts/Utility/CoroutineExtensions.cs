using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class CoroutineExtensions
    {
        private static readonly Dictionary<float, WaitForSeconds> SWaitDictionary = new ();

        public static WaitForSeconds WaitForSeconds(float time)
        {
            if (SWaitDictionary.TryGetValue(time, out var wait)) return wait;
            SWaitDictionary[time] = new WaitForSeconds(time);
            return SWaitDictionary[time];
        }
    }
}