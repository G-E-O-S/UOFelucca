#region Header
//   Vorspire    _,-'/-'/  CharityUI.cs
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
	public class CharityUI : TreeGump
	{
		private static readonly int[] _StockColumns =
		{
			50, // Icon
			-1, // Name
			70, // Stock
			42, // Take
			36 // Reclaim
		};

		private static readonly int[] _CharityColumns =
		{
			50, // Icon
			-1, // Name
			70, // Amount
			42 // Take
		};

		private static readonly int[] _StaffCharityColumns =
		{
			50, // Icon
			-1, // Name
			70, // Amount
			42, // Take
			36 // Cancel
		};

		public static Dictionary<string, Type[]> Categories { get { return Charity.Categories; } }

		public static string GetHelp()
		{
			var h = new StringBuilder();

			h.AppendLine("Welcome to the {0} Charity!", ServerList.ServerName);
			h.AppendLine();
			h.AppendLine("Navigate through the charity offers or your personal stock by using the menu on the left.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Receiving".WrapUOHtmlBold());
			h.AppendLine("You may take items that are listed in the " + "Charity".WrapUOHtmlBold() + " section.");

			if (Charity.CMOptions.AgeLimit > TimeSpan.Zero)
				h.AppendLine($"You may only take items if your account is younger than {Charity.CMOptions.AgeLimit.ToSimpleString(@"!<d\d ><h\h ><m\m ><s\s>")}");
			
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Donating".WrapUOHtmlBold());
			h.AppendLine("Drop items into a " + "Charity Box".WrapUOHtmlBold() + " to add them to your personal stock.");
			h.AppendLine("Reclaim your items at any time using any character on your account.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Profile".WrapUOHtmlBold());
			h.AppendLine("Your profile is account-bound and tracks all of your charity interactions.");
			h.AppendLine("Stats, donations, and takings records can be found in the left-side panel of the footer section.");
			h.AppendLine();
			h.AppendLine(UniGlyph.StarFill + " Management");
			h.AppendLine("Stock management functions are available in the right-side panel of the footer section.");
			h.AppendLine("The bulk functions will confirm and only apply to the items you own on the current page.");
			h.AppendLine();
			h.AppendLine("- Happy Donating! ".WrapUOHtmlRight());

			return h.ToString();
		}

		public static IEnumerable<string> FindCategories(Item item)
		{
			return Charity.FindCategories(item);
		}

		public static bool InCategory(Item item, string category)
		{
			return Charity.InCategory(item, category);
		}

		public static bool HasCategory(Item item)
		{
			return Charity.HasCategory(item);
		}

		public static IEnumerable<string> FindCategories(Type item)
		{
			return Charity.FindCategories(item);
		}

		public static bool InCategory(Type item, string category)
		{
			return Charity.InCategory(item, category);
		}

		public static bool HasCategory(Type item)
		{
			return Charity.HasCategory(item);
		}

		private List<CharityState> _Stock;

		public List<CharityState> Stock { get { return _Stock; } }

		public bool ShowCharityStock { get; set; }

		public bool ViewCharity
		{
			get
			{
				if (SelectedNode != null && !SelectedNode.IsEmpty)
				{
					return SelectedNode.FullName == "Charity" || SelectedNode.IsChildOf("Charity");
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

				if (ViewCharity)
				{
					return _Category = SelectedNode.FullName.Replace("Charity|", String.Empty);
				}

				if (ViewStock)
				{
					return _Category = SelectedNode.FullName.Replace("Stock|", String.Empty);
				}

				return _Category = String.Empty;
			}
			set { _Category = value; }
		}

		public int CharityPages { get; protected set; }
		public int CharityPage { get; protected set; }

		public int StockPages { get; protected set; }
		public int StockPage { get; protected set; }

		public int NodePage
		{
			get
			{
				if (ViewCharity)
				{
					return CharityPage;
				}

				if (ViewStock)
				{
					return StockPage;
				}

				return 0;
			}
			set
			{
				if (ViewCharity)
				{
					CharityPage = value;
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
				if (ViewCharity)
				{
					return CharityPages;
				}

				if (ViewStock)
				{
					return StockPages;
				}

				return 0;
			}
			set
			{
				if (ViewCharity)
				{
					CharityPages = value;
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

		public CharityUI(Mobile user)
			: base(user)
		{
			Title = "Charity";

			Width = 800;
			Height = 450;
			FooterHeight = 200;

			CharityPage = StockPage = 0;
			CharityPages = StockPages = 1;

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
			if (!Charity.CMOptions.ModuleEnabled)
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
			list["Charity"] = RenderCharityNode;

			foreach (var o in Categories.Keys.Select(o => (TreeGumpNode)o))
			{
				list["Stock|" + o.FullName] = RenderStockNode;
				list["Charity|" + o.FullName] = RenderCharityNode;

				foreach (var p in o.GetParents())
				{
					list["Stock|" + p.FullName] = RenderStockNode;
					list["Charity|" + p.FullName] = RenderCharityNode;
				}
			}

			base.CompileNodes(list);
		}

		protected override void CompileList(List<TreeGumpNode> list)
		{
			base.CompileList(list);

			Stock.Clear();

			if (!ViewCharity && !ViewStock)
			{
				return;
			}

			var states = Charity.Items.Values.Where(s => s != null && s.IsValid);

			if (ViewStock)
			{
				states = states.Where(s => User.CheckAccount(s.Giver));
			}
			else if (!ShowCharityStock)
			{
				states = states.Where(s => !User.CheckAccount(s.Giver));
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

			var tabs = new[] {"Stats", "Given", "Taken"};

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

			if (ViewCharity)
			{
				glyph = ShowCharityStock ? UniGlyph.CircleX : UniGlyph.CircleDot;
				color = ShowCharityStock ? Color.IndianRed : Color.LawnGreen;
				text = String.Format("{0} My Stock {1}", ShowCharityStock ? "Hide" : "Show", glyph);
				text = text.WrapUOHtmlRight();

				AddHtmlButton(bounds.X, bounds.Y, bounds.Width, 20, b => ToggleShowStock(), text, color, Color.Black);

				bounds.Y += 20;
				bounds.Height -= 20;
			}

			if (ViewStock || User.AccessLevel >= Charity.Access)
			{
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

			var p = Charity.FindProfile(User);

			if (p != null)
			{
				List<CharityRecord> list = null;

				switch (tab)
				{
					case "Stats":
					{
						if (Charity.CMOptions.StockLimit > 0)
						{
							stats.AppendLine("Stock: {0:#,0} / {1:#,0}", p.Stock.Count, Charity.CMOptions.StockLimit);
						}
						else
						{
							stats.AppendLine("Stock: {0:#,0}", p.Stock.Count);
						}
					}
						break;
					case "Given":
					{
						ObjectPool.Acquire(out list);

						list.AddRange(p.GetGiven());
					}
						goto case "*";
					case "Taken":
					{
						ObjectPool.Acquire(out list);

						list.AddRange(p.GetReceived());
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
							
							var limit = Math.Min(50, list.Count);

							stats.AppendLine("Most Recent {0:#,0}...".WrapUOHtmlSmall(), limit);
							stats.AppendLine();

							var format = "{0} {1}";

							foreach (var o in list.OrderByDescending(o => o.Date).Take(limit))
							{
								stats.AppendLine(format, o.ItemName.ToUpperWords(), o.Amount);
							}
						}
					}
						break;
				}

				stats.AppendLine();
			}
			else
			{
				stats.AppendLine("You have no charity profile.");
			}

			var text = stats.ToString();

			text = text.WrapUOHtmlColor(HtmlColor, false);

			AddHtml(x, y, w, h, text, false, true);
		}

		public override void InvalidatePageCount()
		{
			base.InvalidatePageCount();

			if (!ViewCharity && !ViewStock)
			{
				return;
			}

			var count = Stock.Count;

			var limit = ((Height - 55 - _ControlPanelHeight) / RowHeight) - 1;

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

		protected virtual void RenderCharityNode(Rectangle bounds, int index, TreeGumpNode node)
		{
			RenderStockTable(bounds);
		}

		protected virtual void RenderStockTable(Rectangle bounds)
		{
			var limit = ((bounds.Height - _ControlPanelHeight) / RowHeight) - 1;
			var stock = Stock.Skip(NodePage * limit).Take(limit);
			var cols = ViewStock ? _StockColumns : User.AccessLevel >= Charity.Access ? _StaffCharityColumns : _CharityColumns;

			AddTable(bounds, true, cols, stock, RowHeight, Color.Empty, 1, RenderState);

			AddControlPanel(bounds.X, bounds.Y + (bounds.Height - _ControlPanelHeight), bounds.Width);
		}

		protected virtual void RenderState(int x, int y, int w, int h, CharityState state, int row, int col)
		{
			var fgCol = row % 2 == 0 ? Color.WhiteSmoke : Color.PaleGoldenrod;
			var bgCol = row % 2 == 0 ? Color.Black : Color.DarkSlateGray;

			if (col > 0 && col < 3)
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
					case 3: // Take
						AddHtml(x, y, w, h, "Take", fgCol, bgCol);
						break;
					case 4: // Reclaim | Cancel
					{
						if (ViewStock)
						{
							AddHtml(x, y, w, h, "Claim", fgCol, bgCol);
						}
						else if (User.AccessLevel >= Charity.Access)
						{
							AddHtml(x, y, w, h, "Cancel", fgCol, bgCol);
						}
					}
						break;
				}
			}
			else if (state != null && state.IsValid)
			{
				if (col > 0 && col < 3)
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
					case 3: // Take
                    {
#if ServUOX
						if (!User.CheckAccount(state.Giver) && (Charity.CMOptions.AgeLimit <= TimeSpan.Zero || User.Account.Age <= Charity.CMOptions.AgeLimit))
#else
						if (!User.CheckAccount(state.Giver) && (Charity.CMOptions.AgeLimit <= TimeSpan.Zero || (User.Account is Accounting.Account acc && DateTime.UtcNow - acc.Created <= Charity.CMOptions.AgeLimit)))
#endif
						{
							AddButton(x, y, 4037, 4036, b => Take(state, -1));
						}
						else
						{
							AddImage(x, y, 4037, 900);
						}
					}
						break;
					case 4: // Reclaim | Cancel (Staff)
					{
						if (ViewStock && User.CheckAccount(state.Giver))
						{
							AddButton(x, ComputeCenter(y, h) - 11, 4018, 4019, b => Reclaim(state, true));
						}
						else if (ViewCharity && User.AccessLevel >= Charity.Access)
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
			ShowCharityStock = !ShowCharityStock;

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
					Html = "Reclaim " + "all stock on this page".WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " from charity?" 
						 + (User.AccessLevel >= Charity.Access ? "\n<b>This will include items donated by other players!<b>".WrapUOHtmlColor(Color.IndianRed, Color.WhiteSmoke) : String.Empty) 
						 + "\n\nClick OK to confirm and withdraw your items...",
					AcceptHandler = b => ReclaimAll(false),
					CancelHandler = Refresh
				}.Send();

				return;
			}

			var limit = ((Height - 55 - _ControlPanelHeight) / RowHeight) - 1;
			var stock = Stock.Skip(StockPage * limit).Take(limit);

			if (User.AccessLevel < Charity.Access)
			{
				stock = stock.Where(o => User.CheckAccount(o.Giver));
			}

			foreach (var o in stock)
			{
				o.TryReclaim(User);
			}
		}
		
		protected virtual void Reclaim(CharityState state, bool confirm)
		{
			if (state == null || !state.IsValid)
			{
				Refresh(true);
				return;
			}

			if (confirm)
			{
				new ConfirmDialogGump(User, this)
				{
					IconItem = true,
					Icon = state.ItemID,
					IconHue = state.Hue,
					IconTooltip = state.Item.Serial,
					HtmlColor = Color.WhiteSmoke,
					Title = "Reclaim " + state.Name,
					Html = "Reclaim " + state.Name.WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + " from charity?\n\nClick OK to confirm and withdraw your item...",
					AcceptHandler = b => Reclaim(state, false),
					CancelHandler = Refresh
				}.Send();

				return;
			}

			if (state.TryReclaim(User))
			{
				User.SendMessage("{0} has been withdrawn from charity.", state.Name);
			}

			Refresh(true);
		}

		protected virtual void Take(CharityState state, int amount)
		{
			if (state == null || !state.IsValid)
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
						Title = "Take " + state.Name,
						Html = "Please type the amount you wish to take and click OK to continue...\n\n" 
							 + ("Maximum Amount: " + state.Amount.ToString("#,0")).WrapUOHtmlColor(Color.OrangeRed, Color.WhiteSmoke),
						InputText = state.Amount.ToString(),
						Callback = (b, t) =>
						{
							if (Int32.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
							{
								Take(state, amount);
							}
							else
							{
								Take(state, -1);
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
				new ConfirmDialogGump(User, this)
				{
					IconItem = true,
					Icon = state.ItemID,
					IconHue = state.Hue,
					IconTooltip = state.Item.Serial,
					HtmlColor = Color.WhiteSmoke,
					Title = "Take " + state.Name,
					Html = "Take " + String.Format("{0:#,0} {1}", amount, state.Name).WrapUOHtmlColor(Color.Yellow, Color.WhiteSmoke) + "?\n\nClick OK to confirm and claim your items...",
					AcceptHandler = b =>
					{
						if (state.Take(User, amount))
						{
                            OnTake(state, amount);
						}
						else
						{
                            OnTakeFail(state, amount);
						}
					},
					CancelHandler = b => Take(state, -1)
				}.Send();

				return;
			}

			Refresh(true);
		}

		protected virtual void OnTake(CharityState state, int amount)
		{
			Refresh(true);
		}

		protected virtual void OnTakeFail(CharityState state, int amount)
		{
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

		protected virtual object GetSortKey(CharityState state)
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
				case 3: // Take
                case 4: // Reclaim
					return state.Amount;
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
