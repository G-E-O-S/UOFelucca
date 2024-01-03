#region Header
//   Vorspire    _,-'/-'/  GlobalTradeUI.cs
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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Server.Gumps;
using Server.Misc;

using VitaNex.Collections;
using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;
using VitaNex.Text;
#endregion

namespace Server.Items
{
	public class GlobalTradeUI : TreeGump
	{
		private static readonly int[] _StockColumns =
		{
			50, // Icon
			-1, // Name
			70, // Stock
			100, // Price
			42, // Trading
			36 // Reclaim
		};

		private static readonly int[] _MarketColumns =
		{
			50, // Icon
			-1, // Name
			70, // Amount
			100, // Price
			42 // Purchase
		};

		private static readonly int[] _StaffMarketColumns =
		{
			50, // Icon
			-1, // Name
			70, // Amount
			100, // Price
			42, // Purchase
			36 // Cancel
		};

		public static Dictionary<string, Type[]> Categories { get { return GlobalTrade.Categories; } }

		public static string GetHelp()
		{
			var h = new StringBuilder();

			h.AppendLine("Welcome to the {0} Global Market!", ServerList.ServerName);
			h.AppendLine();
			h.AppendLine("Navigate through the market or your personal stock by using the menu on the left.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Buying".WrapUOHtmlBold());
			h.AppendLine("You may buy items that are listed for sale in the " + "Market".WrapUOHtmlBold() + " section.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Selling".WrapUOHtmlBold());
			h.AppendLine("Drop items into a " + "Global Trade Box".WrapUOHtmlBold() + " to add them to your personal stock.");
			h.AppendLine("Items added to your personal stock will not be available until you mark them for sale.");
			h.AppendLine("Mark an item for sale by clicking the bag icon in the " + "Sell".WrapUOHtmlBold() + " column.");
			h.AppendLine("Reclaim your items at any time using any character on your account.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Profile".WrapUOHtmlBold());
			h.AppendLine("Your profile is account-bound and tracks all of your market interactions.");
			h.AppendLine("Stats, sales, and purchase records can be found in the left-side panel of the footer section.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Management");
			h.AppendLine("Stock management functions are availabile in the right-side panel of the footer section.");
			h.AppendLine("The bulk functions will confirm and only apply to the items you own on the current page.");
			h.AppendLine();
			h.AppendLine("- Happy Trading! ".WrapUOHtmlRight());

			return h.ToString();
		}

		public static IEnumerable<string> FindCategories(Item item)
		{
			return GlobalTrade.FindCategories(item);
		}

		public static bool InCategory(Item item, string category)
		{
			return GlobalTrade.InCategory(item, category);
		}

		public static bool HasCategory(Item item)
		{
			return GlobalTrade.HasCategory(item);
		}

		public static IEnumerable<string> FindCategories(Type item)
		{
			return GlobalTrade.FindCategories(item);
		}

		public static bool InCategory(Type item, string category)
		{
			return GlobalTrade.InCategory(item, category);
		}

		public static bool HasCategory(Type item)
		{
			return GlobalTrade.HasCategory(item);
		}

		private List<GlobalTradeState> _Stock;

		public List<GlobalTradeState> Stock { get { return _Stock; } }

		public bool ShowMarketStock { get; set; }

		public bool ViewMarket
		{
			get
			{
				if (SelectedNode != null && !SelectedNode.IsEmpty)
				{
					return SelectedNode.FullName == "Market" || SelectedNode.IsChildOf("Market");
				}

				return false;
			}
		}

		public bool ViewStock
		{
			get
			{
				if (SelectedNode != null && !SelectedNode.IsEmpty)
				{
					return SelectedNode.FullName == "Stock" || SelectedNode.IsChildOf("Stock");
				}

				return false;
			}
		}

		private string _Category;

		public string Category
		{
			get
			{
				if (_Category != null)
				{
					return _Category;
				}

				if (ViewMarket)
				{
					return _Category = SelectedNode.FullName.Replace("Market|", String.Empty);
				}

				if (ViewStock)
				{
					return _Category = SelectedNode.FullName.Replace("Stock|", String.Empty);
				}

				return _Category = String.Empty;
			}
			set { _Category = value; }
		}

		public int MarketPages { get; protected set; }
		public int MarketPage { get; protected set; }

		public int StockPages { get; protected set; }
		public int StockPage { get; protected set; }

		public int NodePage
		{
			get
			{
				if (ViewMarket)
				{
					return MarketPage;
				}

				if (ViewStock)
				{
					return StockPage;
				}

				return 0;
			}
			set
			{
				if (ViewMarket)
				{
					MarketPage = value;
				}
				else if (ViewStock)
				{
					StockPage = value;
				}
			}
		}

		public int NodePages
		{
			get
			{
				if (ViewMarket)
				{
					return MarketPages;
				}

				if (ViewStock)
				{
					return StockPages;
				}

				return 0;
			}
			set
			{
				if (ViewMarket)
				{
					MarketPages = value;
				}
				else if (ViewStock)
				{
					StockPages = value;
				}
			}
		}

		public int RowHeight { get; protected set; }

		public int Order { get; protected set; }

		public string Search { get; set; }

		public int FooterHeight { get; set; }

		public GlobalTradeUI(Mobile user)
			: base(user)
		{
			Title = "Global Market";

			Width = 800;
			Height = 450;
			FooterHeight = 200;
			
			MarketPage = StockPage = 0;
			MarketPages = StockPages = 1;

			RowHeight = 36;

			CanClose = true;
			CanDispose = true;
			CanMove = true;
			CanResize = true;
		}

		public override void AssignCollections()
		{
			base.AssignCollections();

			if (_Stock == null)
			{
				ObjectPool.Acquire(out _Stock);
			}
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();

			if (_Stock != null)
			{
				ObjectPool.Free(ref _Stock);
			}
		}

		protected override bool OnBeforeSend()
		{
			if (!GlobalTrade.CMOptions.ModuleEnabled || GlobalTrade.Currency == null)
			{
				return false;
			}

			return base.OnBeforeSend();
		}

		protected override void MainButtonHandler(GumpButton b)
		{
			SelectedNode = String.Empty;

			base.MainButtonHandler(b);
		}

		protected override void OnSelected(TreeGumpNode oldNode, TreeGumpNode newNode)
		{
			NodePage = 0;
			NodePages = 1;

			base.OnSelected(oldNode, newNode);
		}

		protected override void Compile()
		{
			Category = null;

			base.Compile();
		}

		protected override void CompileNodes(Dictionary<TreeGumpNode, Action<Rectangle, int, TreeGumpNode>> list)
		{
			list["Stock"] = RenderStockNode;
			list["Market"] = RenderMarketNode;

			foreach (var o in Categories.Keys.Select(o => (TreeGumpNode)o))
			{
				list["Stock|" + o.FullName] = RenderStockNode;
				list["Market|" + o.FullName] = RenderMarketNode;

				foreach (var p in o.GetParents())
				{
					list["Stock|" + p.FullName] = RenderStockNode;
					list["Market|" + p.FullName] = RenderMarketNode;
				}
			}

			base.CompileNodes(list);
		}

		protected override void CompileList(List<TreeGumpNode> list)
		{
			base.CompileList(list);

			Stock.Clear();

			if (!ViewMarket && !ViewStock)
			{
				return;
			}

			var states = GlobalTrade.Items.Values.Where(s => s != null && s.IsValid);

			if (ViewStock)
			{
				states = states.Where(s => User.CheckAccount(s.Seller));
			}
			else
			{
				if (User.AccessLevel < GlobalTrade.Access)
				{
					states = states.Where(s => s.Trading);
				}

				if (!ShowMarketStock)
				{
					states = states.Where(s => !User.CheckAccount(s.Seller));
				}
			}

			if (!SelectedNode.IsRoot)
			{
				states = states.Where(s => InCategory(s.Item, Category));
			}

			if (!String.IsNullOrWhiteSpace(Search))
			{
				states = states.Where(s => Regex.IsMatch(s.Name, Search, RegexOptions.IgnoreCase));
			}

			states = Order < 0 ? states.OrderByDescending(GetSortKey) : states.OrderBy(GetSortKey);

			Stock.AddRange(states);
		}

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			layout.Replace("body/mainbutton", () => AddButton(102, 10, 5609, 5610, MainButtonHandler));

			layout.Add("footer/bg", () => RenderFooter(new Rectangle(0, 43 + Height, Width, FooterHeight)));
		}

		protected virtual void RenderFooter(Rectangle bounds)
		{
			var sup = SupportsUltimaStore;
			var ec = IsEnhancedClient;
			var bgID = ec ? 83 : sup ? 40000 : 9270;

			AddBackground(bounds.X, bounds.Y, bounds.Width, bounds.Height, 9260);

			bounds.X += 15;
			bounds.Y += 15;
			bounds.Width -= 30;
			bounds.Height -= 30;

			AddBackground(bounds.X, bounds.Y, bounds.Width, bounds.Height, bgID);

			bounds.X += 10;
			bounds.Y += 10;
			bounds.Width -= 20;
			bounds.Height -= 20;

			if (!ec)
			{
				AddImageTiled(bounds.X, bounds.Y, bounds.Width, bounds.Height, 2624);
			}

			var cache = bounds;
			
			bounds.Width /= 2;

			var tabs = new[] {"Stats", "Sales", "Purchases"};

			var ty = bounds.Y;
			var tc = Color.WhiteSmoke;
			var ts = Color.Gold;

			AddTabs(bounds, 2, -1, -1, -1, tc, ts, "Stats", tabs, (x, y, w, h, t) => RenderTab(x, y, w, h, y - ty, t));
			
			bounds = cache;

			bounds.X += (bounds.Width / 2) + 5;
			bounds.Width -= (bounds.Width / 2) + 5;

			char glyph;
			Color color;
			string text;

			if (ViewMarket)
			{
				glyph = ShowMarketStock ? UniGlyph.CircleX : UniGlyph.CircleDot;
				color = ShowMarketStock ? Color.IndianRed : Color.LawnGreen;
				text = String.Format("{0} My Stock {1}", ShowMarketStock ? "Hide" : "Show", glyph);
				text = text.WrapUOHtmlRight();

				AddHtmlButton(bounds.X, bounds.Y, bounds.Width, 20, b => ToggleShowStock(), text, color, Color.Black);

				bounds.Y += 20;
				bounds.Height -= 20;
			}

			if (ViewStock || User.AccessLevel >= GlobalTrade.Access)
			{
				glyph = UniGlyph.StarFill;
				color = Color.SkyBlue;
				text = String.Format("Set All Public {0}", glyph);
				text = text.WrapUOHtmlRight();

				AddHtmlButton(bounds.X, bounds.Y, bounds.Width, 20, b => SetAllTrading(true, true), text, color, Color.Black);

				bounds.Y += 20;
				bounds.Height -= 20;

				glyph = UniGlyph.StarEmpty;
				color = Color.Yellow;
				text = String.Format("Set All Private {0}", glyph);
				text = text.WrapUOHtmlRight();

				AddHtmlButton(bounds.X, bounds.Y, bounds.Width, 20, b => SetAllTrading(false, true), text, color, Color.Black);

				bounds.Y += 20;
				bounds.Height -= 20;

				glyph = UniGlyph.StarFill;
				color = Color.Orange;
				text = String.Format("Reclaim All {0}", glyph);
				text = text.WrapUOHtmlRight();

				AddHtmlButton(bounds.X, bounds.Y, bounds.Width, 20, b => ReclaimAll(true), text, color, Color.Black);

				bounds.Y += 20;
				bounds.Height -= 20;
			}
		}

		protected virtual void RenderTab(int x, int y, int w, int h, int hh, string tab)
		{
			AddImageTiled(x + (w - 15), y - hh, 15, hh + h, 256);

			var stats = new StringBuilder();

			var p = GlobalTrade.FindProfile(User);

			if (p != null)
			{
				List<GlobalTradeRecord> list = null;
				string cur = null;

				switch (tab)
				{
					case "Stats":
					{
						if (GlobalTrade.CMOptions.StockLimit > 0)
						{
							stats.AppendLine("Stock: {0:#,0} / {1:#,0}", p.Stock.Count, GlobalTrade.CMOptions.StockLimit);
						}
						else
						{
							stats.AppendLine("Stock: {0:#,0}", p.Stock.Count);
						}

						if (p.Stock.Count > 0)
						{
							stats.AppendLine();

							foreach (var g in p.Stock.ToLookup(o => o.Currency.GetTypeName(false).ToUpperWords()))
							{
								stats.AppendLine("{0} Worth: {1:#,0}", g.Key, g.Aggregate(0L, (v, o) => v + o.Total));
							}
						}
					}
						break;
					case "Sales":
					{
						ObjectPool.Acquire(out list);

						list.AddRange(p.GetSales());

						cur = "Earned";
					}
						goto case "*";
					case "Purchases":
					{
						ObjectPool.Acquire(out list);

						list.AddRange(p.GetPurchases());

						cur = "Spent";
					}
						goto case "*";
					case "*":
					{
						if (list == null)
						{
							ObjectPool.Acquire(out list);

							list.AddRange(p.Records);
						}

						stats.AppendLine("{0}: {1:#,0}", tab, list.Count);

						if (list.Count > 0)
						{
							stats.AppendLine();

							var format = "{0} " + cur + ": {1:#,0}";

							foreach (var g in list.ToLookup(o => o.Currency.ToUpperWords()))
							{
								stats.AppendLine(format, g.Key, g.Aggregate(0L, (v, o) => v + o.Total));
							}

							stats.AppendLine();

							var limit = Math.Min(50, list.Count);

							stats.AppendLine("Most Recent {0:#,0}...".WrapUOHtmlSmall(), limit);
							stats.AppendLine();

							format = "{0} {1} {2:#,0} {3}";

							foreach (var o in list.OrderByDescending(o => o.Date).Take(limit))
							{
								stats.AppendLine(format, o.ItemName.ToUpperWords(), o.Amount, o.Total, o.Currency.ToUpperWords());
							}
						}
					}
						break;
				}

				stats.AppendLine();
			}
			else
			{
				stats.AppendLine("You have no global trade profile.");
				stats.AppendLine("Buy or sell something to get your business started!");
			}

			var text = stats.ToString();

			text = text.WrapUOHtmlColor(HtmlColor, false);

			AddHtml(x, y, w, h, text, false, true);
		}

		public override void InvalidatePageCount()
		{
			base.InvalidatePageCount();

			if (!ViewMarket && !ViewStock)
			{
				return;
			}

			var count = Stock.Count;

			var limit = (((Height - 55) - _ControlPanelHeight) / RowHeight) - 1;

			if (count > limit)
			{
				if (limit > 0)
				{
					NodePages = (int)Math.Ceiling(count / (double)limit);
					NodePages = Math.Max(1, NodePages);
				}
				else
				{
					NodePages = 1;
				}
			}
			else
			{
				NodePages = 1;
			}

			NodePage = Math.Max(0, Math.Min(NodePages - 1, NodePage));
		}

		protected virtual void RenderStockNode(Rectangle bounds, int index, TreeGumpNode node)
		{
			RenderStockTable(bounds);
		}

		protected virtual void RenderMarketNode(Rectangle bounds, int index, TreeGumpNode node)
		{
			RenderStockTable(bounds);
		}
		
		protected virtual void RenderStockTable(Rectangle bounds)
		{
			var limit = ((bounds.Height - _ControlPanelHeight) / RowHeight) - 1;
			var stock = Stock.Skip(NodePage * limit).Take(limit);
			var cols = ViewStock ? _StockColumns : User.AccessLevel >= GlobalTrade.Access ? _StaffMarketColumns : _MarketColumns;

			AddTable(bounds, true, cols, stock, RowHeight, Color.Empty, 1, RenderState);

			AddControlPanel(bounds.X, bounds.Y + (bounds.Height - _ControlPanelHeight), bounds.Width);
		}

		protected virtual void RenderState(int x, int y, int w, int h, GlobalTradeState state, int row, int col)
		{
			var fgCol = row % 2 == 0 ? Color.WhiteSmoke : Color.PaleGoldenrod;
			var bgCol = row % 2 == 0 ? Color.Black : Color.DarkSlateGray;

			if (col > 0 && col < 4)
			{
				AddRectangle(x, y, w, h, bgCol, true);
			}
			else
			{
				bgCol = Color.Empty;
			}

			if (row < 0)
			{
				x += 3;
				w -= 6;

				string label = null;

				switch (col)
				{
					case -1: // Sorted Column
					{
						if (Math.Abs(Order) == col)
						{
							var tri = Order < 0 ? UniGlyph.TriDownFill : UniGlyph.TriUpFill;

							label = String.Concat(label, " ", tri.ToString().WrapUOHtmlSmall());
						}

						AddHtmlButton(x, y, w, h, b => SortBy(col), label, fgCol, bgCol);
					}
						break;
					case 0: // Icon
						break;
					case 1: // Name
						label = "Name";
						goto case -1;
					case 2: // Amount
						label = "Stock";
						goto case -1;
					case 3: // Price
						label = "Price";
						goto case -1;
					case 4: // Purchase | Trading
					{
						if (ViewStock)
						{
							AddHtml(x, y, w, h, "Sell", fgCol, bgCol);
						}
						else if (User.AccessLevel >= GlobalTrade.Access)
						{
							AddHtml(x, y, w, h, "Toggle", fgCol, bgCol);
						}
						else
						{
							AddHtml(x, y, w, h, "Buy", fgCol, bgCol);
						}
					}
						break;
					case 5: // Reclaim | Cancel
					{
						if (ViewStock)
						{
							AddHtml(x, y, w, h, "Claim", fgCol, bgCol);
						}
						else if (User.AccessLevel >= GlobalTrade.Access)
						{
							AddHtml(x, y, w, h, "Cancel", fgCol, bgCol);
						}
					}
						break;
				}
			}
			else if (state != null && state.IsValid)
			{
				if (col > 0 && col < 4)
				{
					x += 3;
					w -= 6;
				}

				switch (col)
				{
					case 0: // Icon
					{
						var o = GetItemOffset(state.ItemID);

						if (state.Hue > 0)
						{
							AddItem(x + o.X, y + o.Y, state.ItemID, state.Hue);
						}
						else
						{
							AddItem(x + o.X, y + o.Y, state.ItemID);
						}

						AddProperties(state.Item);
					}
						break;
					case 1: // Name
					{
						var name = state.Name.ToUpperWords();

						if (state.Charges > 0)
						{
							name += String.Format(" ({0:#,0} uses)", state.Charges).WrapUOHtmlSmall();
						}
						else if (state.Quantity > 0)
						{
							name += String.Format(" ({0:#,0} qty)", state.Quantity).WrapUOHtmlSmall();
						}

						AddHtml(x, y, w, h, name, Color.Yellow, bgCol);
						AddProperties(state.Item);
					}
						break;
					case 2: // Amount
					{
						AddHtml(x, y, w, h, state.Amount.ToString("#,0"), fgCol, bgCol);
						AddProperties(state.Item);
					}
						break;
					case 3: // Price
					{
						if (ViewStock && !state.Trading)
						{
							AddHtmlButton(x, y, w, h, b => UpdatePrice(state), state.Value.ToString("#,0"), fgCol, bgCol);
						}
						else
						{
							AddHtml(x, y, w, h, state.Value.ToString("#,0"), fgCol, bgCol);
						}
					}
						break;
					case 4: // Purchase | Trading
					{
						if (ViewStock || User.AccessLevel >= GlobalTrade.Access)
						{
							var onID = state.Trading ? 4036 : 4037;
							var offID = state.Trading ? 4037 : 4036;
							var hue = state.Trading ? 0x55 : 0x22;

							AddInputEC();

							AddButton(x, y, onID, offID, b => ToggleTrading(state));
							AddImage(x, y, onID, hue);

							AddInputEC();
						}
						else if (state.Trading && !User.CheckAccount(state.Seller))
						{
							AddButton(x, y, 4037, 4036, b => Purchase(state, -1));
						}
						else
						{
							AddImage(x, y, 4037, 900);
						}
					}
						break;
					case 5: // Reclaim | Cancel (Staff)
					{
						if (ViewStock && User.CheckAccount(state.Seller))
						{
							AddButton(x, ComputeCenter(y, h) - 11, 4018, 4019, b => Reclaim(state, true));
						}
						else if (ViewMarket && User.AccessLevel >= GlobalTrade.Access)
						{
							AddButton(x, ComputeCenter(y, h) - 11, 4018, 4019, b => Reclaim(state, true));
						}
					}
						break;
				}
			}
		}

		protected override void CompileEmptyNodeLayout(SuperGumpLayout layout, int x, int y, int w, int h, int index, TreeGumpNode node)
		{
			layout.Add("empty/" + index, () => AddHtml(x, y, w, h, GetHelp().WrapUOHtmlColor(HtmlColor, false), false, true));
		}

		protected virtual void PreviousNodePage()
		{
			--NodePage;

			Refresh(true);
		}

		protected virtual void NextNodePage()
		{
			++NodePage;

			Refresh(true);
		}

		protected virtual void ToggleShowStock()
		{
			ShowMarketStock = !ShowMarketStock;

			Refresh(true);
		}

		protected virtual void SetAllTrading(bool value, bool confirm)
		{
			if (confirm)
			{
				var token = value ? "Public" : "Private";

				new ConfirmDialogGump(User, this)
				{
					HtmlColor = Color.WhiteSmoke,
					Title = "Set All Stock " + token,
					Html = "Set " + "all stock on this page".WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " to " + token.WrapUOHtmlColor(value ? Color.LawnGreen : Color.IndianRed, Color.WhiteSmoke) + "?" 
						 + (User.AccessLevel >= GlobalTrade.Access ? "\n<b>This will include items sold by other players!<b>".WrapUOHtmlColor(Color.IndianRed, Color.WhiteSmoke) : String.Empty) 
						 + "\n\nClick OK to confirm and update your items...",
					AcceptHandler = b => SetAllTrading(value, false),
					CancelHandler = Refresh
				}.Send();

				return;
			}

			var limit = ((Height - 55 - _ControlPanelHeight) / RowHeight) - 1;
			var stock = Stock.Skip(StockPage * limit).Take(limit);

			if (User.AccessLevel < GlobalTrade.Access)
			{
				stock = stock.Where(o => User.CheckAccount(o.Seller));
			}

			foreach (var o in stock)
			{
				o.Trading = value;
			}

			Refresh(true);
		}

		protected virtual void ReclaimAll(bool confirm)
		{
			if (confirm)
			{
				new ConfirmDialogGump(User, this)
				{
					HtmlColor = Color.WhiteSmoke,
					Title = "Reclaim All Stock",
					Html = "Reclaim " + "all stock on this page".WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " from the global market?" 
						 + (User.AccessLevel >= GlobalTrade.Access ? "\n<b>This will include items sold by other players!<b>".WrapUOHtmlColor(Color.IndianRed, Color.WhiteSmoke) : String.Empty) 
						 + "\n\nClick OK to confirm and withdraw your items...",
					AcceptHandler = b => ReclaimAll(false),
					CancelHandler = Refresh
				}.Send();

				return;
			}

			var limit = ((Height - 55 - _ControlPanelHeight) / RowHeight) - 1;
			var stock = Stock.Skip(StockPage * limit).Take(limit);

			if (User.AccessLevel < GlobalTrade.Access)
			{
				stock = stock.Where(o => User.CheckAccount(o.Seller));
			}

			foreach (var o in stock)
			{
				o.TryReclaim(User);
			}
		}

		protected virtual void ToggleTrading(GlobalTradeState state)
		{
			if (state != null && state.IsValid)
			{
				state.Trading = !state.Trading;
			}

			Refresh(true);
		}

		protected virtual void Reclaim(GlobalTradeState state, bool confirm)
		{
			if (state == null || !state.IsValid)
			{
				Refresh(true);
				return;
			}

			if (confirm)
			{
				var trading = state.Trading;

				state.Trading = false;

				new ConfirmDialogGump(User, this)
				{
					IconItem = true,
					Icon = state.ItemID,
					IconHue = state.Hue,
					IconTooltip = state.Item.Serial,
					HtmlColor = Color.WhiteSmoke,
					Title = "Reclaim " + state.Name,
					Html = "Reclaim " + state.Name.WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " from the global market?\n\nClick OK to confirm and withdraw your item...",
					AcceptHandler = b => Reclaim(state, false),
					CancelHandler = b =>
					{
						state.Trading = trading;

						Refresh(true);
					}
				}.Send();

				return;
			}

			if (state.TryReclaim(User))
			{
				User.SendMessage("{0} has been withdrawn from the global market.", state.Name);
			}

			Refresh(true);
		}

		protected virtual void Purchase(GlobalTradeState state, int amount)
		{
			if (state == null || !state.IsValid || !state.Trading)
			{
				Refresh(true);
				return;
			}

			if (amount < 0)
			{
				if (state.Stackable && state.Amount > 1)
				{
					new InputDialogGump(User, Refresh())
					{
						IconItem = true,
						Icon = state.ItemID,
						IconHue = state.Hue,
						IconTooltip = state.Item.Serial,
						HtmlColor = Color.WhiteSmoke,
						Title = "Purchase " + state.Name,
						Html = "Please type the amount you wish to purchase and click OK to continue...\n\n" 
							 + ("Maximum Amount: " + state.Amount.ToString("#,0")).WrapUOHtmlColor(Color.OrangeRed, Color.WhiteSmoke),
						InputText = state.Amount.ToString(),
						Callback = (b, t) =>
						{
							if (Int32.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
							{
								Purchase(state, amount);
							}
							else
							{
								Purchase(state, -1);
							}
						},
						CancelHandler = Refresh
					}.Send();

					return;
				}

				amount = state.Amount;
			}

			amount = Math.Max(0, Math.Min(state.Amount, amount));

			if (amount > 0)
			{
				var currency = (state.Currency ?? GlobalTrade.CMOptions.Currency.InternalType).GetTypeName(false);

				var total = amount * (long)state.Value;

				new ConfirmDialogGump(User, this)
				{
					IconItem = true,
					Icon = state.ItemID,
					IconHue = state.Hue,
					IconTooltip = state.Item.Serial,
					HtmlColor = Color.WhiteSmoke,
					Title = "Purchase " + state.Name,
					Html = "Purchase " + String.Format("{0:#,0} {1}", amount, state.Name).WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) 
						 + " for " + String.Format("{0:#,0} {1}", total, currency).WrapUOHtmlColor(Color.SkyBlue, Color.WhiteSmoke) + "?" 
						 + "\n\nClick OK to confirm and claim your purchase...",
					AcceptHandler = b =>
					{
						if (total != amount * state.Value)
						{
							Purchase(state, amount);
						}
						else if (state.Purchase(User, amount))
						{
							OnPurchase(state, amount, total);
						}
						else
						{
							OnPurchaseFail(state, amount, total);
						}
					},
					CancelHandler = b => Purchase(state, -1)
				}.Send();

				return;
			}

			Refresh(true);
		}

		protected virtual void OnPurchase(GlobalTradeState state, int amount, long total)
		{
			Refresh(true);
		}

		protected virtual void OnPurchaseFail(GlobalTradeState state, int amount, long total)
		{
			Refresh(true);
		}

		protected virtual void UpdatePrice(GlobalTradeState state)
		{
			if (state == null || !state.IsValid)
			{
				Refresh(true);
				return;
			}

			new InputDialogGump(User, this)
			{
				IconItem = true,
				Icon = state.ItemID,
				IconHue = state.Hue,
				IconTooltip = state.Item.Serial,
				HtmlColor = Color.WhiteSmoke,
				Title = "Price " + state.Name,
				Html = "Please type the amount you wish to sell " + state.Name.WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " for and click OK to confirm...\n" 
					 + "This is the price per single item." + "\n\nMaximum Value: 999,999,999".WrapUOHtmlColor(Color.OrangeRed, Color.WhiteSmoke),
				InputText = state.Value.ToString(),
				Callback = (t, v) => UpdatePrice(state, v),
				CancelHandler = Refresh
			}.Send();
		}

		protected virtual void UpdatePrice(GlobalTradeState state, string price)
		{
			if (state == null || !state.IsValid)
			{
				Refresh(true);
				return;
			}

			price = String.Join(String.Empty, price.Where(Char.IsNumber));

			if (Int32.TryParse(price, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
			{
				state.Value = Math.Max(0, Math.Min(999999999, value));

				state.PushPriceHistory();
			}

			Refresh(true);
		}

		protected virtual void SortBy(int col)
		{
			if (Order == col)
			{
				Order = -col;
			}
			else
			{
				Order = col;
			}

			Refresh(true);
		}

		protected virtual object GetSortKey(GlobalTradeState state)
		{
			if (state == null || !state.IsValid)
			{
				return null;
			}

			switch (Math.Abs(Order))
			{
				case 0: // Icon
				case 1: // Name
					return state.Name;
				case 2: // Amount
					return state.Amount;
				case 3: // Price
				case 4: // Purchase
				case 5: // Reclaim
					return state.Total;
			}

			return state;
		}

		private const int _ControlPanelHeight = 24;

		protected virtual void AddControlPanel(int x, int y, int w)
		{
			AddRectangle(x, y, w, 1, Color.SkyBlue, true);

			y += 2;

			var w2 = w / 2;
			var w4 = w / 4;

			var xx = x;
			var yy = y;

			var pl = (NodePage > 0 ? UniGlyph.TriLeftFill : UniGlyph.TriLeftEmpty) + " PREV";

			pl = pl.WrapUOHtmlCenter();

			if (NodePage > 0)
			{
				AddHtmlButton(xx + 1, yy + 1, w4 - 2, 20, b => PreviousNodePage(), pl, Color.SkyBlue, Color.Black);
			}
			else
			{
				AddHtml(xx + 1, yy + 1, w4 - 2, 20, pl, Color.Gray, Color.Black);
			}

			xx += w4;

			AddSearchBar(xx + 1, yy + 1, w2 - 2);

			xx += w2;

			var nl = "NEXT " + (NodePage < (NodePages - 1) ? UniGlyph.TriRightFill : UniGlyph.TriRightEmpty);

			nl = nl.WrapUOHtmlCenter();

			if (NodePage < NodePages - 1)
			{
				AddHtmlButton(xx + 1, yy + 1, w4 - 2, 20, b => NextNodePage(), nl, Color.SkyBlue, Color.Black);
			}
			else
			{
				AddHtml(xx + 1, yy + 1, w4 - 2, 20, nl, Color.Gray, Color.Black);
			}
		}

		protected virtual void AddSearchBar(int x, int y, int w)
		{
			var col = Color.SkyBlue;
			var bg = Color.Black;

			var t = "Search:";

			var tw = UOFont.Unicode[1].GetWidth(t);

			AddHtml(x, y, tw + 5, 20, t, col, bg);

			x += tw + 5;
			w -= tw + 5;

			AddTextEntryLimited(x, y, w - 40, 20, 1153, Search, 20, (e, s) => Search = s);
			AddRectangle(x, y + 20, w - 40, 1, col, true);

			x += w - 40;
			w = 40;

			col = Color.SkyBlue;
			bg = Color.Black;

			t = UniGlyph.TriRightFill.ToString();

			AddHtmlButton(x, y, w / 2, 20, OnSearch, t, col, bg);

			if (!String.IsNullOrWhiteSpace(Search))
			{
				col = Color.IndianRed;
				bg = Color.Black;

				t = UniGlyph.CircleX.ToString();

				AddHtmlButton(x + (w / 2), y, w / 2, 20, OnClearSearch, t, col, bg);
			}
		}

		private void OnSearch(GumpButton b)
		{
			Refresh(true);
		}

		private void OnClearSearch(GumpButton b)
		{
			Search = String.Empty;

			Refresh(true);
		}
	}
}
