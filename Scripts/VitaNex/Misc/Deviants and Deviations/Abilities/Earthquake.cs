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

using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class EarthquakeDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Earthquake"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Earth; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(20); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			if (deviant.PlayAttackAnimation())
			{
				deviant.PlayAttackSound();
			}

			foreach (var t in AcquireTargets<Mobile>(deviant))
			{
				ScreenFX.LightFlash.Send(t);

				t.PlaySound(1230);
				Damage(deviant, t);
				t.Paralyze(TimeSpan.FromSeconds(2.0));
			}
		}
	}
}
