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
using System.Linq;

using Server.Spells;

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class BoulderTossDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Boulder Toss"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Earth; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(20); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(10); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(5); } }

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			var t =
				AcquireTargets<Mobile>(deviant).Where(m => deviant.GetDistanceToSqrt(m) >= deviant.RangePerception * 0.25).GetRandom();

			if (t == null || t.Deleted || !t.Alive)
			{
				return;
			}

			deviant.CantWalk = true;

			SpellHelper.Turn(deviant, t);

			int x = 0, y = 0;

			Movement.Movement.Offset(deviant.Direction & Direction.Mask, ref x, ref y);

			var loc = deviant.Location.Clone3D(x, y, 5);

			if (deviant.PlayAttackAnimation())
			{
				deviant.PlayAttackSound();
			}

			new MovingEffectInfo(loc, t.Location, deviant.Map, 4534)
			{
				SoundID = 541
			}.MovingImpact(
				e =>
				{
					BoulderImpact(deviant, e.Target.Location, 4);
					deviant.CantWalk = false;
				});
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
