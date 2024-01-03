using System;
using System.Diagnostics;
using Server.Commands;
using Server.Mobiles;

namespace Server.Commands
{
    public class PingCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Ping", AccessLevel.Player, new CommandEventHandler(Ping_OnCommand));
        }

        [Usage("Ping")]
        [Description("Displays the player's current ping.")]
        private static void Ping_OnCommand(CommandEventArgs e)
        {
            Mobile mobile = e.Mobile;

            if (mobile == null || mobile.NetState == null)
            {
                e.Mobile.SendMessage("Unable to determine ping for an offline player.");
                return;
            }

            int ping = GetPing(mobile);

            if (ping >= 0)
            {
                e.Mobile.SendMessage($"Use 'Connection' in the manu bar to display ping");
                //e.Mobile.SendMessage($"Your current ping is: {ping} ms");
            }
            else
            {
                e.Mobile.SendMessage("Unable to determine ping at the moment.");
            }
        }

        private static int GetPing(Mobile mobile)
        {
            if (mobile == null || mobile.NetState == null)
                return -1;

            // Measure the time it takes to send a message and get a response
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();       

            stopwatch.Stop();

            return (int)stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
