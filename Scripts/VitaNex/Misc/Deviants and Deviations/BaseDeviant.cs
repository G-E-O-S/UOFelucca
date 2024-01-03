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

using Server.ContextMenus;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;
using Server.Spells;

using VitaNex;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	[CorpseName("decaying remains of an exodus champion")]
	public abstract class BaseDeviant : BaseCreature
	{
		private static readonly SkillName[] _InitialSkills = ((SkillName)0).GetValues<SkillName>();

		public static readonly int MinDeviantLevel = (int)DeviantLevel.Normal;
		public static readonly int MaxDeviantLevel = (int)DeviantLevel.Insane;

		public static Type[] DeviantTypes { get; private set; }

		public static List<BaseDeviant> Instances { get; private set; }

		static BaseDeviant()
		{
			Instances = new List<BaseDeviant>();

			DeviantTypes = typeof(BaseDeviant).GetConstructableChildren();
		}

		public static BaseDeviant CreateRandomInstance()
		{
			return CreateInstance(DeviantTypes.GetRandom());
		}

		public static BaseDeviant CreateInstance(Type t)
		{
			return t != null && t.IsChildOf<BaseDeviant>() ? t.CreateInstanceSafe<BaseDeviant>() : null;
		}

		public static TDeviant CreateInstance<TDeviant>() where TDeviant : BaseDeviant
		{
			return typeof(TDeviant).CreateInstanceSafe<TDeviant>();
		}

		private DateTime _NextAbility = DateTime.UtcNow;

		public override FoodType FavoriteFood { get { return FoodType.Gold; } }

		public override bool IgnoreYoungProtection { get { return true; } }

		public override bool CanBeParagon { get { return false; } }
     
        public override bool CanRummageCorpses { get { return false; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool CanFlee { get { return false; } }

        public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool BardImmune { get { return false; } } // default true
		public override bool Unprovokable { get { return false; } } // default true
		public override bool Uncalmable { get { return false; } } // default true
		public override bool ShowFameTitle { get { return false; } }

		//public override int TreasureMapLevel { get { return 10; } }
		//public override double TreasureMapChance { get { return 0.50 + ((int)DeviantLevel * 0.10); } }

		public virtual SkillName[] InitialSkills { get { return _InitialSkills; } }

		public virtual bool HealFromPoison { get { return Enraged; } }

		private DeviantLevel _Level;

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public DeviantLevel DeviantLevel
		{
			get { return _Level; }
			set
			{
				_Level = value;

				InitLevel();

				InvalidateProperties();
			}
		}

		public virtual DeviantLevel DefaultLevel { get { return DeviantLevel.Normal; } }

		private DeviationAttributes _Deviations;

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public DeviationAttributes Deviations
		{
			get { return _Deviations ?? (_Deviations = new DeviationAttributes(this)); }
			set { _Deviations = value ?? new DeviationAttributes(this); }
		}

		public virtual DeviationFlags DefaultDeviations { get { return DeviationFlags.All; } } // default DeviationFlags.None;

		public virtual double EnrageThreshold { get { return 0.01; } } // changed from 0.05

		// Default: 10% Increase to all stats
		private readonly StatBuffInfo _EnrageStatBuff = new StatBuffInfo(StatType.All, "Enraged", 10, TimeSpan.Zero);

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public StatType EnrageBuffType
		{
			get { return _EnrageStatBuff.Type; }
			set
			{
				RemoveStatMod(_EnrageStatBuff.Name);

				_EnrageStatBuff.Type = value;
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public string EnrageBuffName
		{
			get { return _EnrageStatBuff.Name; }
			set
			{
				RemoveStatMod(_EnrageStatBuff.Name);

				_EnrageStatBuff.Name = value;
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public int EnrageBuffOffset
		{
			get { return _EnrageStatBuff.Offset; }
			set
			{
				RemoveStatMod(_EnrageStatBuff.Name);

				_EnrageStatBuff.Offset = value;
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public TimeSpan EnrageBuffDuration
		{
			get { return _EnrageStatBuff.Duration; }
			set
			{
				RemoveStatMod(_EnrageStatBuff.Name);

				_EnrageStatBuff.Duration = value;
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public bool Enraged { get { return Hits / (double)HitsMax <= EnrageThreshold; } }

		private long _NextEnrage;
		private long _NextEffect;

		private bool _ReflectMelee;
		private bool _ReflectSpell;

		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool ReflectMelee
		{
			get { return _ReflectMelee; }
			set
			{
				_ReflectMelee = value;

				switch (SolidHueOverride)
				{
					case -1:
					{
						if (value)
						{
							SolidHueOverride = 51;
						}
					}
					break;
					case 51:
					{
						if (!value)
						{
							SolidHueOverride = -1;
						}
					}
					break;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool ReflectSpell
		{
			get { return _ReflectSpell; }
			set
			{
				_ReflectSpell = value;

				switch (SolidHueOverride)
				{
					case -1:
					{
						if (value)
						{
							SolidHueOverride = 62;
						}
					}
					break;
					case 62:
					{
						if (!value)
						{
							SolidHueOverride = -1;
						}
					}
					break;
				}
			}
		}

		public BaseDeviant(AIType aiType, FightMode mode, int perception, int rangeFight, double activeSpeed, double passiveSpeed)
			: base(aiType, mode, perception, rangeFight, activeSpeed, passiveSpeed)
		{
			_Deviations = new DeviationAttributes(this);
			_Level = DefaultLevel;

            AI = AIType.AI_Melee;

			Female = Utility.RandomBool();

			Title = InitTitle();

			Body = InitBody();

            HairItemID = (8252);
            HairHue = (1194);

            Item shroud = new HoodedShroudOfShadows();

            shroud.Movable = false;

            AddItem(shroud);

            Item boots = new Boots();

            shroud.Movable = false;

            AddItem(boots);

            Fame = 50000;
			Karma = -50000;

			SpeechHue = YellHue = 34;

			SetDamageType(ResistanceType.Physical, 10); // default 20
			SetDamageType(ResistanceType.Fire, 10); // default 20
            SetDamageType(ResistanceType.Cold, 10); // default 20
            SetDamageType(ResistanceType.Poison, 10); // default 20
            SetDamageType(ResistanceType.Energy, 10); // default 20

            SetResistance(ResistanceType.Physical, 10, 25); // default 25, 50
			SetResistance(ResistanceType.Fire, 10, 25); // default 25, 50
            SetResistance(ResistanceType.Cold, 10, 25); // default 25, 50
            SetResistance(ResistanceType.Energy, 10, 25); // default 25, 50
            SetResistance(ResistanceType.Poison, 10, 25); // default 25, 50

            InitLevel();

			var pack = Backpack;
           
			if (pack != null)
			{
				pack.Delete();
			}

			AddItem(new BottomlessBackpack());

			PackItems();
			EquipItems();

			Instances.Add(this);
		}

		public BaseDeviant(Serial serial)
			: base(serial)
		{
			Instances.Add(this);
		}

		public virtual void DeviationChanged(DeviationFlags a, bool state)
		{
			Title = InitTitle();

			InvalidateProperties();
		}

		protected virtual void InitLevel()
		{
			Scale(this);
		}

		public virtual double GetLevelFactor()
		{
			return 1.0 + (4.0 * ((int)DeviantLevel / 4.0));
		}

		public void Scale(BaseCreature c)
		{
			if (c == null)
			{
				return;
			}

			if (c is BaseDeviant && c != this)
			{
				return;
			}

			var factor = GetLevelFactor();

			if (c is IDeviantSpawn asp)
			{
				if (asp.Deviant != this)
				{
					return;
				}

				factor *= 0.05; // default 0.10
			}
			else if (c.Team != Team)
			{
				return;
			}

			c.VirtualArmor = Math.Min(60, 30 + Scale(10, factor)); // default 90, 40 + scale(10

			c.SetStr(Scale(250, factor), Scale(850, factor)); // default (200, factor), Scale(250
			c.SetDex(Scale(100, factor), Scale(175, factor)); // default (200, factor), Scale(250
            c.SetInt(Scale(100, factor), Scale(175, factor)); // default (200, factor), Scale(250

            c.SetHits(Scale(12499, factor), Scale(15000, factor)); // default (50000, factor), Scale(75000
			c.SetStam(Scale(100, factor), Scale(250, factor)); // default (1000, factor), Scale(2500
            c.SetMana(Scale(100, factor), Scale(250, factor)); // default (1000, factor), Scale(2500

            var damage = Scale(10, factor); // default (10,

			c.SetDamage(Math.Min(60, 10 + damage), Math.Min(60, 15 + damage)); // default (80, 15 + damage), Math.Min(90, 30

			var skill = Math.Min(100.0, 60.0 + Scale(3, factor)); // default (120.0, 95.0 + Scale(5.0,

			c.SetAllSkills(skill, Math.Max(100, skill)); // default (100,
		}

		public int Scale(int value)
		{
			return Scale(value, GetLevelFactor());
		}

		public virtual int Scale(int value, double factor)
		{
			return (int)Math.Ceiling(value * factor);
		}

		public double Scale(double value)
		{
			return Scale(value, GetLevelFactor());
		}

		public virtual double Scale(double value, double factor)
		{
			return Math.Ceiling(value * factor);
		}

		protected abstract int InitBody();

		protected virtual string InitTitle()
		{
			var title = "the Exodus Champion"; // default the Deviant
                           
			//if (Deviations.IsEmpty)
			//{
			//	return title;
			//}

			//if (Deviations.All)
			//{
			//	title += " of Infinity";
			//}

			return title;
		}

		protected virtual void PackItems()
		{ }

		protected virtual void EquipItems()
		{ }

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if (damage > 0 && from != null && from != this && from.Player && ReflectMelee)
			{
				damage /= 10;

				from.Damage(damage, this);
			}

			base.AlterMeleeDamageFrom(from, ref damage);
		}

		public override void AlterSpellDamageFrom(Mobile from, ref int damage)
		{
			if (damage > 0 && from != null && from != this && from.Player && ReflectSpell)
			{
				damage /= 10;

				from.Damage(damage, this);
			}

			base.AlterSpellDamageFrom(from, ref damage);
		}

		public override int Damage(int amount, Mobile m, bool informMount)
		{
			// Poison will cause all damage to heal instead.
			if (HealFromPoison && Poison != null)
			{
				Hits += amount;

				if (Utility.RandomDouble() < 0.10)
				{
					var message = $"*{Name} {Utility.RandomList("looks healthy", "looks stronger", "is absorbing damage", "is healing")}*";

					NonlocalOverheadMessage(MessageType.Regular, 0x21, false, message);
				}

				return 0;
			}
			else
			{
				return base.Damage(amount, m, informMount);
			}
		}

		public override void OnPoisoned(Mobile m, Poison poison, Poison oldPoison)
		{
			if (HealFromPoison)
			{
				NonlocalOverheadMessage(MessageType.Regular, 0x21, false, "*The poison seems to have the opposite effect*");
				return;
			}

			base.OnPoisoned(m, poison, oldPoison);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			list.RemoveAll(e => e is PaperdollEntry);
		}

		public override void OnThink()
		{
			base.OnThink();

			if (Deleted || Map == null || Map == Map.Internal || !this.InCombat())
			{
				return;
			}

			if ((ReflectMelee || ReflectSpell) && Core.TickCount > _NextEffect)
			{
				_NextEffect = Core.TickCount + 1000;
				new EffectInfo(this.Clone3D(0, 0, 20), Map, 14120, 0x33, 10, 30, EffectRender.SemiTransparent).Send();
			}

			if (EnrageThreshold > 0 && Enraged && Core.TickCount > _NextEnrage)
			{
				_NextEnrage = Core.TickCount + 10000;

				var buff = _EnrageStatBuff;

				if (buff.Offset != 0 && !string.IsNullOrWhiteSpace(buff.Name))
				{
					var old = GetStatMod(buff.Name);

					if (old == null)
					{
						var offset = buff.Offset / 10.0; // default 100.0

						var s = buff.Type == StatType.All || buff.Type == StatType.Str;
						var d = buff.Type == StatType.All || buff.Type == StatType.Dex;
						var i = buff.Type == StatType.All || buff.Type == StatType.Int;

						if (s || d || i)
						{
							var sv = RawStr * (s ? offset : 0);
							var dv = RawDex * (d ? offset : 0);
							var iv = RawInt * (i ? offset : 0);

							var bonus = (int)Math.Ceiling((sv + dv + iv) / ((s ? 1 : 0) + (d ? 1 : 0) + (i ? 1 : 0)));

							if (bonus != 0)
							{
								var clone = buff.Clone();

								if (clone != null)
								{
									clone.Offset = bonus;

									AddStatMod(clone);
								}
							}
						}
					}
				}
			}

			var now = DateTime.UtcNow;

			if (Deviations.IsEmpty)
			{
				_NextAbility = now.AddSeconds(1.0);
				return;
			}

			if (!this.InCombat(TimeSpan.Zero) || now <= _NextAbility)
			{
				return;
			}

			var ability = GetRandomAbility(true);

			if (ability == null || !ability.TryInvoke(this))
			{
				_NextAbility = now.AddSeconds(1.0);
				return;
			}

			var cooldown = GetAbilityCooldown(ability);

			_NextAbility = cooldown > TimeSpan.Zero ? now.Add(cooldown) : now;
		}

		protected virtual DeviantAbility GetRandomAbility(bool checkLock)
		{
			var abilities = GetAbilities(checkLock);

			var a = abilities.GetRandom();

			abilities.Free(true);

			return a;
		}

		protected virtual TimeSpan GetAbilityCooldown(DeviantAbility ability)
		{
			if (ability != null)
			{
				var cool = ability.Cooldown.TotalSeconds;

				if (cool > 0)
				{
					cool -= Scale(cool * 0.10);
					cool = Math.Max(0, cool);
				}

				if (cool > 0)
				{
					return TimeSpan.FromSeconds(cool);
				}
			}

			return TimeSpan.Zero;
		}

		public virtual List<DeviantAbility> GetAbilities(bool checkLock)
		{
			return DeviantAbility.GetAbilities(this, checkLock);
		}

		public virtual bool CanUseAbility(DeviantAbility ability)
		{
			return true;
		}

		public virtual void OnAbility(DeviantAbility ability)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 4);            
        }

        public override bool OnBeforeDeath()
        {
            GoldShower.DoForChamp(Location, Map);

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            //if (Paragon.ChestChance > Utility.RandomDouble())
            c.DropItem(new Gold(3499, 5999));
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new ParagonChest(Name, 4));
            c.DropItem(new ParagonChest(Name, 4));
            c.DropItem(new ParagonChest(Name, 5));
            c.DropItem(new ParagonChest(Name, 5));
            c.DropItem(new IDWand());
            c.DropItem(new LightningWand());
            c.DropItem(new GreaterHealWand());

            PowerScroll ps = PowerScroll.CreateRandom(5, 5);
            ps.Value = Utility.RandomList(105, 110, 115, 120);
            c.DropItem(ps);    

            if (Utility.RandomDouble() < 0.5)
            {
                switch (Utility.Random(6))
                {
                    case 0:
                        c.DropItem(new TrueSlice());
                        break;
                    case 1:
                        c.DropItem(new SpartanSpear());
                        break;
                    case 2:
                        c.DropItem(new PeaceStick());
                        break;
                    case 3:
                        c.DropItem(new AncientDagger());
                        break;
                    case 4:
                        c.DropItem(new BodyHarvester());
                        break;
                    case 5:
                        c.DropItem(new Mjollnir());
                        break;
                }
            }

            if (Utility.RandomDouble() < 0.30)
                c.DropItem(new SocketDeedPlusOne());                    

            if (Utility.RandomDouble() < 0.25)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        c.DropItem(new GoldGem());
                        break;
                    case 1:
                        c.DropItem(new PoisonGem());
                        break;
                    case 2:
                        c.DropItem(new AttackGem());
                        break;
                }
            }

            if (Utility.RandomDouble() < 0.04)
                c.DropItem(new GemRecoveryHammer());

            if (Utility.RandomDouble() < 0.02)
            {
                switch (Utility.Random(8))
                {
                    case 0:
                        c.DropItem(new TreasurePile01AddonDeed());
                        break;
                    case 1:
                        c.DropItem(new TreasurePile02AddonDeed());
                        break;
                    case 2:
                        c.DropItem(new TreasurePile03AddonDeed());
                        break;
                    case 3:
                        c.DropItem(new TreasurePile04AddonDeed());
                        break;
                    case 4:
                        c.DropItem(new TreasurePile05AddonDeed());
                        break;
                    case 5:
                        c.DropItem(new TreasurePileAddonDeed());
                        break;
                    case 6:
                        c.DropItem(new TreasurePile2AddonDeed());
                        break;
                    case 7:
                        c.DropItem(new TreasurePile3AddonDeed());
                        break;
                }
            }

            if (Utility.RandomDouble() < 0.01)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        c.DropItem(new SpellHueDeed());
                        break;
                    case 1:
                        c.DropItem(new SpellHueDeed2());
                        break;                        
                }
            }

        }

        public override void OnDelete()
		{
			base.OnDelete();

			Instances.Remove(this);
			Instances.Free(false);
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Instances.Remove(this);
			Instances.Free(false);
		}

		#region Acquiring Targets
		public Mobile AcquireRandomTarget(int range)
		{
			return AcquireRandomTarget(Location, range);
		}

		public Mobile AcquireRandomTarget(Point3D p, int range)
		{
			return AcquireTargets(p, range).GetRandom();
		}

		public TMobile AcquireRandomTarget<TMobile>(int range)
			where TMobile : Mobile
		{
			return AcquireRandomTarget<TMobile>(Location, range);
		}

		public TMobile AcquireRandomTarget<TMobile>(Point3D p, int range)
			where TMobile : Mobile
		{
			return AcquireTargets<TMobile>(p, range).GetRandom();
		}

		public IEnumerable<TMobile> AcquireTargets<TMobile>(int range)
			where TMobile : Mobile
		{
			return AcquireTargets<TMobile>(Location, range);
		}

		public IEnumerable<TMobile> AcquireTargets<TMobile>(Point3D p, int range)
			where TMobile : Mobile
		{
			return AcquireTargets(p, range).OfType<TMobile>();
		}

		public IEnumerable<Mobile> AcquireTargets(int range)
		{
			return AcquireTargets(Location, range);
		}

		public virtual IEnumerable<Mobile> AcquireTargets(Point3D p, int range)
		{
			return p.FindMobilesInRange(Map, range)
				.Where(m => m != null && !m.Deleted && m != this && m.AccessLevel <= AccessLevel && m.Alive)
				.Where(m => CanBeHarmful(m, false, true) && SpellHelper.ValidIndirectTarget(this, m))
				.Where(m => !m.IsControlledBy(this))
				.Where(m => Team == 0 || !(m is BaseCreature) || Team != ((BaseCreature)m).Team)
				.Where(m => Party == null || m.Party == null || m.Party != Party)
				.Where(m => m.Player || m.IsControlled() || m.HasAggressed(this) || m.HasAggressor(this));
		}
		#endregion

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(3);

			switch (version)
			{
				case 3:
				case 2:
				{
					writer.WriteFlag(_Level);

					Deviations.Serialize(writer);
				}
				goto case 1;
				case 1:
					_EnrageStatBuff.Serialize(writer);
					goto case 0;
				case 0:
					break;
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			switch (version)
			{
				case 3:
				case 2:
				{
					_Level = reader.ReadFlag<DeviantLevel>();
					_Deviations = new DeviationAttributes(reader);
				}
				goto case 1;
				case 1:
				{
					if (version < 3)
						reader.ReadInt();

					_EnrageStatBuff.Deserialize(reader);
				}
				goto case 0;
				case 0:
					break;
			}

			if (_Deviations == null)
			{
				_Deviations = new DeviationAttributes(this);

				Title = InitTitle();
			}

			if (version < 2)
			{
				_Level = DefaultLevel;

				InitLevel();
			}
		}

		private sealed class BottomlessBackpack : StrongBackpack
		{
			public BottomlessBackpack()
			{
				MaxItems = 0;
				Movable = false;
				Hue = 0;
				Weight = 0.0;
			}

			public BottomlessBackpack(Serial serial)
				: base(serial)
			{ }

			public override void OnSnoop(Mobile m)
			{
				if (m != null && m.AccessLevel > AccessLevel.Player)
				{
					base.OnSnoop(m);
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
