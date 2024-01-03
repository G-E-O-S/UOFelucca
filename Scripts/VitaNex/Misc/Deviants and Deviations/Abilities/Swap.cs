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

using VitaNex.Collections;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class SwapDeviantAbility : DeviantAbility
	{
		public override string Name { get { return "Swap"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Time | DeviationFlags.Chaos | DeviationFlags.Tech | DeviationFlags.Illusion; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(15); } }

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

			var targets = ListPool<Mobile>.AcquireObject();

			targets.AddRange(AcquireTargets<Mobile>(deviant));

			if (targets.Count > 0)
			{
				using (var fx = new EffectInfo(deviant, deviant.Map, 14120, 0, 10, 10, EffectRender.Lighten))
				{
					fx.SoundID = 510;

					Mobile t;

					var i = targets.Count;

					foreach (var p in targets.Select(o => o.Location))
					{
						t = targets[--i];

						fx.SetSource(t);

						fx.Send();
						t.Location = p;
						fx.Send();
					}

					var l = deviant.Location;

					t = targets.GetRandom();

					fx.SetSource(deviant);

					fx.Send();
					deviant.Location = t.Location;
					fx.Send();

					t.Location = l;
				}
			}

			ObjectPool.Free(targets);
		}

		protected override void OnLocked(BaseDeviant deviant)
		{
			base.OnLocked(deviant);

			deviant.Yell("TIME FOR A SWITCH!");
		}
	}
}
