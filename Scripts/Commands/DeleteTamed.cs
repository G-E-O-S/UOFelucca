using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Commands
{

    public class DeleteTamed
    {
        public static void Initialize()
        {
            CommandSystem.Register("DeleteTamed", AccessLevel.Owner, new CommandEventHandler(CleanSave_OnCommand));
        }
        [Usage("DeleteTamed")]
        [Description("Deletes all tamed creatures")]
        public static void CleanSave_OnCommand(CommandEventArgs e)
        {
            List<Mobile> tamed = new List<Mobile>();

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseCreature)
                {
                    BaseCreature bc = m as BaseCreature;
                    tamed.Add(bc);
                }
            }

            foreach (BaseCreature m in tamed)
            {
                if (m is BaseCreature && !m.Deleted)
                {
                    if (m.ControlMaster != null)
                    {
                        if (m.Controlled == true)
                        {
                            m.Delete();
                        }
                    }
                }
            }
        }
    }
}
