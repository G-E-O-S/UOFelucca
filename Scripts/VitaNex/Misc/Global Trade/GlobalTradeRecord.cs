#region Header
//   Vorspire    _,-'/-'/  GlobalTradeRecord.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#if ServUO58
#define ServUOX
#endif

#region References
using System;

using Server.Accounting;

using VitaNex;
#endregion

namespace Server.Items
{
	public sealed class GlobalTradeRecord : PropertyObject
	{
		private static int _UID;

		[CommandProperty(GlobalTrade.Access)]
		public int UID { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public DateTime Date { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public string Currency { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public string ItemType { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public int ItemAsset { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public int ItemHue { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public Serial ItemSerial { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public string ItemName { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public IAccount SellerAccount { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public Serial SellerSerial { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public string SellerName { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public IAccount BuyerAccount { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public Serial BuyerSerial { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public string BuyerName { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public int Amount { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public int Value { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public long Total { get { return Amount * (long)Value; } }

		public GlobalTradeRecord(DateTime date, Type currency, Item item, Mobile seller, Mobile buyer, int amount, int value)
		{
			UID = ++_UID;

			Date = date;

			if (currency != null)
			{
				Currency = currency.GetTypeName(false);
			}

			if (item != null)
			{
				ItemType = item.GetTypeName(true);

				ItemAsset = item.ItemID;
				ItemHue = item.Hue;

				ItemSerial = item.Serial;
				ItemName = item.ResolveName();
			}

			if (seller != null)
			{
				SellerAccount = seller.Account;
				SellerSerial = seller.Serial;
				SellerName = seller.RawName;
			}

			if (buyer != null)
			{
				BuyerAccount = buyer.Account;
				BuyerSerial = buyer.Serial;
				BuyerName = buyer.RawName;
			}

			Amount = amount;
			Value = value;
		}

		public GlobalTradeRecord(GenericReader reader)
			: base(reader)
		{
			_UID = Math.Max(_UID, UID);
		}

		public override int GetHashCode()
		{
			return UID;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(UID);

			writer.Write(Date);

			writer.Write(Currency);

			writer.Write(ItemType);

			writer.Write(ItemAsset);
			writer.Write(ItemHue);

			writer.Write(ItemSerial);
			writer.Write(ItemName);

			writer.Write(SellerAccount);
			writer.Write(SellerSerial);
			writer.Write(SellerName);

			writer.Write(BuyerAccount);
			writer.Write(BuyerSerial);
			writer.Write(BuyerName);

			writer.Write(Amount);
			writer.Write(Value);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			UID = reader.ReadInt();

			Date = reader.ReadDateTime();

			Currency = reader.ReadString();

			ItemType = reader.ReadString();

			ItemAsset = reader.ReadInt();
			ItemHue = reader.ReadInt();

#if ServUOX
			ItemSerial = reader.ReadSerial();
			ItemName = reader.ReadString();

			SellerAccount = reader.ReadAccount();
			SellerSerial = reader.ReadSerial();
			SellerName = reader.ReadString();

			BuyerAccount = reader.ReadAccount();
			BuyerSerial = reader.ReadSerial();
			BuyerName = reader.ReadString();
#else
			ItemSerial = reader.ReadInt();
			ItemName = reader.ReadString();

			SellerAccount = reader.ReadAccount();
			SellerSerial = reader.ReadInt();
			SellerName = reader.ReadString();

			BuyerAccount = reader.ReadAccount();
			BuyerSerial = reader.ReadInt();
			BuyerName = reader.ReadString();
#endif

			Amount = reader.ReadInt();
			Value = reader.ReadInt();
		}
	}
}
