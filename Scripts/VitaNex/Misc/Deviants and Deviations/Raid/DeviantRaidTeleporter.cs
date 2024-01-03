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

namespace Server.Items
{
	public class DeviantRaidTeleporter : Teleporter
	{
		public override string DefaultName => "Deviant Raid Teleporter";

		[CommandProperty(AccessLevel.GameMaster)]
		public int DeviantKeysRequired { get; set; }

		[Constructable]
		public DeviantRaidTeleporter()
			: this(1)
		{ }

		[Constructable]
		public DeviantRaidTeleporter(Point3D pointDest, Map mapDest)
			: this(1, pointDest, mapDest)
		{ }

		[Constructable]
		public DeviantRaidTeleporter(Point3D pointDest, Map mapDest, bool creatures)
			: this(1, pointDest, mapDest, creatures)
		{ }

		[Constructable]
		public DeviantRaidTeleporter(int keys)
			: this(keys, Point3D.Zero, null, false)
		{ }

		[Constructable]
		public DeviantRaidTeleporter(int keys, Point3D pointDest, Map mapDest)
			: this(keys, pointDest, mapDest, false)
		{ }

		[Constructable]
		public DeviantRaidTeleporter(int keys, Point3D pointDest, Map mapDest, bool creatures)
			: base(pointDest, mapDest, creatures)
		{
			DeviantKeysRequired = keys;
		}

		public DeviantRaidTeleporter(Serial serial)
			: base(serial)
		{ }

		public override bool CanTeleport(Mobile m)
		{
			if (!base.CanTeleport(m))
				return false;

			if (DeviantKeysRequired > 0 && m.Player)
			{
				if (!m.Backpack.HasItem<DeviantRaidKey>(DeviantKeysRequired, false))
				{
					if (DeviantKeysRequired > 1)
						m.SendMessage("{0} deviant raid keys are required to teleport.", DeviantKeysRequired);
					else
						m.SendMessage("A deviant raid key is required to teleport.");

					return false;
				}
			}

			return true;
		}

		public override void DoTeleport(Mobile m)
		{
			var req = DeviantKeysRequired;

			var keys = m.Backpack.FindItemsByType<DeviantRaidKey>(true);

			foreach (var key in keys)
			{
				if (key.Amount >= req)
				{
					key.Consume(req);
					break;
				}

				req -= key.Amount;

				key.Delete();

				if (req <= 0)
					break;
			}

			if (DeviantKeysRequired > 0)
			{
				if (DeviantKeysRequired > 1)
					m.SendMessage("{0} deviant raid keys were used to teleport.", DeviantKeysRequired);
				else
					m.SendMessage("A deviant raid was used to teleport.");
			}

			base.DoTeleport(m);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(DeviantKeysRequired);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			DeviantKeysRequired = reader.ReadInt();
		}
	}
}
