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

	public class ChristmasSpiritCrystal : Item
	{


		[Constructable]
		public ChristmasSpiritCrystal()
			: base(0x1F1A)
		{
			Weight = 1.0;
			Hue = 0x09C2;
			Name = "christmas spirit trapped in a crystal";

		}

		public ChristmasSpiritCrystal(Serial serial)
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
