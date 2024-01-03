using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Multis;

namespace Server.Items
{
	public enum InventoryType
	{
		MandrakeRoot,
		Bloodmoss,
		BlackPearl,
		Garlic,
		Ginseng,
		Nightshade,
		SulfurousAsh,
		SpidersSilk,
		GreaterExplosionPotion,
		GreaterCurePotion,
		GreaterHealPotion,
		TotalRefreshPotion,
		GreaterStrengthPotion,
		GreaterAgilityPotion,
		DeadlyPoisonPotion,
		Arrow,
		Bolt,
		Bandage
	}
	
	public class ButlerDeed : Item
	{
		public Butler m_Butler;
		
		public Dictionary<InventoryType, int> Inventory { get; set; }
		
		[Constructable]
		public ButlerDeed()
			: base(0x2258)
		{
			Name = "Butler Deed";
			Weight = 1.0;
			Hue = 1266;
			LootType = LootType.Blessed;
			
			Inventory = new Dictionary<InventoryType, int>();				
			foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
			{
				Inventory.Add(type, 0);
			}
		}

		public ButlerDeed(Serial serial)
			: base(serial)
		{
			
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); //version
			
			foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
			{
				writer.Write(Inventory[type]);
			}
			
			if(m_Butler is object)
			{
				writer.Write((int)1); // Butler exists flag
				writer.WriteMobile(m_Butler);
			}
			else 
			{
				writer.Write((int)0);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			
			Inventory = new Dictionary<InventoryType, int>();				
			foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
			{
				Inventory.Add(type, reader.ReadInt());
			}
			
			int butlerExists = reader.ReadInt();			
			if(butlerExists == 1)
			{
				m_Butler = (Butler)reader.ReadMobile();
			}
		}
		
		public override void OnDelete()
		{
			if(m_Butler is object)
			{
				m_Butler.Delete();
			}
			
			base.OnDelete();
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if(m_Butler is object)
			{
				return;
			}
			
			if (!this.IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (from.AccessLevel >= AccessLevel.GameMaster)
			{
				from.SendLocalizedMessage(503248); // Your godly powers allow you to place this vendor whereever you wish.

				CreateButler(from);
			}
			else
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);

				if (house == null)
				{
					from.SendLocalizedMessage(503240); // Vendors can only be placed in houses.	
				}
				//else if (!house.Public) 
    //            {
    //                from.SendLocalizedMessage(503241); // You cannot place this vendor or barkeep.  Make sure the house is public and has sufficient storage available.
    //            }
				else if (!house.IsOwner(from))
				{
					from.SendLocalizedMessage(1062423); // Only the house owner can directly place vendors.  Please ask the house owner to offer you a vendor contract so that you may place a vendor in this house.
				}
				else
				{
					CreateButler(from);
				}
			}
		}
		
		public void CreateButler(Mobile from)
		{
			Butler butler = new Butler(this, from);
			m_Butler = butler;

			butler.MoveToWorld(from.Location, from.Map);	
			MoveToWorld(from.Location, from.Map);
			Movable = false;
			Visible = false;
		}
		
		public void RemoveButler()
		{
			m_Butler.Delete();
			m_Butler = null;
		}
	}
}
