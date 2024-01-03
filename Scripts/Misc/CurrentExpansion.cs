#region References
using System;

using Server.Accounting;
using Server.Network;
using Server.Services.TownCryer;
#endregion

namespace Server
{
	public class CurrentExpansion
	{
		public static readonly Expansion Expansion = Config.GetEnum("Expansion.CurrentExpansion", Expansion.LBR);

		[CallPriority(Int32.MinValue)]
		public static void Configure()
		{
			Core.Expansion = Expansion;

			AccountGold.Enabled = Core.LBR; //default TOL
			AccountGold.ConvertOnBank = true;
			AccountGold.ConvertOnTrade = false;
			VirtualCheck.UseEditGump = true; // default true
            
			TownCryerSystem.Enabled = Core.TOL;

			ObjectPropertyList.Enabled = Core.LBR;

            Mobile.InsuranceEnabled = Core.AOS && !Siege.SiegeShard;
			Mobile.VisibleDamageType = Core.LBR ? VisibleDamageType.Related : VisibleDamageType.None;
			Mobile.GuildClickMessage = !Core.AOS;
			Mobile.AsciiClickMessage = !Core.AOS;

			if (!Core.AOS)
			{
				return;
			}

			AOS.DisableStatInfluences();

			if (ObjectPropertyList.Enabled)
			{
				PacketHandlers.SingleClickProps = true; // single click for everything is overriden to check object property list
			}

			Mobile.ActionDelay = Core.LBR ? 100 : Core.AOS ? 500 : 250; // changed from core.TOL ? 500 / 1000 : 500
            Console.WriteLine("ActionDelay: " + Mobile.ActionDelay);
            Mobile.AOSStatusHandler = AOS.GetStatus;
		}
	}
}
