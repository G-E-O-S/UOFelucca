using Server.Commands;
using Server.Items;
using Server.Multis;
using System;

namespace Server.Commands
{
    public class LeaveHouse
    {
        public static void Initialize()
        {
            CommandSystem.Register("LeaveHouse", AccessLevel.Player, new CommandEventHandler(Leave_OnCommand));
        }

        [Usage("LeaveHouse")]
        [Description("Moves player to house ban location.")]
        private static void Leave_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            BaseHouse house = BaseHouse.FindHouseAt(from);

            if (house != null)
            {
                from.Location = house.BanLocation;
                from.SendMessage("You have left the house.");
            }
            else
            {
                from.SendMessage("You are not in a house.");
            }
        }
    }
}
