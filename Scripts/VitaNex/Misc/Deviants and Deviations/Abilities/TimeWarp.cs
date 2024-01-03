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
#endregion

namespace Server.Mobiles
{
	public class TimeWarpDeviantAbility : ExplosionDeviantAbility
	{
		public override string Name { get { return "Time Warp"; } }

		public override DeviationFlags Deviations { get { return DeviationFlags.Time | DeviationFlags.Illusion; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseDeviant deviant)
		{
			return new EnergyExplodeEffect(deviant.Location, deviant.Map, Math.Max(5, deviant.RangePerception / 2));
		}

		protected override void OnLocked(BaseDeviant deviant)
		{
			base.OnLocked(deviant);

			deviant.Yell("TIME... IS RELATIVE!");
		}

		protected override void OnUnlocked(BaseDeviant deviant)
		{
			base.OnUnlocked(deviant);

			deviant.Yell("I SEE THE FUTURE. THERE IS NO FUTURE!");
		}

		protected override void OnDamage(BaseDeviant deviant, Mobile target, ref int damage)
		{
			base.OnDamage(deviant, target, ref damage);

			Effects.SendBoltEffect(target, true, deviant.Hue);
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (state.Deviant == null || state.Target == null)
			{
				return;
			}

			state.Target.SendMessage(state.Deviant.SpeechHue, "[{0}]: Time is just an illusion... Let me show you.", state.Deviant.RawName);
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
			state.Target.SendMessage(85, "You escape the influence of the time warp.");
		}
	}
}
