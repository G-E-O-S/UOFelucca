#region Header
//   Vorspire    _,-'/-'/  GlobalTradeState.cs
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

using Server.Commands;

using VitaNex;
#endregion

namespace Server.Items
{
	public class GlobalTradeState : PropertyObject, IDisposable
	{
		private static int _UID;

		[CommandProperty(GlobalTrade.Access)]
		public int UID { get; private set; }

		[CommandProperty(GlobalTrade.Access)]
		public GlobalTradeContainer Container { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public Type Currency { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public Item Item { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public int Value { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public bool Trading { get; set; }

		[CommandProperty(GlobalTrade.Access)]
		public bool IsValid
		{
			get
			{
				if (IsDisposed)
				{
					return false;
				}

				if (Container == null || Container.Deleted)
				{
					return false;
				}

				if (Item == null || Item.Deleted)
				{
					return false;
				}

				if (Currency == null || Amount <= 0)
				{
					return false;
				}

				return Item.IsChildOf(Container);
			}
		}

		[CommandProperty(GlobalTrade.Access)]
		public Mobile Seller { get { return Container != null ? Container.Owner : null; } }

		[CommandProperty(GlobalTrade.Access)]
		public string Name { get { return Item != null ? Item.ResolveName() : String.Empty; } }

		[CommandProperty(GlobalTrade.Access)]
		public int ItemID { get { return Item != null ? Item.ItemID : -1; } }

		[CommandProperty(GlobalTrade.Access)]
		public int Quantity { get { return Item is IHasQuantity q ? q.Quantity : -1; } }

		[CommandProperty(GlobalTrade.Access)]
		public int Charges { get { return Item is IUsesRemaining u ? u.UsesRemaining : -1; } }

		[CommandProperty(GlobalTrade.Access)]
		public int Amount { get { return Item != null ? Item.Amount : -1; } }

		[CommandProperty(GlobalTrade.Access)]
		public int Hue { get { return Item != null ? Item.Hue : -1; } }

		[CommandProperty(GlobalTrade.Access)]
		public bool Stackable { get { return Item != null && Item.Stackable; } }

		[CommandProperty(GlobalTrade.Access)]
		public long Total { get { return Amount * (long)Value; } }

		public bool IsDisposed { get; private set; }

		public GlobalTradeState(GlobalTradeContainer cont, Item item)
			: this(cont, item, GlobalTrade.Currency, 0)
		{ }

		public GlobalTradeState(GlobalTradeContainer cont, Item item, Type currency, int value)
		{
			UID = ++_UID;

			Container = cont;
			Item = item;
			Currency = currency;
			Value = value;
		}

		public GlobalTradeState(GenericReader reader)
			: base(reader)
		{
			_UID = Math.Max(_UID, UID);
		}

		public override int GetHashCode()
		{
			return UID;
		}

		public override void Clear()
		{
			base.Clear();

			Value = 0;
			Trading = false;
		}

		public override void Reset()
		{
			base.Reset();

			Value = 0;
			Trading = false;
		}

		public void PushPriceHistory()
		{
			if (Item == null || Value <= 0)
			{
				return;
			}

			var profile = GlobalTrade.AcquireProfile(Seller);

			if (profile != null)
			{
				profile.PriceHistory[Item.GetType()] = Value;
			}
		}

		public void PullPriceHistory()
		{
			if (Item == null || Value > 0)
			{
				return;
			}

			var type = Item.GetType();

			var profile = GlobalTrade.AcquireProfile(Seller);

			if (profile != null && profile.PriceHistory.ContainsKey(type))
			{
				Value = profile.PriceHistory[type];
			}
		}

		public bool TryReclaim(Mobile user)
		{
			if (user == null || user.Deleted || !user.Player)
			{
				return false;
			}

			if (Item == null || Item.Deleted)
			{
				user.SendMessage("That item is no longer available.");
				return false;
			}

			if (user.AccessLevel < GlobalTrade.Access)
			{
				var seller = Seller;

				if (seller != null && !seller.CheckAccount(user))
				{
					user.SendMessage("You do not own that item.");
					return false;
				}
			}

			return Reclaim(user);
		}

		public bool Reclaim(Mobile user)
		{
			if (Item != null && !Item.Deleted)
			{
				if (!user.GiveItem(Item, GiveFlags.PackBankFeet).WasReceived())
				{
					return false;
				}
			}

			Trading = false;

			Timer.DelayCall(Dispose);

			return true;
		}

		public bool Purchase(Mobile buyer, int amount)
		{
			if (buyer == null || buyer.Deleted || !buyer.Player)
			{
				return false;
			}

			if (buyer.Backpack == null)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			var seller = Seller;

			if (seller == null || seller.Deleted || !seller.Player)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (seller.Backpack == null)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (!GlobalTrade.CMOptions.ModuleEnabled)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (!IsValid || !Trading)
			{
				buyer.SendMessage("That item is no longer available.");
				return false;
			}
			
			amount = Math.Max(0, Math.Min(Amount, amount));
			
			Item item = null;

			if (amount > 0)
			{
				if (Stackable && Amount > 1 && amount < Amount)
				{
					item = Dupe.DupeItem(Item);

					if (item != null)
					{
						item.Amount = amount;
					}
				}
				else
				{
					item = Item;
				}
			}

			if (item == null)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			var currency = Currency;

			if (currency == null)
			{
				buyer.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			var total = Value * (long)amount;

			if (!GlobalTrade.Withdraw(buyer, currency, total))
			{
				if (item != Item)
				{
					item.Delete();
				}

				buyer.SendMessage("You cannot afford that item.");
				return false;
			}

			buyer.SendMessage("{0:#,0} {1} has been purchased for {2:#,0} {3} from the global trade market.", amount, item.ResolveName(buyer), total, currency.GetTypeName(false));

			seller.SendMessage("{0:#,0} {1} has been sold for {2:#,0} {3} on the global trade market.", amount, item.ResolveName(seller), total, currency.GetTypeName(false));

			GlobalTrade.Deposit(seller, currency, total);

			GlobalTrade.CreateRecords(currency, item, seller, buyer, amount, Value);

			if (Item != item)
			{
				Item.Consume(item.Amount);
			}

			if (Item == item || Item.Deleted)
			{
				Trading = false;

				Timer.DelayCall(Dispose);
			}

			buyer.GiveItem(item, GiveFlags.PackBankFeet);

			return true;
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			IsDisposed = true;

			if (Item != null)
			{
				GlobalTrade.Items.Remove(Item);
			}

			var profile = GlobalTrade.FindProfile(Seller);

			if (profile != null)
			{
				profile.Stock.Remove(this);
			}

			Clear();

			Container = null;
			Currency = null;
			Item = null;

			GC.SuppressFinalize(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(1);

			writer.Write(UID);

			writer.Write(Container);

			writer.Write(Item);
			
#if ServUOX
			writer.WriteObjectType(Currency);
#else
			writer.WriteType(Currency);
#endif

			writer.Write(Value);
			writer.Write(Trading);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var v = reader.GetVersion();

			UID = reader.ReadInt();

			Container = reader.ReadItem<GlobalTradeContainer>();

			Item = reader.ReadItem();
			
#if ServUOX
			Currency =  reader.ReadObjectType();
#else
			Currency = reader.ReadType();
#endif

			Value = reader.ReadInt();
			Trading = reader.ReadBool();

			if (v < 1 && !Stackable && Amount == 1)
			{
				int val;

				if ((val = Charges) > 0)
				{
					Value *= val;
				}
				else if ((val = Quantity) > 0)
				{
					Value *= val;
				}
			}
		}
	}
}
