#region Header
//   Vorspire    _,-'/-'/  GlobalTradeBox.cs
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
using System.Drawing;
using System.Linq;

using VitaNex.Network;
using VitaNex.SuperGumps;
#endregion

namespace Server.Items
{
	public class GlobalTradeBox : LargeCrate
	{
		private static bool AddToTradeContainer(Mobile m, Item item)
		{
			var box = GlobalTradeContainer.Acquire(m);

			if (box == null || box.Deleted)
			{
				return false;
			}

			var count = 0;

			try
			{
				return AddTo(m, item, box, true, ref count);
			}
			finally
			{
				if (count > 0)
				{
					foreach (var g in SuperGump.EnumerateInstances<GlobalTradeUI>(m))
					{
						if (g.ViewStock && (g.SelectedNode.IsRoot || GlobalTrade.InCategory(item, g.Category)))
						{
							g.Refresh(true);
						}
					}
				}
			}
		}

		private static bool AddTo(Mobile m, Item item, GlobalTradeContainer box, bool fullMessage, ref int count)
		{
			if (item is Container)
			{
				var i = item.Items.Count;

				while (--i >= 0)
				{
					if (i < item.Items.Count)
					{
						AddTo(m, item.Items[i], box, false, ref count);
					}
				}
			}

			if (item.Items.Count == 0 && box.CheckHold(m, item, fullMessage))
			{
				box.DropItem(item);

				OnAdded(m, item);

				++count;

				return true;
			}

			return false;
		}

		private static void OnAdded(Mobile m, Item item)
		{
			var label = item.ResolveName(m);

			if (item.Amount > 1)
			{
				label = String.Format("{0:#,0} {1}", item.Amount, label);
			}
			else if (item is IHasQuantity)
			{
				label = String.Format("{0} ({1:#,0})", label, ((IHasQuantity)item).Quantity);
			}
			else if (item is IUsesRemaining)
			{
				label = String.Format("{0} ({1:#,0})", label, ((IUsesRemaining)item).UsesRemaining);
			}

			m.SendMessage("{0} was added to your Global Market stock-pile.", label);
		}

		public override string DefaultName { get { return "Global Market Box"; } }

		public override bool DisplaysContent { get { return false; } }

		[Constructable]
		public GlobalTradeBox()
		{ }

		public GlobalTradeBox(Serial serial)
			: base(serial)
		{ }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			using (var opl = new ExtendedOPL(list))
			{
				var total = GlobalTrade.Items.Values.Count(o => o.IsValid && o.Trading);

				opl.Add("{0:#,0} Items".WrapUOHtmlColor(Color.SkyBlue), total);
				opl.Add("Use: Browse the Global Market".WrapUOHtmlColor(Color.LawnGreen));
				opl.Add("Use: Drop items inside to trade them!".WrapUOHtmlColor(Color.LawnGreen));
			}
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (!this.CheckUse(m, true, false, 2))
			{
				return;
			}

			if (GlobalTrade.CMOptions.ModuleEnabled && SuperGump.RefreshInstances<GlobalTradeUI>(m) <= 0)
			{
				new GlobalTradeUI(m).Send();
			}

			DisplayTo(m);
		}

		public override bool TryDropItem(Mobile m, Item item, bool sendFullMessage)
		{
			return AddToTradeContainer(m, item);
		}

		public override bool OnDragDropInto(Mobile m, Item item, Point3D p)
		{
			return AddToTradeContainer(m, item);
		}

		public override bool OnDragDrop(Mobile m, Item item)
		{
			return AddToTradeContainer(m, item);
		}

#if ServUOX
		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, bool checkWeight, int plusItems, int plusWeight)
#else
		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
#endif
		{
			var box = GlobalTradeContainer.Acquire(m);

			return box != null && !box.Deleted && box.CheckHold(m, item, message);
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
