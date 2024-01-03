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
using VitaNex;
#endregion

namespace Server.Mobiles
{
	public sealed class DeviationAttributes : SettingsObject<DeviationFlags>
	{
		public bool IsEmpty { get { return Flags == DeviationFlags.None; } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Time { get { return GetFlag(DeviationFlags.Time); } set { SetFlag(DeviationFlags.Time, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Light { get { return GetFlag(DeviationFlags.Light); } set { SetFlag(DeviationFlags.Light, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Darkness { get { return GetFlag(DeviationFlags.Darkness); } set { SetFlag(DeviationFlags.Darkness, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Faith { get { return GetFlag(DeviationFlags.Faith); } set { SetFlag(DeviationFlags.Faith, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Despair { get { return GetFlag(DeviationFlags.Despair); } set { SetFlag(DeviationFlags.Despair, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Illusion { get { return GetFlag(DeviationFlags.Illusion); } set { SetFlag(DeviationFlags.Illusion, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Life { get { return GetFlag(DeviationFlags.Life); } set { SetFlag(DeviationFlags.Life, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Death { get { return GetFlag(DeviationFlags.Death); } set { SetFlag(DeviationFlags.Death, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Elements { get { return GetFlag(DeviationFlags.Elements); } set { SetFlag(DeviationFlags.Elements, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Greed { get { return GetFlag(DeviationFlags.Greed); } set { SetFlag(DeviationFlags.Greed, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Famine { get { return GetFlag(DeviationFlags.Famine); } set { SetFlag(DeviationFlags.Famine, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Tech { get { return GetFlag(DeviationFlags.Tech); } set { SetFlag(DeviationFlags.Tech, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Decay { get { return GetFlag(DeviationFlags.Decay); } set { SetFlag(DeviationFlags.Decay, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Earth { get { return GetFlag(DeviationFlags.Earth); } set { SetFlag(DeviationFlags.Earth, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Fire { get { return GetFlag(DeviationFlags.Fire); } set { SetFlag(DeviationFlags.Fire, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Frost { get { return GetFlag(DeviationFlags.Frost); } set { SetFlag(DeviationFlags.Frost, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Poison { get { return GetFlag(DeviationFlags.Poison); } set { SetFlag(DeviationFlags.Poison, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Energy { get { return GetFlag(DeviationFlags.Energy); } set { SetFlag(DeviationFlags.Energy, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool Chaos { get { return GetFlag(DeviationFlags.Chaos); } set { SetFlag(DeviationFlags.Chaos, value); } }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool All { get { return GetFlag(DeviationFlags.All); } set { SetFlag(DeviationFlags.All, value); } }

		public DeviationAttributes(BaseDeviant owner)
			: this(owner.DefaultDeviations)
		{
			OnChanged = owner.DeviationChanged;
		}

		public DeviationAttributes()
			: this(DeviationFlags.None)
		{ }

		public DeviationAttributes(DeviationFlags flags)
			: base(flags)
		{ }

		public DeviationAttributes(GenericReader reader)
			: base(reader)
		{ }

		public override string ToString()
		{
			return "...";
		}

		public override void Clear()
		{
			Flags = DeviationFlags.None;
		}

		public override void Reset()
		{
			Flags = DeviationFlags.None;
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
