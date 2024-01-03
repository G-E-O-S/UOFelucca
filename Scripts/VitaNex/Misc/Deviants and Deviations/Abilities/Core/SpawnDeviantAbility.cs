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
using System.Collections.Generic;
using System.Linq;

using Server.Spells;

using VitaNex.Collections;
#endregion

namespace Server.Mobiles
{
	public abstract class SpawnDeviantAbility : DeviantAbility
	{
		public static Dictionary<BaseDeviant, List<IDeviantSpawn>> Spawn { get; private set; }

		static SpawnDeviantAbility()
		{
			Spawn = new Dictionary<BaseDeviant, List<IDeviantSpawn>>();
		}

		public static void Register(IDeviantSpawn spawn)
		{
			if (spawn == null || spawn.Deviant == null)
			{
				return;
			}

			List<IDeviantSpawn> list;

			if (!Spawn.TryGetValue(spawn.Deviant, out list) || list == null)
			{
				Spawn[spawn.Deviant] = list = ListPool<IDeviantSpawn>.AcquireObject();
			}

			list.Update(spawn);
		}

		public static void Unregister(IDeviantSpawn spawn)
		{
			if (spawn == null || spawn.Deviant == null)
			{
				return;
			}

			List<IDeviantSpawn> list;

			if (!Spawn.TryGetValue(spawn.Deviant, out list))
			{
				return;
			}

			if (list == null)
			{
				Spawn.Remove(spawn.Deviant);
				return;
			}

			list.Remove(spawn);

			if (list.Count == 0)
			{
				Spawn.Remove(spawn.Deviant);

				ObjectPool.Free(list);
			}
		}

		public virtual int SpawnLimit { get { return 5; } }

		protected abstract IDeviantSpawn CreateSpawn(BaseDeviant deviant);

		public override bool CanInvoke(BaseDeviant deviant)
		{
			if (!base.CanInvoke(deviant))
			{
				return false;
			}

			List<IDeviantSpawn> spawn;

			return !Spawn.TryGetValue(deviant, out spawn) || spawn == null || spawn.Count < SpawnLimit;
		}

		protected override void OnInvoke(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deleted)
			{
				return;
			}

			Point3D loc;
			var tries = 30;

			do
			{
				loc = deviant.GetRandomPoint3D(4, 8, deviant.Map, true, true);
			}
			while (loc.FindEntitiesInRange<IDeviantSpawn>(deviant.Map, 8).Any() && --tries >= 0);

			if (tries < 0)
			{
				return;
			}

			var s = CreateSpawn(deviant);

			if (s == null)
			{
				return;
			}

			Register(s);

			if (deviant.PlayAttackAnimation())
			{
				deviant.PlayAttackSound();

				deviant.TryParalyze(
					TimeSpan.FromSeconds(1.5),
					m =>
					{
						SpellHelper.Turn(m, loc);

						s.OnBeforeSpawn(loc, m.Map);
						s.MoveToWorld(loc, m.Map);
						s.OnAfterSpawn();
					});
			}
			else
			{
				SpellHelper.Turn(deviant, loc);

				s.OnBeforeSpawn(loc, deviant.Map);
				s.MoveToWorld(loc, deviant.Map);
				s.OnAfterSpawn();
			}
		}
	}
}
