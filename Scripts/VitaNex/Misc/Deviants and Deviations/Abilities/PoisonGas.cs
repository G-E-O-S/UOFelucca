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

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class PoisonGasDeviantAbility : ExplosionDeviantAbility
	{
		public override string Name { get { return "Poison Gas"; } }

		public override DeviationFlags Deviations
		{
			get
			{
				return DeviationFlags.Elements | DeviationFlags.Death | DeviationFlags.Famine | DeviationFlags.Decay | DeviationFlags.Poison;
			}
		}

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseDeviant deviant)
		{
			return new PoisonExplodeEffect(deviant.Location, deviant.Map, Math.Max(5, deviant.RangePerception / 2));
		}

		protected override void OnDamage(BaseDeviant deviant, Mobile target, ref int damage)
		{
			base.OnDamage(deviant, target, ref damage);

			if (target.ApplyPoison(deviant, Poison.Lethal) != ApplyPoisonResult.Poisoned)
			{
				damage *= 2;
			}
		}
	}
}
