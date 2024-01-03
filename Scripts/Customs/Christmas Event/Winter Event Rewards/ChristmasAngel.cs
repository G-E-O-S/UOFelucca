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
	[Flipable(0x46FB, 0x46FA)]
	public class ChristmasAngel : Item
	{
		[Constructable]
		public ChristmasAngel()
			: base(0x46FB)
		{
			Movable = true;
			
			Name = "christmas angel";

			Weight = 3.0;
		}

		public ChristmasAngel(Serial serial)
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
