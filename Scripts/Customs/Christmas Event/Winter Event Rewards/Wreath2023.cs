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

namespace Server.Items
{
	[Flipable(0x232C, 0x232D)]
	public class Wreath2023 : Item
	{
		[Constructable]
		public Wreath2023()
			: base(0x232D)
		{
			Movable = true;
			Hue = Utility.RandomList(33, 1157, 1774, 1771, 1779, 1911, 1919, 1932, 1927, 2498, 2500);
			Name = "holiday wreath";
		
			Weight = 3.0;
		}

		public Wreath2023(Serial serial)
			: base(serial)
		{
		}

		public override void AddWeightProperty(ObjectPropertyList list)
		{
			base.AddWeightProperty(list);
			list.Add("<basefont color=#8BDFFC>Wintertide 2023<basefont color=#FFFFFF>");
		}

	
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
