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
using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public abstract class WaveDeviantAbility : DeviantAbility
	{
		protected abstract BaseWaveEffect CreateEffect(BaseDeviant deviant);

		protected override void OnInvoke(BaseDeviant deviant)
		{
			var fx = CreateEffect(deviant);

			if (fx == null)
			{
				return;
			}

			fx.AverageZ = false;

			fx.EffectHandler = e =>
			{
				if (e.ProcessIndex != 0)
				{
					return;
				}

				foreach (var t in AcquireTargets<Mobile>(deviant, e.Source.Location, 0))
				{
					OnTargeted(deviant, t);
				}
			};

			fx.Send();
		}

		protected virtual void OnTargeted(BaseDeviant deviant, Mobile target)
		{
			Damage(deviant, target);
		}
	}
}
