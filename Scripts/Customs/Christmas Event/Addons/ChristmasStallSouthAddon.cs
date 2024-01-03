/*
 (�`�.��.���`�.��.���)
 ( \               / )
  ( \ ) Made by ( / )
   ( ) (MrRiots  ) ( )
  ( / )         ( \ )
 ( /               \ )
 (_.���`�.��.���`�.�_)
*/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChristmasStallSouthAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1473, 2, -1, 15}, {1473, -2, -1, 15}, {1473, -1, -1, 15}// 1	2	3	
			, {1473, 0, -1, 15}, {1471, -2, 0, 18}, {1473, 1, -1, 15}// 4	5	6	
			, {1471, 0, 0, 18}, {6092, 3, 0, 0}, {1471, -1, 0, 18}// 7	8	9	
			, {1471, 1, 0, 18}, {1471, 2, 0, 18}, {6086, 3, -1, 0}// 10	11	12	
			, {42612, -1, 0, 27}, {17781, 3, 0, 5}, {17782, 3, -1, 5}// 13	14	15	
			, {4014, 3, -1, 0}, {4014, 3, 0, 0}, {4014, 3, -1, 5}// 16	17	18	
			, {42612, 2, 0, 21}, {2880, -1, 1, 0}, {2880, 0, 1, 0}// 19	20	21	
			, {2880, 1, 1, 0}, {1472, -2, 1, 15}, {1472, -1, 1, 15}// 22	23	24	
			, {1472, 0, 1, 15}, {1472, 1, 1, 15}, {1472, 2, 1, 15}// 25	26	27	
			, {6087, 3, 1, 0}, {42610, 2, 1, 25}, {17782, 3, 1, 5}// 28	29	30	
			, {9079, -2, 2, 0}// 31	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ChristmasStallSouthAddonDeed();
			}
		}

		[ Constructable ]
		public ChristmasStallSouthAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 44557, 3, 1, 0, 0, 29, "", 1);// 32

		}

		public ChristmasStallSouthAddon( Serial serial ) : base( serial )
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

	public class ChristmasStallSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ChristmasStallSouthAddon();
			}
		}

		[Constructable]
		public ChristmasStallSouthAddonDeed()
		{
			Name = "ChristmasStallSouth";
		}

		public ChristmasStallSouthAddonDeed( Serial serial ) : base( serial )
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