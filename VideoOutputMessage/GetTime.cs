using System.Diagnostics;

namespace VideoOutputMessage
{
    public static class GetTime
    {
        public static string GetStartupTime()
        {
            var time = DateTime.Now - Process.GetCurrentProcess().StartTime;

            return $" (起動から {time.Hours} 時間 {time.Minutes} 分)";
        }
    }
}
