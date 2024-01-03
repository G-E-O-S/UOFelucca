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

using Server.Spells;

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class HailstormDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Hailstorm"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Frost; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(90); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(15); } }

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

			deviant.PlaySound(1230);

			var delay = 500;

			for (var range = 4; range <= deviant.RangePerception; range++, delay += 500)
			{
				var loc = deviant.Location.GetRandomPoint2D(range, range).GetSurfaceTop(deviant.Map);

				Timer.DelayCall(
					TimeSpan.FromMilliseconds(delay),
					() =>
					{
						SpellHelper.Turn(deviant, loc);

						if (deviant.PlayAttackAnimation())
						{
							deviant.PlayAttackSound();
						}

						Hailstorm(deviant, loc);
					});
			}
		}

		private void Hailstorm(BaseDeviant deviant, Point3D loc)
		{
			var effect = Utility.RandomMinMax(9006, 9007);

			new MovingEffectInfo(loc.Clone3D(-4, 0, 15), loc.Clone3D(0, 0, 4), deviant.Map, effect, 2999, 1).Send();
			new MovingEffectInfo(loc.Clone3D(0, 0, 60), loc.Clone3D(0, 0, 5), deviant.Map, effect, 0, 4)
			{
				SoundID = 247
			}.MovingImpact(e => HailstormImpact(deviant, e.Target.Location, 4));
		}

		private void HailstormImpact(BaseDeviant deviant, Point3D loc, int blastRange)
		{
			if (deviant.Deleted || !deviant.Alive)
			{
				return;
			}

			Effects.PlaySound(loc, deviant.Map, Utility.RandomBool() ? 910 : 912);

			new AirExplodeEffect(loc, deviant.Map, blastRange)
			{
				AverageZ = false,
				EffectMutator = e => e.SoundID = 21,
				EffectHandler = e => HailstormBlast(deviant, e)
			}.Send();
		}

		private void HailstormBlast(BaseDeviant deviant, EffectInfo e)
		{
			if (deviant.Deleted || !deviant.Alive || e.ProcessIndex != 0)
			{
				return;
			}

			foreach (var t in AcquireTargets<Mobile>(deviant, e.Source.Location, 0))
			{
				Damage(deviant, t);
			}
		}
	}
}
