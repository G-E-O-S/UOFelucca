#region Header
//               _,-'/-'/
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2023  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #                                       #
#endregion

#region References
using System;
#endregion

namespace Server.Mobiles
{
	public class ReflectSpellDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Reflect Spells"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.All; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		public override bool CanInvoke(BaseDeviant deviant)
		{
			return base.CanInvoke(deviant) && !deviant.ReflectSpell && !deviant.ReflectMelee;
		}

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			deviant.ReflectSpell = true;

			Timer.DelayCall(Duration, a => a.ReflectSpell = false, deviant);
		}
	}
}
