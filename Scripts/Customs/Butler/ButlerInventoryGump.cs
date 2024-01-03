using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;

namespace Server.Gumps
{
	public class ButlerInventoryGump : Gump
    {
		private readonly Butler m_Butler;
		private readonly Mobile m_From;
		private int m_Preset;
		
		public ButlerInventoryGump(Mobile from, Butler butler, int preset)
			: base(50, 40)
		{
			m_Butler = butler;
			m_From = from;
			m_Preset = preset;
			
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			
			AddPage(0);
			AddBackground(0, 0, 450, 515, 9200);
			AddLabel(175, 30, 0, @"Butler's Inventory");
			AddLabel(35, 60, 0, @"Restock Preset " + preset + " :");
			
			LoadInventory();
		}
		
		public override void OnResponse(NetState sender, RelayInfo info)
        {			
			if( info.ButtonID == 0 
				|| m_Butler == null 
				|| m_Butler.Deleted
				|| m_Butler.m_ButlerDeed == null 
				|| m_Butler.m_ButlerDeed.Deleted ) 
			{
				return;
			}
			
			if( !m_From.InRange(m_Butler, 32) )
			{
				m_Butler.SayTo(m_From, "You're too far away!");
				return;
			}
			
			if ( info.ButtonID >= 1 && info.ButtonID <= 8 ) // Go to Restock Preset
			{
				m_From.SendGump(new ButlerInventoryGump(m_From, m_Butler, info.ButtonID));
			}
			else if (info.ButtonID == 9) // Save Preset
			{
				SavePreset(info);
				m_From.SendGump(new ButlerInventoryGump(m_From, m_Butler, m_Preset));
			}
			else if (info.ButtonID == 10) // Restock Now
			{
				SavePreset(info);
				m_Butler.RefillFromStock(m_Preset, m_From);
				m_From.SendGump(new ButlerInventoryGump(m_From, m_Butler, m_Preset));
			}
		}
		
		private void SavePreset(RelayInfo info)
		{
			SavePresetValue(InventoryType.MandrakeRoot, info, 1);
			SavePresetValue(InventoryType.Bloodmoss, info, 2);
			SavePresetValue(InventoryType.BlackPearl, info, 3);
			SavePresetValue(InventoryType.Garlic, info, 4);
			SavePresetValue(InventoryType.Ginseng, info, 5);
			SavePresetValue(InventoryType.Nightshade, info, 6);
			SavePresetValue(InventoryType.SulfurousAsh, info, 7);
			SavePresetValue(InventoryType.SpidersSilk, info, 8);
			
			SavePresetValue(InventoryType.GreaterExplosionPotion, info, 9);
			SavePresetValue(InventoryType.GreaterCurePotion, info, 10);
			SavePresetValue(InventoryType.GreaterHealPotion, info, 11);
			SavePresetValue(InventoryType.TotalRefreshPotion, info, 12);
			SavePresetValue(InventoryType.GreaterStrengthPotion, info, 13);
			SavePresetValue(InventoryType.GreaterAgilityPotion, info, 14);
			SavePresetValue(InventoryType.DeadlyPoisonPotion, info, 15);
			
			SavePresetValue(InventoryType.Bolt, info, 16);
			SavePresetValue(InventoryType.Arrow, info, 17);
			
			SavePresetValue(InventoryType.Bandage, info, 18);
		}
		
		private void SavePresetValue(InventoryType type, RelayInfo info, int textID)
		{
			String AmountText = info.GetTextEntry(textID).Text;
			if (Int32.TryParse(AmountText, out int Amount))
			{
				m_Butler.SetRestockPresetValue(m_Preset, type, Amount);
			}
			else
			{
				m_Butler.SetRestockPresetValue(m_Preset, type, 0);
			}
		}
		
		private void LoadInventory()
		{
			int xIndex = 0;
			int startX = 100;
			int startY = 30;
			int offsetY = 200;
			
			// Mandrake Root
			AddItem(startY, startX + (xIndex * 30), 0x0F86);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.MandrakeRoot) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 1, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.MandrakeRoot) + "");
			
			// Greater Explosion Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F0D);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.GreaterExplosionPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 9, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.GreaterExplosionPotion) + "");
			
			xIndex++;
			// Blood Moss			
			AddItem(startY, startX + (xIndex * 30), 0x0F7B);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Bloodmoss) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 2, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Bloodmoss) + "");
			
			// Greater Cure Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F07);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.GreaterCurePotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 10, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.GreaterCurePotion) + "");
			
			xIndex++;
			// Black Pearl			
			AddItem(startY, startX + (xIndex * 30), 0x0F7A);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.BlackPearl) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 3, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.BlackPearl) + "");
			
			// Greater Heal Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F0C);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.GreaterHealPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 11, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.GreaterHealPotion) + "");
			
			xIndex++;
			// Garlic
			AddItem(startY, startX + (xIndex * 30), 0x0F84);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Garlic) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 4, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Garlic) + "");
			
			// Total Refresh Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F0B);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.TotalRefreshPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 12, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.TotalRefreshPotion) + "");
			
			xIndex++;
			// Ginseng
			AddItem(startY, startX + (xIndex * 30), 0x0F85);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Ginseng) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 5, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Ginseng) + "");
			
			// Greater Strength Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F09);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.GreaterStrengthPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 13, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.GreaterStrengthPotion) + "");
						
			xIndex++;
			// Nightshade
			AddItem(startY, startX + (xIndex * 30), 0x0F88);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Nightshade) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 6, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Nightshade) + "");
			
			// Greater Agility Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F08);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.GreaterAgilityPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 14, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.GreaterAgilityPotion) + "");
			
			xIndex++;
			// Sulfurous Ash
			AddItem(startY, startX + (xIndex * 30), 0x0F8C);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.SulfurousAsh) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 7, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.SulfurousAsh) + "");
			
			// Deadly Poison Potion
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x0F0A);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.DeadlyPoisonPotion) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 15, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.DeadlyPoisonPotion) + "");
						
			xIndex++;
			// Spiders Silk
			AddItem(startY, startX + (xIndex * 30), 0x0F8D);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.SpidersSilk) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 8, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.SpidersSilk) + "");
			
			// Bolt
			AddItem(startY + offsetY, startX + (xIndex * 30), 0x1BFB);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Bolt) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 16, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Bolt) + "");
			
			xIndex++;
			// Bandage
			AddItem(startY, startX + (xIndex * 30), 0xE21);
			AddLabel(startY + 45, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Bandage) + "");
			AddImageTiled(startY + 115, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115, startX + (xIndex * 30), 65, 15, 0, 18, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Bandage) + "");
			
			// Arrow
			AddItem(startY + offsetY, startX + (xIndex * 30), 0xF3F);
			AddLabel(startY + 45 + offsetY, startX + (xIndex * 30), 0, m_Butler.GetInventoryAmount(InventoryType.Arrow) + "");
			AddImageTiled(startY + 115 + offsetY, startX + (xIndex * 30), 65, 18, 3004);
			AddTextEntry(startY + 115 + offsetY, startX + (xIndex * 30), 65, 15, 0, 17, m_Butler.GetRestockPresetValue(m_Preset, InventoryType.Arrow) + "");
			
			int offsetX = 15;
			xIndex++;
			AddButton(90, startX + (xIndex * 30) + offsetX, 1589, 1590, 9, GumpButtonType.Reply, 0);
			AddLabel(115, startX + (xIndex * 30) + 3 + offsetX, 0, "Save Preset");
			
			AddButton(240, startX + (xIndex * 30) + offsetX, 1589, 1590, 10, GumpButtonType.Reply, 0);
			AddLabel(265, startX + (xIndex * 30) + 3 + offsetX, 0, "Restock Now");
			
			xIndex += 2;
			AddLabel(25, startX + (xIndex * 30) + 5, 0, @"Restock Presets :");
			
			xIndex ++;			
			int yIndex = 0;
			
			AddButton(25, startX + (xIndex * 30), 1595, 1594, 1, GumpButtonType.Reply, 0);
			AddLabel(43, startX + (xIndex * 30) + 3, 0, "1");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 2, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "2");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 3, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "3");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 4, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "4");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 5, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "5");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 6, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "6");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 7, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "7");
			
			yIndex++;
			AddButton(25 + (yIndex * 50), startX + (xIndex * 30), 1595, 1594, 8, GumpButtonType.Reply, 0);
			AddLabel(43 + (yIndex * 50), startX + (xIndex * 30) + 3, 0, "8");
		}
	}
}