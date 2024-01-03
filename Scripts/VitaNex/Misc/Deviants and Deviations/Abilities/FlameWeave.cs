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

using Server.Network;

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class FlameWeaveDeviantAbility : WaveDeviantAbility
	{
		public override string Name { get { return "Flame Weave"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Fire; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseWaveEffect CreateEffect(BaseDeviant deviant)
		{
			var dir = deviant.Direction & Direction.ValueMask;

			return new FireWaveEffect(deviant.Location, deviant.Map, dir, Math.Max(5, deviant.RangePerception / 2));
		}

		protected override void OnLocked(BaseDeviant deviant)
		{
			base.OnLocked(deviant);

			deviant.Yell("BURN!");
		}

		protected override void OnDamage(BaseDeviant deviant, Mobile target, ref int damage)
		{
			base.OnDamage(deviant, target, ref damage);

			if (Utility.RandomBool())
			{
				int x = 0, y = 0;

				Movement.Movement.Offset(deviant.GetDirectionTo(target), ref x, ref y);

				var loc = target.Clone3D(x, y, target.Map.GetTopZ(target.Clone2D(x, y)));

				if (target.Map.CanSpawnMobile(loc))
				{
					ScreenFX.LightFlash.Send(target);

					target.Location = loc;

					if (target.PlayDamagedAnimation())
					{
						target.PlayHurtSound();
					}
				}
			}
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (state.Deviant == null || state.Target == null)
			{
				return;
			}

			state.Target.SendMessage(state.Deviant.SpeechHue, "[{0}]: Burn!", state.Deviant.RawName);
			state.Target.Send(SpeedControl.WalkSpeed);
		}

		protected override void OnRemoved(State state)
		{
			base.OnRemoved(state);

			if (state.Target == null)
			{
				return;
			}

			state.Target.Send(SpeedControl.Disable);
			state.Target.SendMessage(85, "Your flaming tomb fades away.");
		}
	}
}
