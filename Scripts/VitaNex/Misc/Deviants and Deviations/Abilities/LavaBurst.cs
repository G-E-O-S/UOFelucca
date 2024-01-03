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

using Server.Spells.Fourth;

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class LavaBurstDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Lava Burst"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Earth | DeviationFlags.Fire; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(45); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override double DamageFactor { get { return 0.5; } }

		public override bool CanInvoke(BaseDeviant deviant)
		{
			return base.CanInvoke(deviant) && deviant.Map.HasLand(deviant) && !deviant.Map.HasWater(deviant);
		}

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			var map = deviant.Map;
			var x = deviant.X;
			var y = deviant.Y;
			var z = deviant.Z;

			var count = Utility.RandomMinMax(5, 10);
			var sect = 360 / count;

			var shift = Utility.RandomMinMax(0, sect);
			var range = Math.Max(3, deviant.RangePerception - 3);

			for (var i = 0; i < count; i++)
			{
				var t = Angle.GetPoint3D(x, y, z, shift + (i * sect), range);
				var l = deviant.PlotLine3D(t).TakeWhile(p => map.HasLand(p) && !map.HasWater(p));

				var q = new EffectQueue(range);

				var c = -1;

				foreach (var p in l)
				{
					q.Add(
						new EffectInfo(p, map, 14089, 0, 8, 15, EffectRender.Darken)
						{
							QueueIndex = ++c
						});
				}

				if (q.Count == 0)
				{
					q.Dispose();
					continue;
				}

				q.Handler = fx =>
				{
					var isEnd = fx.QueueIndex >= c;

					if (!TryBurst(deviant, fx, ref isEnd) || isEnd)
					{
						q.Clear();
					}
				};

				q.Callback = q.Dispose;

				Timer.DelayCall(
					TimeSpan.FromSeconds(0.2 * i),
					() =>
					{
						deviant.Direction = deviant.GetDirection(t);

						if (deviant.PlayAttackAnimation())
						{
							deviant.PlayAttackSound();
						}

						q.Process();
					});
			}
		}

		protected virtual bool TryBurst(BaseDeviant deviant, EffectInfo e, ref bool isEnd)
		{
			if (deviant.Deleted || !deviant.Alive)
			{
				return false;
			}

			if (!isEnd)
			{
				var lf = TileData.LandTable[e.Map.GetLandTile(e.Source).ID].Flags;

				if (lf.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				var flags = e.Map.GetStaticTiles(e.Source).Select(t => TileData.ItemTable[t.ID].Flags);

				if (flags.Any(tf => tf.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall)))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				var flags = e.Source.FindItemsInRange(e.Map, 0).Select(o => TileData.ItemTable[o.ItemID].Flags);

				if (flags.Any(f => f.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall)))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				isEnd = AcquireTargets<Mobile>(deviant, 0, false).Any();
			}

			if (!isEnd)
			{
				new FireFieldSpell.FireFieldItem(6571, e.Source.Location, deviant, e.Map, TimeSpan.FromSeconds(5.0), 1)
				{
					Hue = e.Hue
				};

				return true;
			}

			new TornadoEffect(e.Source, e.Map, deviant.GetDirection(e.Source), 3)
			{
				Size = 3,
				Climb = 5,
				Height = 40,
				EffectMutator = efx =>
				{
					efx.Hue = e.Hue;
					efx.EffectID = 14027;
					efx.SoundID = 519;
				},
				EffectHandler = efx =>
				{
					if (efx.ProcessIndex != 0 || efx.Source.Z > e.Source.Z)
					{
						return;
					}

					foreach (var t in AcquireTargets<Mobile>(deviant, efx.Source.Location, 0, false))
					{
						Damage(deviant, t);
					}

					new FireFieldSpell.FireFieldItem(
						6571,
						efx.Source.Location,
						deviant,
						efx.Map,
						TimeSpan.FromSeconds(5.0),
						1)
					{
						Hue = efx.Hue
					};
				}
			}.Send();

			return true;
		}

		protected override void OnDamage(BaseDeviant deviant, Mobile target, ref int damage)
		{
			base.OnDamage(deviant, target, ref damage);

			using (var fx = new EffectInfo(target, target.Map, 14000, 0, 10, 30))
			{
				fx.SoundID = 519;
				fx.Send();
			}

			target.TryParalyze(TimeSpan.FromSeconds(1.0));
			target.Z = Math.Max(target.Z, Math.Min(deviant.Z + 40, target.Z + 5));
		}
	}
}
