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

using Server.Misc;
using Server.Spells;

using VitaNex.Collections;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class DeathRayDeviantAbility : DeviantAbility
	{
		private static readonly TileFlag[] _BlockingFlags = 
		{ 
			TileFlag.Impassable, TileFlag.Wall, TileFlag.Roof, TileFlag.Door,
		};

		public override string Name { get { return "Death Ray"; } }

		public override DeviationFlags Deviations
		{
			get { return DeviationFlags.Tech | DeviationFlags.Despair | DeviationFlags.Darkness | DeviationFlags.Chaos; }
		}

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(20); } }

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			var list = ListPool<EffectQueue>.AcquireObject();
			var cw = Utility.RandomBool();

			for (int a = (cw ? 0 : 360), h = 0; (cw ? a <= 360 : a >= 0); a += (cw ? 1 : -1))
			{
				var x = (int)Math.Round(deviant.X + (deviant.RangePerception * Math.Sin(Geometry.DegreesToRadians(a))));
				var y = (int)Math.Round(deviant.Y + (deviant.RangePerception * Math.Cos(Geometry.DegreesToRadians(a))));

				if (((x * 397) ^ y) == h)
				{
					// This location was just handled, ignore it to avoid small increments
					continue;
				}

				h = ((x * 397) ^ y);

				var start = deviant.Clone3D(0, 0, 10);
				var end = new Point3D(x, y, deviant.Z);

				end.Z = end.GetTopZ(deviant.Map);

				var l = start.GetLine3D(end, deviant.Map, false);

				var q = new EffectQueue
				{
					Deferred = false,
					Handler = e => HandleDeathRay(deviant, e)
				};

				for (var i = 0; i < l.Length; i++)
				{
					var p = new Block3D(l[i], 5);
					var blocked = i + 1 >= l.Length;

					if (!blocked)
					{
						var land = deviant.Map.GetLandTile(p);

						if (p.Intersects(land.Z, land.Height))
						{
							var o = TileData.LandTable[land.ID];

							if (o.Flags.AnyFlags(_BlockingFlags))
							{
								blocked = true;
							}
						}
					}

					if (!blocked)
					{
						var tiles = deviant.Map.GetStaticTiles(p);

						var data = tiles.Where(o => p.Intersects(o.Z, o.Height)).Select(t => TileData.ItemTable[t.ID]);

						if (data.Any(o => o.Flags.AnyFlags(_BlockingFlags)))
						{
							blocked = true;
						}
					}

					if (!blocked)
					{
						var items = p.FindItemsAt(deviant.Map);

						var data = items.Where(p.Intersects).Select(o => TileData.ItemTable[o.ItemID]);

						if (data.Any(o => o.Flags.AnyFlags(_BlockingFlags)))
						{
							blocked = true;
						}
					}

					var effect = blocked ? 14120 : Utility.RandomMinMax(12320, 12324);
					var hue = blocked ? 0 : 2075;

					if (blocked)
					{
						p = p.Clone3D(0, 0, -8);
					}

					q.Add(
						new EffectInfo(p, deviant.Map, effect, hue, 10, 10, EffectRender.Darken)
						{
							QueueIndex = i
						});

					if (blocked)
					{
						break;
					}
				}

				if (q.Queue.Count > 0)
				{
					list.Add(q);
				}
			}

			if (list.Count == 0)
			{
				ObjectPool.Free(list);

				return;
			}

			for (var i = 0; i < list.Count; i++)
			{
				var cur = list[i];

				if (i + 1 < list.Count)
				{
					var next = list[i + 1];

					cur.Callback = () =>
					{
						if (deviant.Deleted || !deviant.Alive)
						{
							list.ForEach(q => q.Dispose());

							ObjectPool.Free(list);

							return;
						}

						if (next.Queue.Count > 0)
						{
							SpellHelper.Turn(deviant, next.Queue.Last().Source);
						}

						Timer.DelayCall(TimeSpan.FromSeconds(0.05), next.Process);
					};
				}
				else
				{
					cur.Callback = () =>
					{
						list.ForEach(q => q.Dispose());

						ObjectPool.Free(list);

						deviant.CantWalk = false;
						deviant.LockDirection = false;
					};
				}
			}

			if (list.Count == 0)
			{
				ObjectPool.Free(list);

				return;
			}

			deviant.CantWalk = true;
			deviant.LockDirection = true;

			if (list[0].Queue.Count > 0)
			{
				SpellHelper.Turn(deviant, list[0].Queue.Last().Source);
			}

			Timer.DelayCall(TimeSpan.FromSeconds(0.1), list[0].Process);
		}

		private void HandleDeathRay(BaseDeviant deviant, EffectInfo e)
		{
			if (deviant.Deleted || !deviant.Alive || e.ProcessIndex != 0)
			{
				return;
			}

			foreach (var t in AcquireTargets<Mobile>(deviant, e.Source.Location, 0))
			{
				Effects.SendBoltEffect(t, true, e.Hue);

				Damage(deviant, t);
			}
		}
	}
}
