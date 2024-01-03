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

using VitaNex;
#endregion

namespace Server.Mobiles
{
	public interface IDeviantSpawn : ISpawnable
	{
		BaseDeviant Deviant { get; }
	}

	public abstract class DeviantAbility
	{
		public sealed class State
		{
			public DeviantAbility Ability { get; private set; }

			public BaseDeviant Deviant { get; set; }
			public Mobile Target { get; set; }

			public bool Expires { get; set; }
			public DateTime Expire { get; set; }

			public bool TargetDeathPersist { get; set; }

			public bool IsValid
			{
				get
				{
					return Ability != null && Deviant != null && !Deviant.Deleted && Deviant.Alive && Target != null && !Target.Deleted &&
						   (Target.Alive || (TargetDeathPersist && (Target.Player || Target.IsDeadBondedPet)));
				}
			}

			public bool IsExpired { get { return CheckExpired(DateTime.UtcNow); } }

			public State(DeviantAbility ability, BaseDeviant deviant, Mobile target, TimeSpan duration)
			{
				Ability = ability;
				Deviant = deviant;
				Target = target;

				Expire = DateTime.UtcNow + duration;
			}

			public bool CheckExpired(DateTime utcNow)
			{
				return Expires && Expire < utcNow;
			}
		}

		private static PollTimer _Timer;

		public static DeviantAbility[] Abilities { get; private set; }

		public static Dictionary<DeviantAbility, Dictionary<Mobile, State>> States { get; private set; }

		static DeviantAbility()
		{
			Abilities =
				typeof(DeviantAbility).GetConstructableChildren()
									 .Select(t => t.CreateInstanceSafe<DeviantAbility>())
									 .Where(a => a != null)
									 .ToArray();

			States = Abilities.ToDictionary(a => a, a => new Dictionary<Mobile, State>());
		}

		public static void Configure()
		{
			if (_Timer == null)
			{
				_Timer = PollTimer.FromSeconds(1.0, DefragmentStates, States.Any, false);
			}
		}

		public static void Initialize()
		{
			if (_Timer != null)
			{
				_Timer.Start();
			}
		}

		public static void DefragmentStates()
		{
			var now = DateTime.UtcNow;

			foreach (var states in States.Values)
			{
				states.RemoveValueRange(
					s =>
					{
						if (s == null)
						{
							return true;
						}

						if (!s.IsValid || s.CheckExpired(now))
						{
							if (s.Ability != null)
							{
								s.Ability.OnRemoved(s);
							}

							return true;
						}

						return false;
					});

				states.RemoveKeyRange(m => m == null || m.Deleted);
			}
		}

		public static List<DeviantAbility> GetAbilities(BaseDeviant deviant, bool checkLock)
		{
			return Abilities.Where(a => a.CanInvoke(deviant)).ToList();
		}

		public static bool HasAbility<TAbility>(BaseDeviant deviant) where TAbility : DeviantAbility
		{
			return Abilities.OfType<TAbility>().Any(a => a.HasFlags(deviant));
		}

		public abstract string Name { get; }

		public abstract DeviationFlags Deviations { get; }

		public abstract TimeSpan Lockdown { get; }
		public abstract TimeSpan Cooldown { get; }

		public virtual TimeSpan Duration { get { return TimeSpan.Zero; } }

		public virtual double DamageFactor { get { return 0.5; } } // default 1

		public virtual bool Stackable { get { return false; } }

		public virtual bool MatchAnyDeviation { get { return true; } }

		public void Damage(BaseDeviant deviant, Mobile target)
		{
			deviant.DoHarmful(target, true);

			var damage = Utility.RandomMinMax(deviant.DamageMin, deviant.DamageMax);

			if (DamageFactor != 1.0)
			{
				damage = (int)Math.Ceiling(damage * DamageFactor);
			}

			if (damage > 0)
			{
				OnDamage(deviant, target, ref damage);
			}

			if (damage > 0)
			{
				target.Damage(damage, deviant);

				if (target.PlayDamagedAnimation())
				{
					target.PlayHurtSound();
				}
			}
		}

		protected virtual void OnDamage(BaseDeviant deviant, Mobile target, ref int damage)
		{ }

		protected virtual void OnAdded(State state)
		{ }

		protected virtual void OnRemoved(State state)
		{ }

		public bool HasFlags(BaseDeviant deviant)
		{
			if (deviant == null || deviant.Deviations == DeviationFlags.None || Deviations == DeviationFlags.None)
			{
				return false;
			}

			if (Deviations == deviant.Deviations)
			{
				return true;
			}

			return MatchAnyDeviation &&
				   Deviations.EnumerateValues<DeviationFlags>(true).Any(a => a != DeviationFlags.None && deviant.Deviations.GetFlag(a));
		}

		public bool CheckLock(BaseDeviant deviant, bool locked)
		{
			return deviant != null && (locked ? !deviant.CanBeginAction(this) : deviant.CanBeginAction(this));
		}

		public void SetLock(BaseDeviant deviant, bool locked)
		{
			if (deviant == null)
			{
				return;
			}

			if (locked)
			{
				deviant.BeginAction(this);
				OnLocked(deviant);
			}
			else
			{
				deviant.EndAction(this);
				OnUnlocked(deviant);
			}
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseDeviant deviant,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(deviant, deviant.Location, deviant.RangePerception, cache, filter);
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseDeviant deviant,
			Point3D p,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(deviant, p, deviant.RangePerception, cache, filter);
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseDeviant deviant,
			int range,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(deviant, deviant.Location, range, cache, filter);
		}

		protected virtual IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseDeviant deviant,
			Point3D p,
			int range,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			if (deviant == null || deviant.Deleted || deviant.Map == null || deviant.Map == Map.Internal)
			{
				yield break;
			}

			var targets = deviant.AcquireTargets<TMobile>(p, range);

			foreach (var t in (filter != null ? targets.Where(filter) : targets))
			{
				if (cache && Duration > TimeSpan.Zero)
				{
					SetTargetState(deviant, t, Duration);
				}

				yield return t;
			}
		}

		public State GetTargetState(Mobile m)
		{
			return States.GetValue(this).GetValue(m);
		}

		public void SetTargetState(BaseDeviant deviant, Mobile target, TimeSpan duration)
		{
			Dictionary<Mobile, State> states;

			if (!States.TryGetValue(this, out states) || states == null)
			{
				States[this] = states = new Dictionary<Mobile, State>();
			}

			State state;

			if (!states.TryGetValue(target, out state) || state == null)
			{
				states[target] = state = new State(this, deviant, target, duration);
			}
			else
			{
				OnRemoved(state);

				state.Deviant = deviant;
				state.Target = target;

				if (Stackable && !state.IsExpired)
				{
					state.Expire += duration;
				}
				else
				{
					state.Expire = DateTime.UtcNow.Add(duration);
				}
			}

			OnAdded(state);
		}

		public virtual bool CanInvoke(BaseDeviant deviant)
		{
			return deviant != null && !deviant.Deleted && deviant.Alive && !deviant.Blessed && //
				   deviant.InCombat(TimeSpan.Zero) && HasFlags(deviant) && CheckLock(deviant, false) && deviant.CanUseAbility(this);
		}

		public bool TryInvoke(BaseDeviant deviant)
		{
			if (CanInvoke(deviant))
			{
				return VitaNexCore.TryCatchGet(
					() =>
					{
						SetLock(deviant, true);

						OnInvoke(deviant);

						deviant.OnAbility(this);

						var locked = Lockdown.TotalSeconds;

						if (locked > 0)
						{
							locked -= deviant.Scale(locked * 0.10);
						}

						locked = Math.Max(0, locked);

						Timer.DelayCall(TimeSpan.FromSeconds(locked), a => SetLock(a, false), deviant);

						return true;
					},
					x => x.ToConsole(true));
			}

			return false;
		}

		protected abstract void OnInvoke(BaseDeviant deviant);

		protected virtual void OnLocked(BaseDeviant deviant)
		{ }

		protected virtual void OnUnlocked(BaseDeviant deviant)
		{ }
	}
}
