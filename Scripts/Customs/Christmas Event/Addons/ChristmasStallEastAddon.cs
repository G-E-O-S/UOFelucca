/*
 (¯`·.¸¸.·´¯`·.¸¸.·´¯)
 ( \               / )
  ( \ ) Made by ( / )
   ( ) (MrRiots  ) ( )
  ( / )         ( \ )
 ( /               \ )
 (_.·´¯`·.¸¸.·´¯`·.¸_)
*/

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChristmasStallEastAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {2879, 1, 0, 0}, {2879, 1, -1, 0}, {2879, 1, 1, 0}// 1	2	3	
			, {1480, 1, 1, 15}, {1476, -1, -2, 15}, {1476, -1, -1, 15}// 4	5	6	
			, {1476, -1, 0, 15}, {1476, -1, 1, 15}, {1476, -1, 2, 15}// 7	8	9	
			, {1475, 1, -2, 15}, {42612, 0, -1, 25}, {42613, 1, 1, 22}// 10	11	12	
			, {1475, 1, 2, 15}, {1474, 0, -2, 18}, {1474, 0, -1, 18}// 13	14	15	
			, {1474, 0, 0, 18}, {1474, 0, 1, 18}, {1474, 0, 2, 18}// 16	17	18	
			, {1478, 1, -1, 15}, {1486, 1, 0, 18}, {9080, 2, 2, 0}// 19	20	21	
			, {4014, 2, -2, 0}, {6088, -1, 3, 0}, {6087, 1, 3, 0}// 22	26	27	
			, {6089, 0, 3, 0}, {2917, 1, 3, 0}, {2918, -1, 3, 0}// 28	29	30	
			, {2919, 0, 3, 0}// 31	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ChristmasStallEastAddonDeed();
			}
		}

		[ Constructable ]
		public ChristmasStallEastAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 2594, 2, -2, 5, 0, 29, "", 1);// 23
			AddComplexComponent( (BaseAddon) this, 43294, 0, 3, 0, 0, 1, "", 1);// 24
			AddComplexComponent( (BaseAddon) this, 43294, 1, 3, 0, 0, 1, "", 1);// 25

		}

		public ChristmasStallEastAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class ChristmasStallEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ChristmasStallEastAddon();
			}
		}

		[Constructable]
		public ChristmasStallEastAddonDeed()
		{
			Name = "ChristmasStallEast";
		}

		public ChristmasStallEastAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
