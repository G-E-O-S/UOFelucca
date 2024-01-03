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

using Server.Items;
#endregion

namespace Server.Mobiles
{
	public abstract class DeviantSpawn : BaseCreature, IDeviantSpawn
	{
		[CommandProperty(AccessLevel.GameMaster, true)]
		public BaseDeviant Deviant { get; private set; }

		public override bool AlwaysAttackable { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }

		public override bool DeleteCorpseOnDeath { get { return true; } }

		public override bool CanBeParagon { get { return false; } }

		public override bool CanFlee { get { return Deviant == null || Deviant.Deleted || !Deviant.Alive; } }

		public DeviantSpawn(BaseDeviant deviant, AIType ai, FightMode mode, double dActiveSpeed, double dPassiveSpeed)
			: base(ai, mode, deviant.RangePerception, deviant.RangeFight, dActiveSpeed, dPassiveSpeed)
		{
			Deviant = deviant;

			Name = "Deviant Spawn";
			Body = 129;

			Hue = Deviant.Hue;
			Team = Deviant.Team;

			Resistances.SetAll(i => Deviant.Resistances[i]);

			SetDamageType(ResistanceType.Physical, Deviant.PhysicalDamage);
			SetDamageType(ResistanceType.Fire, Deviant.FireDamage);
			SetDamageType(ResistanceType.Cold, Deviant.ColdDamage);
			SetDamageType(ResistanceType.Poison, Deviant.PoisonDamage);
			SetDamageType(ResistanceType.Energy, Deviant.EnergyDamage);

			Deviant.Scale(this);

			SpawnDeviantAbility.Register(this);
		}

		public DeviantSpawn(Serial serial)
			: base(serial)
		{ }

		public override void OnThink()
		{
			base.OnThink();

			if (Deviant == null || Deviant.Deleted || !Deviant.Alive || !Deviant.InRange(this, Deviant.RangePerception * 2))
			{
				Kill();
			}
		}

		public override void AggressiveAction(Mobile aggressor)
		{
			base.AggressiveAction(aggressor);

			if (Deviant != null)
			{
				Deviant.AggressiveAction(aggressor);
			}
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (c != null)
			{
				c.Delete();
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			SpawnDeviantAbility.Unregister(this);

			if (Corpse != null)
			{
				Corpse.Delete();
			}

			if (Deviant != null)
			{
				Deviant = null;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Deviant);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Deviant = reader.ReadMobile<BaseDeviant>();

			SpawnDeviantAbility.Register(this);
		}
	}
}
