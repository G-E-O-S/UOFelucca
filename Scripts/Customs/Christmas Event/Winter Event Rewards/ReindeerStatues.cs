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
	[Flipable(0x3A72, 0x3A6F)]
	public class ReindeerStatues : Item
	{


		[Constructable]
		public ReindeerStatues()
			: base(0x3A72)
		{
			Weight = 1.0;
			
			Name = "reindeer statuette";

		}

		public ReindeerStatues(Serial serial)
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
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
