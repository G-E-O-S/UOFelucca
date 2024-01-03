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

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class VoidSpawnDeviantAbility : SpawnDeviantAbility
	{
		public override string Name { get { return "Void Spawn"; } }

		public override DeviationFlags Deviations
		{
			get { return DeviationFlags.Elements | DeviationFlags.Frost | DeviationFlags.Darkness; }
		}

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(45); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		protected override IDeviantSpawn CreateSpawn(BaseDeviant deviant)
		{
			return new Void(deviant);
		}

		private class Void : DeviantSpawn
		{
			private AirExplodeEffect _Aura;
			private long _NextAura;

			public Void(BaseDeviant deviant)
				: base(deviant, AIType.AI_Mage, FightMode.None, 0.2, 0.4)
			{
				Name = "Void";
				Body = 261;

				CantWalk = true;
			}

			public Void(Serial serial)
				: base(serial)
			{ }

			public override void OnThink()
			{
				base.OnThink();

				if (Deleted || !Alive || !Deviant.InCombat())
				{
					return;
				}

				if (!this.InCombat() && Utility.RandomDouble() < 0.01)
				{
					var t = Deviant.AcquireRandomTarget(Deviant.RangePerception);

					if (t != null)
					{
						if (this.PlayAttackAnimation())
						{
							this.PlayAttackSound();
						}

						MoveToWorld(t.Location, t.Map);
					}
				}

				if (_Aura == null)
				{
					_Aura = new AirExplodeEffect(Location, Map, 2)
					{
						AverageZ = false,
						Interval = TimeSpan.FromMilliseconds(500.0),
						EffectMutator = e =>
						{
							if (e.ProcessIndex == 0 && Utility.RandomDouble() < 0.33)
							{
								e.SoundID = Utility.RandomMinMax(20, 22);
							}

							e.Hue = Hue;
						},
						EffectHandler = HandleAura,
						Callback = () =>
						{
							if (this.PlayAttackAnimation())
							{
								this.PlayAttackSound();
							}
						}
					};
				}

				if (_Aura.Sending || Core.TickCount < _NextAura)
				{
					return;
				}

				_NextAura = Core.TickCount + 5000;

				_Aura.Start = Location;
				_Aura.Map = Map;
				_Aura.Send();
			}

			private void HandleAura(EffectInfo e)
			{
				if (Deleted || !Alive || e.ProcessIndex != 0)
				{
					return;
				}

				foreach (var t in Deviant.AcquireTargets(e.Source.Location, 0))
				{
					if (!t.Frozen)
					{
						Damage(10, this);

						t.TryFreeze(TimeSpan.FromSeconds(3.0));
					}

					t.PlaySound(912);
				}
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (_Aura != null)
				{
					_Aura.Clear();
					_Aura = null;
				}
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.SetVersion(0);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				reader.GetVersion();
			}
		}
	}
}
