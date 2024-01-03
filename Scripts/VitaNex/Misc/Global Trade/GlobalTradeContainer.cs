#region Header
//   Vorspire    _,-'/-'/  GlobalTradeContainer.cs
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
#endregion

namespace Server.Items
{
	public sealed class GlobalTradeContainer : Container
	{
		public static List<GlobalTradeContainer> Instances { get; private set; }

		static GlobalTradeContainer()
		{
			Instances = new List<GlobalTradeContainer>();
		}

		public static GlobalTradeContainer Acquire(Mobile m)
		{
			if (m == null || m.Deleted)
			{
				return null;
			}

			var box = m.Items.OfType<GlobalTradeContainer>().FirstOrDefault();

			if (box == null || box.Deleted)
			{
				box = new GlobalTradeContainer(m);
			}

			if (m.NetState != null)
			{
				box.SendInfoTo(m.NetState);
			}

			return box;
		}

		private static void AcquireState(Item item, bool pull)
		{
			if (item is Container)
			{
				foreach (var o in item.Items)
				{
					AcquireState(o, pull);
				}
			}

			var state = GlobalTrade.AcquireState(item);

			if (state != null && pull)
			{
				state.PullPriceHistory();
			}

			if (GlobalTrade.CMOptions.ModuleDebug)
			{
				var message = "Create State ({0}) [{1}]";

				GlobalTrade.CMOptions.ToConsole(message, item, state != null ? "Created" : "Ignored");
			}
		}

		private static void DestroyState(Item item, bool push)
		{
			if (item is Container)
			{
				foreach (var o in item.Items)
				{
					DestroyState(o, push);
				}
			}

			if (push)
			{
				var state = GlobalTrade.FindState(item);

				if (state != null)
				{
					state.PushPriceHistory();
				}
			}

			var result = GlobalTrade.DestroyState(item);

			if (GlobalTrade.CMOptions.ModuleDebug)
			{
				var message = "Remove State ({0}) [{1}]";

				GlobalTrade.CMOptions.ToConsole(message, item, result ? "Removed" : "Ignored");
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; private set; }

		public override bool IsVirtualItem { get { return Parent is Mobile; } }

		public override string DefaultName { get { return "Global Trade Container"; } }

		private GlobalTradeContainer(Mobile owner)
			: base(0xE42)
		{
			Owner = owner;

			Movable = false;
			Visible = false;

			Layer = (Layer)127;

			Owner.AddItem(this);

			Instances.Add(this);
		}

		public GlobalTradeContainer(Serial serial)
			: base(serial)
		{
			Instances.Add(this);
		}

		public override void Delete()
		{
			foreach (var item in Items)
			{
				DestroyState(item, false);
			}

			base.Delete();
		}

		public override void OnDelete()
		{
			base.OnDelete();

			Instances.Remove(this);
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Owner = null;

			Instances.Remove(this);
		}

		public override DeathMoveResult OnParentDeath(Mobile parent)
		{
			return DeathMoveResult.RemainEquiped;
		}

		public override bool CheckContentDisplay(Mobile m)
		{
			return m == null || Owner == null || m == Owner || m.AccessLevel > Owner.AccessLevel;
		}

		public override bool IsAccessibleTo(Mobile m)
		{
			return m == null || Owner == null || m == Owner || m.AccessLevel > Owner.AccessLevel;
		}

		public override bool OnDragDrop(Mobile m, Item item)
		{
			return IsAccessibleTo(m) && base.OnDragDrop(m, item);
		}

		public override bool OnDragDropInto(Mobile m, Item item, Point3D p)
		{
			return IsAccessibleTo(m) && base.OnDragDropInto(m, item, p);
		}

#if ServUOX
		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, bool checkWeight, int plusItems, int plusWeight)
#else
		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
#endif
		{
			if (item.BlessedFor != null)
			{
				if (message)
				{
					m.SendMessage("Soulbound items cannot be traded on the global market.");
				}

				return false;
			}

			if (item.Insured)
			{
				if (message)
				{
					m.SendMessage("Insured items cannot be traded on the global market.");
				}

				return false;
			}

			if (item.QuestItem)
			{
				if (message)
				{
					m.SendMessage("Quest items cannot be traded on the global market.");
				}

				return false;
			}

			if (!GlobalTrade.HasCategory(item))
			{
				if (message)
				{
					m.SendMessage("That item can not be traded on the global market.");
				}

				return false;
			}
			
			if (item is Container && item.Items.Count > 0)
			{
				if (message)
				{
					m.SendMessage("That container must be empty before trading it on the global market.");
				}

				return false;
			}

			var limit = GlobalTrade.CMOptions.StockLimit;

			if (limit > 0 && Items.Count >= limit)
			{
				if (message)
				{
					m.SendMessage("You have reached your global market stock limit of {0:#,0} items.", limit);
				}

				return false;
			}

			return true;
		}

		private bool _Moving;

		public bool Relocate(GlobalTradeContainer to)
		{
			if (to == null || to.Deleted || to == this)
			{
				return false;
			}

			if (Items.Count > 0)
			{
				_Moving = to._Moving = true;

				Items.ForEachReverse(o =>
				{
					to.DropItem(o);

					var state = GlobalTrade.Items.GetValue(o);

					if (state != null)
					{
						state.Container = to;
					}
				});

				_Moving = to._Moving = false;
			}

			return true;
		}

		public override void OnItemAdded(Item item)
		{
			base.OnItemAdded(item);

			if (!_Moving)
			{
				AcquireState(item, true);
			}
		}

		public override void OnSubItemAdded(Item item)
		{
			base.OnSubItemAdded(item);

			if (!_Moving)
			{
				AcquireState(item, true);
			}
		}

		public override void OnItemRemoved(Item item)
		{
			base.OnItemRemoved(item);

			if (!_Moving)
			{
				DestroyState(item, true);
			}
		}

		public override void OnSubItemRemoved(Item item)
		{
			base.OnSubItemRemoved(item);

			if (!_Moving)
			{
				DestroyState(item, true);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();

			Owner = reader.ReadMobile();

			if (Owner == null || Owner.Deleted)
			{
				Timer.DelayCall(Delete);
			}
		}
	}
}
