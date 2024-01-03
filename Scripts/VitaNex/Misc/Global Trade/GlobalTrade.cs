#region Header
//   Vorspire    _,-'/-'/  GlobalTrade.cs
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
using System.Collections.Generic;
using System.Linq;

using Server.Accounting;
using Server.Mobiles;

using VitaNex;
using VitaNex.IO;
using VitaNex.Items;
#endregion

namespace Server.Items
{
	[CoreModule("Global Trade", "1.0.0.2")]
	public static class GlobalTrade
	{
		public const AccessLevel Access = AccessLevel.Administrator;

		public static readonly Type DefaultCurrency = typeof(Gold);

		public static GlobalTradeOptions CMOptions { get; private set; }

		public static Dictionary<string, Type[]> Categories { get; private set; }

		public static Dictionary<Item, GlobalTradeState> Items { get; private set; }

		public static BinaryDataStore<IAccount, GlobalTradeProfile> Profiles { get; private set; }

		public static List<GlobalTradeContainer> Chests { get { return GlobalTradeContainer.Instances; } }

		public static Type Currency { get { return CMOptions.Currency.InternalType ?? DefaultCurrency; } }

		static GlobalTrade()
		{
			CMOptions = new GlobalTradeOptions();

			Categories = new Dictionary<string, Type[]>
			{
				{"Currencies", new[] {typeof(IVendorToken)}},

				{"Resources|Granite", new[] {typeof(BaseGranite)}},
				{"Resources|Leather", new[] {typeof(BaseHides), typeof(BaseLeather)}},
				{"Resources|Metal", new[] {typeof(BaseOre), typeof(BaseIngot)}},
				{"Resources|Scales", new[] {typeof(BaseScales)}},
#if ServUOX
				{"Resources|Wood", new[] {typeof(BaseLog), typeof(BaseWoodBoard)}},
#else
				{"Resources|Wood", new[] {typeof(BaseLog), typeof(BaseBoard)}},
#endif
				{"Tools|Crafting", new[] {typeof(BaseTool)}},
				{"Tools|Harvesting", new[] {typeof(BaseHarvestTool)}},
				{"Tools|Runic", new[] {typeof(BaseRunicTool)}}
			};

			Items = new Dictionary<Item, GlobalTradeState>();

			Profiles = new BinaryDataStore<IAccount, GlobalTradeProfile>(VitaNexCore.SavesDirectory + "/GlobalTrade", "Profiles")
			{
				Async = true,
				OnSerialize = SerializeProfiles,
				OnDeserialize = DeserializeProfiles
			};
		}

		private static void CMConfig()
		{
			EventSink.DeleteRequest += OnDeleteRequest;
		}

		private static void CMSave()
		{
			Profiles.Export();
		}

		private static void CMLoad()
		{
			Profiles.Import();

			foreach (var p in Profiles.Values)
			{
				foreach (var o in p.Stock.Where(o => o.Item != null))
				{
					Items[o.Item] = o;
				}

				foreach (var c in p.Account.FindMobiles().SelectMany(m => m.Items.OfType<GlobalTradeContainer>()))
				{
					foreach (var o in c.Items)
					{
						if (!Items.TryGetValue(o, out var state))
						{
							Items[o] = new GlobalTradeState(c, o);
						}
					}
				}
			}
		}

		private static bool SerializeProfiles(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.WriteBlockDictionary(Profiles, (w, k, v) => v.Serialize(w));

			return true;
		}

		private static bool DeserializeProfiles(GenericReader reader)
		{
			reader.GetVersion();

			reader.ReadBlockDictionary(r =>
			{
				var o = new GlobalTradeProfile(r);

				return new KeyValuePair<IAccount, GlobalTradeProfile>(o.Account, o);
			}, Profiles);

			return true;
		}

		public static IEnumerable<string> FindCategories(Item item)
		{
			if (item == null || item.TypeEquals(Currency))
			{
				return Enumerable.Empty<string>();
			}

			return FindCategories(item.GetType());
		}

		public static IEnumerable<string> FindCategories(Type item)
		{
			if (item == null || item.TypeEquals(Currency))
			{
				yield break;
			}

			foreach (var o in Categories)
			{
				if (o.Value.Any(item.IsEqualOrChildOf))
				{
					yield return o.Key;
				}
			}
		}

		public static bool HasCategory(Item item)
		{
			return item != null && HasCategory(item.GetType());
		}

		public static bool HasCategory(Type item)
		{
			if (item == null || item.TypeEquals(Currency))
			{
				return false;
			}

			foreach (var o in Categories)
			{
				if (o.Value.Any(item.IsEqualOrChildOf))
				{
					return true;
				}
			}

			return false;
		}

		public static bool InCategory(Item item, string category)
		{
			return item != null && InCategory(item.GetType(), category);
		}

		public static bool InCategory(Type item, string category)
		{
			if (item == null || item.TypeEquals(Currency))
			{
				return false;
			}

			if (Categories.ContainsKey(category))
			{
				return Categories[category].Any(item.IsEqualOrChildOf);
			}

			return FindCategories(item).Any(o => o.StartsWith(category, StringComparison.OrdinalIgnoreCase));
		}

		public static GlobalTradeProfile AcquireProfile(Mobile m)
		{
			if (m != null && m.Account != null)
			{
				return AcquireProfile(m.Account);
			}

			return null;
		}

		public static GlobalTradeProfile AcquireProfile(IAccount a)
		{
			if (a == null)
			{
				return null;
			}

			if (!Profiles.TryGetValue(a, out var profile) || profile == null)
			{
				Profiles[a] = profile = new GlobalTradeProfile(a);
			}

			return profile;
		}

		public static GlobalTradeProfile FindProfile(Mobile m)
		{
			if (m != null && m.Account != null)
			{
				return FindProfile(m.Account);
			}

			return null;
		}

		public static GlobalTradeProfile FindProfile(IAccount a)
		{
			if (a == null)
			{
				return null;
			}

			return Profiles.GetValue(a);
		}

		public static bool DestroyState(Item item)
		{
			if (item == null)
			{
				return false;
			}

			var state = Items.GetValue(item);

			if (state != null)
			{
				state.Dispose();
				return true;
			}

			return false;
		}

		public static GlobalTradeState FindState(Item item)
		{
			return item != null ? Items.GetValue(item) : null;
		}

		public static GlobalTradeState AcquireState(Item item)
		{
			if (item == null || item.Deleted || !HasCategory(item))
			{
				return null;
			}

			if (!Items.TryGetValue(item, out var state) || state == null)
			{
				GlobalTradeContainer c = null;

				var p = item.Parent as Item;

				while (p != null)
				{
					if (p is GlobalTradeContainer)
					{
						c = (GlobalTradeContainer)p;
						break;
					}

					p = p.Parent as Item;
				}

				if (c != null)
				{
					Items[item] = state = new GlobalTradeState(c, item);

					var profile = AcquireProfile(c.Owner);

					if (profile != null)
					{
						profile.Stock.Add(state);
					}
				}
			}

			return state;
		}

		public static void CreateRecords(Type currency, Item item, Mobile seller, Mobile buyer, int amount, int value)
		{
			var date = DateTime.UtcNow;

			var sp = AcquireProfile(seller);

			if (sp != null)
			{
				sp.CreateRecord(date, currency, item, seller, buyer, amount, value);
			}

			var bp = AcquireProfile(buyer);

			if (bp != null)
			{
				bp.CreateRecord(date, currency, item, seller, buyer, amount, value);
			}
		}

		public static long GetBalance(Mobile user)
		{
			return GetBalance(user, CMOptions.Currency);
		}

		public static long GetBalance(Mobile user, Type currency)
		{
			var balance = 0L;

			if (currency != null)
			{
				if (currency.IsEqual<Gold>())
				{
					if (AccountGold.Enabled)
					{
						balance += (long)(user.Account.TotalCurrency * AccountGold.CurrencyThreshold);
					}
					else
					{
						balance += Banker.GetBalance(user);
					}
				}
				else
				{
					if (user.Backpack != null)
					{
						balance += user.Backpack.GetAmount(currency);
					}

					if (user.BankBox != null)
					{
						balance += user.BankBox.GetAmount(currency);
					}
				}
			}

			return balance;
		}

		public static bool HasBalance(Mobile user, long amount)
		{
			return GetBalance(user) >= amount;
		}

		public static bool HasBalance(Mobile user, Type currency, long amount)
		{
			return GetBalance(user, currency) >= amount;
		}

		public static bool Withdraw(Mobile user, long amount)
		{
			return Withdraw(user, Currency, amount);
		}

		public static bool Withdraw(Mobile user, Type currency, long amount)
		{
			if (GetBalance(user, currency) < amount)
			{
				return false;
			}

			if (currency != null)
			{
				if (currency.IsEqual<Gold>())
				{
					if (AccountGold.Enabled)
					{
						if (user.Account.WithdrawGold(amount))
						{
							return true;
						}
					}
					else
					{
						while (amount > 0)
						{
							var take = (int)Math.Min(Int32.MaxValue, amount);

							if (Banker.Withdraw(user, take))
							{
								amount -= take;
							}
							else
							{
								break;
							}
						}

						return true;
					}
				}
				else
				{
					var pack = user.Backpack;

					if (pack != null)
					{
						while (amount > 0)
						{
							var take = pack.ConsumeUpTo(currency, (int)Math.Min(Int32.MaxValue, amount));

							if (take > 0)
							{
								amount -= take;
							}
							else
							{
								break;
							}
						}
					}

					var bank = user.BankBox;

					if (bank != null)
					{
						while (amount > 0)
						{
							var take = bank.ConsumeUpTo(currency, (int)Math.Min(Int32.MaxValue, amount));

							if (take > 0)
							{
								amount -= take;
							}
							else
							{
								break;
							}
						}
					}

					return true;
				}
			}

			return false;
		}

		public static long DepositUpTo(Mobile user, long amount)
		{
			return DepositUpTo(user, Currency, amount);
		}

		public static long DepositUpTo(Mobile user, Type currency, long amount)
		{
			if (user == null)
			{
				return 0;
			}

			var deposit = 0L;

			if (currency != null)
			{
				if (currency.IsEqual<Gold>())
				{
					if (AccountGold.Enabled)
					{
						if (user.Account.DepositGold(amount))
						{
							deposit = amount;
						}
					}
					else
					{
						while (deposit < amount)
						{
							var give = Banker.DepositUpTo(user, (int)Math.Min(Int32.MaxValue, amount));

							if (give > 0)
							{
								deposit += give;
							}
							else
							{
								break;
							}
						}
					}
				}
				else
				{
					var bank = user.BankBox;

					if (bank != null)
					{
						while (deposit < amount)
						{
							var item = currency.CreateInstance<Item>();

							if (item != null)
							{
								if (item.Stackable)
								{
									item.Amount = (int)Math.Min(60000, amount - deposit);
								}

								bank.DropItem(item);

								deposit += item.Amount;
							}
							else
							{
								break;
							}
						}
					}
				}
			}

			return deposit;
		}

		public static void Deposit(Mobile user, long amount)
		{
			DepositUpTo(user, amount);
		}

		public static void Deposit(Mobile user, Type currency, long amount)
		{
			DepositUpTo(user, currency, amount);
		}

		private static void OnDeleteRequest(DeleteRequestEventArgs e)
		{
			if (e.State == null || e.State.Account == null || e.Index < 0 || e.Index >= e.State.Account.Length)
			{
				return;
			}

			var d = e.State.Account[e.Index];

			if (d == null)
			{
				return;
			}

			var m = e.State.Account.FindMobiles<PlayerMobile>(o => o != d).Highest(o => o.GameTime);

			if (m != null)
			{
				var source = GlobalTradeContainer.Acquire(d);
				var target = GlobalTradeContainer.Acquire(m);

				if (source != null && target != null)
				{
					source.Relocate(target);
				}
			}
		}
	}
}
