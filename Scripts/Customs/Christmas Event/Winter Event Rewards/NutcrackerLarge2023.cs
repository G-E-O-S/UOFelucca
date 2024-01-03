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
	[Flipable(0x46F2, 0x46F2)]
	public class NutcrackerLarge2023 : Item
	{
		[Constructable]
		public NutcrackerLarge2023()
			: base(0x46F2)
		{
			Movable = true;
			
			Name = "nutcracker";
		
			Weight = 3.0;
		}

		public NutcrackerLarge2023(Serial serial)
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
