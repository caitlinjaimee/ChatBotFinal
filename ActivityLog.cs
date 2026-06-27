using System;
using System.Collections.Generic;

namespace ChatbotWebForm
{
    public static class ActivityLog
    {
        private static List<string> log = new List<string>();

        public static void Add(string action)
        {
            log.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + action);
        }

        public static List<string> GetLast(int count = 10)
        {
            int start = Math.Max(0, log.Count - count);
            return log.GetRange(start, log.Count - start);
        }
    }
}
