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

using System;
using System.Linq;

using Server.Items;

namespace Server.Mobiles
{
	public class DeviantRaidBoss : BaseDeviant
	{
		private static readonly Body[] _ValidBodies;
		private static readonly AIType[] _ValidAI;

		private static readonly DeviationFlags[] _ValidFlags;
		private static readonly DeviantLevel[] _ValidLevels;

		static DeviantRaidBoss()
		{
			_ValidBodies = Enumerable.Range(1, 4096).Select(b => (Body)b).Where(b => b.IsMonster).ToArray();

			_ValidAI = new[] { AIType.AI_Mage, AIType.AI_Melee };

			_ValidFlags = default(DeviationFlags).EnumerateValues<DeviationFlags>(false).Where(f => f != 0 && f != DeviationFlags.All).ToArray();

			_ValidLevels = new[] { DeviantLevel.Taxing, DeviantLevel.Extreme };
		}

		private static DeviationFlags GetRandomDeviations()
		{
			if (_ValidFlags.Length == 0)
				return 0;

			if (_ValidFlags.Length < 4)
				return _ValidFlags.GetRandom();

			var length = Math.Min(8, _ValidFlags.Length);
			var count = Utility.RandomMinMax(length / 4, length / 2);

			var flags = DeviationFlags.None;

			while (--count >= 0)
			{
				var f = _ValidFlags.GetRandom();

				if (!flags.HasFlag(f))
					flags |= f;
				else
					++count;
			}

			return flags;
		}

		private DeviationFlags _DefaultFlags = GetRandomDeviations();

		public override DeviationFlags DefaultDeviations => _DefaultFlags;

		private DeviantLevel _DefaultLevel = _ValidLevels.GetRandom();

		public override DeviantLevel DefaultLevel => _DefaultLevel;

		[CommandProperty(AccessLevel.GameMaster)]
		public int DeviantKeysDropped { get; set; }

		[Constructable]
		public DeviantRaidBoss()
			: this(1)
		{ }

		[Constructable]
		public DeviantRaidBoss(int keys)
			: base(_ValidAI.GetRandom(), FightMode.Closest, 16, 1, 0.1, 0.2)
		{
			DeviantKeysDropped = keys;

			Name = NameList.RandomName("daemon");
		}

		public DeviantRaidBoss(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return _ValidBodies.GetRandom();
		}

		public override void OnKilledBy(Mobile mob)
		{
			base.OnKilledBy(mob);

			if (mob != null && mob.Player)
			{
				var count = DeviantKeysDropped;

				while (--count >= 0)
					mob.GiveItem(new DeviantRaidKey(), GiveFlags.All);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.WriteFlag(_DefaultFlags);
			writer.WriteFlag(_DefaultLevel);

			writer.Write(DeviantKeysDropped);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			_DefaultFlags = reader.ReadFlag<DeviationFlags>();
			_DefaultLevel = reader.ReadFlag<DeviantLevel>();

			DeviantKeysDropped = reader.ReadInt();
		}
	}
}
