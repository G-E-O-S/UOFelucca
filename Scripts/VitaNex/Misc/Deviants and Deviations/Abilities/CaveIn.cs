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
	public class CaveInDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Cave In"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Earth; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

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
				Timer.DelayCall(
					TimeSpan.FromMilliseconds(delay),
					loc =>
					{
						SpellHelper.Turn(deviant, loc);

						if (deviant.PlayAttackAnimation())
						{
							deviant.PlayAttackSound();
						}

						CaveInBoulder(deviant, loc);
					},
					deviant.Location.GetRandomPoint2D(range, range).GetSurfaceTop(deviant.Map));
			}
		}

		private void CaveInBoulder(BaseDeviant deviant, Point3D loc)
		{
			new MovingEffectInfo(loc.Clone3D(-4, 0, 15), loc.Clone3D(0, 0, 4), deviant.Map, 4534, 2999, 1).Send();
			new MovingEffectInfo(loc.Clone3D(0, 0, 60), loc.Clone3D(0, 0, 5), deviant.Map, 4534, 0, 4)
			{
				SoundID = 541
			}.MovingImpact(e => BoulderImpact(deviant, e.Target.Location, 3));
		}

		private void BoulderImpact(BaseDeviant deviant, Point3D loc, int blastRange)
		{
			if (deviant.Deleted || !deviant.Alive)
			{
				return;
			}

			new EarthExplodeEffect(loc, deviant.Map, blastRange)
			{
				AverageZ = false,
				EffectMutator = e => e.SoundID = 1231,
				EffectHandler = e => BoulderBlast(deviant, e)
			}.Send();
		}

		private void BoulderBlast(BaseDeviant deviant, EffectInfo e)
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
