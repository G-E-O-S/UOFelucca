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
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Icicles2023 : Item
	{
		
		[Constructable]
		public Icicles2023() : base(Utility.RandomList(0x08E0, 0x08E1, 0x08E3, 0x08E4, 0x08E5, 0x08E7, 0x08E9, 0x08EA, 0x08E8, 0x08E2))
		{
			Weight = 1;
			Hue = 0x09C2;
			Name = "icicle";

		}
	

		public Icicles2023(Serial serial) : base(serial)
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
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
