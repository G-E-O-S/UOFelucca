using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class HueCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Hue", AccessLevel.Player, new CommandEventHandler(Hue_OnCommand));
        }

        [Usage("Hue")]
        [Description("Displays the HUE of the targeted item.")]
        private static void Hue_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target an item to get its HUE.");
            e.Mobile.Target = new HueTarget();
        }

        private class HueTarget : Target
        {
            public HueTarget() : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    from.SendMessage($"The HUE of the targeted item is: {item.Hue}");
                }
                else
                {
                    from.SendMessage("Invalid target. Target an item to get its HUE.");
                }
            }
        }
    }
}
