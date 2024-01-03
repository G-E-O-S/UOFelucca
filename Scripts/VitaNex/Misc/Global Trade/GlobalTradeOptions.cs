#region Header
//   Vorspire    _,-'/-'/  GlobalTradeOptions.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using VitaNex;
#endregion

namespace Server.Items
{
	public sealed class GlobalTradeOptions : CoreModuleOptions
	{
		[CommandProperty(GlobalTrade.Access)]
		public ItemTypeSelectProperty Currency { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public int StockLimit { get; set; }

		public GlobalTradeOptions()
			: base(typeof(GlobalTrade))
		{
			SetDefaults();
		}

		public GlobalTradeOptions(GenericReader reader)
			: base(reader)
		{ }

		public void SetDefaults()
		{
			Currency = GlobalTrade.DefaultCurrency;

			StockLimit = 50;
		}

		public override void Clear()
		{
			base.Clear();

			SetDefaults();
		}

		public override void Reset()
		{
			base.Reset();

			SetDefaults();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			Currency.Serialize(writer);

			writer.Write(StockLimit);
		}

		public override void Deserialize(GenericReader reader)
		{
			SetDefaults();

			base.Deserialize(reader);

			reader.GetVersion();

			Currency.Deserialize(reader);

			StockLimit = reader.ReadInt();
		}
	}
}