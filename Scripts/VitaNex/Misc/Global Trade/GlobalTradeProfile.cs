#region Header
//   Vorspire    _,-'/-'/  GlobalTradeProfile.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server.Accounting;

using VitaNex;
#endregion

namespace Server.Items
{
	public sealed class GlobalTradeProfile : PropertyObject
	{
		[CommandProperty(GlobalTrade.Access)]
		public IAccount Account { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public List<GlobalTradeRecord> Records { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public List<GlobalTradeState> Stock { get; private set; }

		public Dictionary<Type, int> PriceHistory { get; private set; }

		public GlobalTradeProfile(IAccount account)
		{
			Account = account;

			Records = new List<GlobalTradeRecord>();
			Stock = new List<GlobalTradeState>();

			PriceHistory = new Dictionary<Type, int>();
		}

		public GlobalTradeProfile(GenericReader reader)
			: base(reader)
		{ }

		private void SetDefaults()
		{
			Records.Clear();
			PriceHistory.Clear();
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

		public IEnumerable<GlobalTradeRecord> GetSales()
		{
			return Records.Where(o => o.SellerAccount == Account);
		}

		public IEnumerable<GlobalTradeRecord> GetPurchases()
		{
			return Records.Where(o => o.BuyerAccount == Account);
		}

		public void CreateRecord(DateTime date, Type currency, Item item, Mobile seller, Mobile buyer, int amount, int value)
		{
			Records.Add(new GlobalTradeRecord(date, currency, item, seller, buyer, amount, value));
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Account);

			writer.WriteBlockList(Records, (w, o) => o.Serialize(w));
			writer.WriteBlockList(Stock, (w, o) => o.Serialize(w));

			writer.WriteBlockDictionary(
				PriceHistory,
				(w, k, v) =>
				{
					w.WriteType(k);
					w.Write(v);
				});
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Account = reader.ReadAccount();

			Records = reader.ReadBlockList(r => new GlobalTradeRecord(r), Records);
			Stock = reader.ReadBlockList(r => new GlobalTradeState(r), Stock);

			PriceHistory = reader.ReadBlockDictionary(
				r =>
				{
					var k = r.ReadType();
					var v = r.ReadInt();

					return new KeyValuePair<Type, int>(k, v);
				},
				PriceHistory);
		}
	}
}