#region Header
//   Vorspire    _,-'/-'/  CharityState.cs
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
	public class CharityState : PropertyObject, IDisposable
	{
		private static int _UID;

		[CommandProperty(Charity.Access)] 
		public int UID { get; private set; }

		[CommandProperty(Charity.Access)] 
		public CharityContainer Container { get; set; }
		
		[CommandProperty(Charity.Access)] 
		public Item Item { get; set; }
		
		[CommandProperty(Charity.Access)]
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

				if (Amount <= 0)
				{
					return false;
				}

				return Item.IsChildOf(Container);
			}
		}

		[CommandProperty(Charity.Access)]
		public Mobile Giver { get { return Container != null ? Container.Owner : null; } }

		[CommandProperty(Charity.Access)]
		public string Name { get { return Item != null ? Item.ResolveName() : String.Empty; } }

		[CommandProperty(Charity.Access)] 
		public int ItemID { get { return Item != null ? Item.ItemID : -1; } }

		[CommandProperty(Charity.Access)]
		public int Quantity { get { return Item is IHasQuantity q ? q.Quantity : -1; } }

		[CommandProperty(Charity.Access)]
		public int Charges { get { return Item is IUsesRemaining u ? u.UsesRemaining : -1; } }

		[CommandProperty(Charity.Access)]
		public int Amount { get { return Item != null ? Item.Amount : -1; } }

		[CommandProperty(Charity.Access)] 
		public int Hue { get { return Item != null ? Item.Hue : -1; } }

		[CommandProperty(Charity.Access)]
		public bool Stackable { get { return Item != null && Item.Stackable; } }
		
		public bool IsDisposed { get; private set; }
		
		public CharityState(CharityContainer cont, Item item)
		{
			UID = ++_UID;

			Container = cont;
			Item = item;
		}

		public CharityState(GenericReader reader)
			: base(reader)
		{
			_UID = Math.Max(_UID, UID);
		}

		public override int GetHashCode()
		{
			return UID;
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

			if (user.AccessLevel < Charity.Access)
			{
				var giver = Giver;

				if (giver != null && !giver.CheckAccount(user))
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
			
			Timer.DelayCall(Dispose);

			return true;
		}

		public bool Take(Mobile receiver, int amount)
		{
			if (receiver == null || receiver.Deleted || !receiver.Player)
			{
				return false;
			}

			if (receiver.Backpack == null)
			{
				receiver.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			var giver = Giver;

			if (giver == null || giver.Deleted || !giver.Player)
			{
				receiver.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (giver.Backpack == null)
			{
				receiver.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (!Charity.CMOptions.ModuleEnabled)
			{
				receiver.SendMessage("That item is temporarily unavailable.");
				return false;
			}

			if (!IsValid)
			{
				receiver.SendMessage("That item is no longer available.");
				return false;
			}

			if (Charity.CMOptions.AgeLimit > TimeSpan.Zero)
			{
#if ServUOX
				if (receiver.Account.Age > Charity.CMOptions.AgeLimit)
#else
				if (receiver.Account is Accounting.Account acc && DateTime.UtcNow - acc.Created > Charity.CMOptions.AgeLimit)
#endif
				{
					receiver.SendMessage($"You may only take charity donations if your account is younger than {Charity.CMOptions.AgeLimit.ToSimpleString(@"!<d#days #><h#hours #><m#minutes #><s#seconds#>")}");
					return false;
				}
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
				receiver.SendMessage("That item is temporarily unavailable.");
				return false;
			}
			
			receiver.SendMessage("{0:#,0} {1} has been taken from charity.", amount, item.ResolveName(receiver));

			giver.SendMessage("{0:#,0} {1} has been taken from charity by a needy adventurer.", amount, item.ResolveName(giver));
			
			Charity.CreateRecords(item, giver, receiver, amount);

			if (Item != item)
			{
				Item.Consume(item.Amount);
			}

			if (Item == item || Item.Deleted)
			{
				Timer.DelayCall(Dispose);
			}

			receiver.GiveItem(item, GiveFlags.PackBankFeet);

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
				Charity.Items.Remove(Item);
			}

			var profile = Charity.FindProfile(Giver);

			if (profile != null)
			{
				profile.Stock.Remove(this);
			}

			Clear();

			Container = null;
			Item = null;

			GC.SuppressFinalize(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(UID);

			writer.Write(Container);

			writer.Write(Item);

			writer.Write(true);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			UID = reader.ReadInt();

			Container = reader.ReadItem<CharityContainer>();

			Item = reader.ReadItem();

			reader.ReadBool();
		}
	}
}
