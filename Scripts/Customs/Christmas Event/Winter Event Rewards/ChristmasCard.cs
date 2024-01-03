/*
 (¯`·.¸¸.·´¯`·.¸¸.·´¯)
 ( \               / )
  ( \ ) Made by ( / )
   ( ) (MrRiots  ) ( )
  ( / )         ( \ )
 ( /               \ )
 (_.·´¯`·.¸¸.·´¯`·.¸_)
*/

using System.Linq;
using System;
using Server.Mobiles;


namespace Server.Items
{
	[Flipable(0x0EBD, 0x0EBE)]
	public class ChristmasCard : Item
	{
		[Constructable]
		public ChristmasCard() : base(0x0EBD)
		{
			Weight = 1;
			Hue = Utility.RandomList(33, 1157, 1774, 1771, 1779, 1911, 1919, 1932, 1927, 2498, 2500);
			var names = World.Mobiles.Values.OfType<PlayerMobile>().Select(pm => pm.Name).Where(name => name != null).ToList();
			this.Name = "a christmas card from " + names[Utility.Random(0, names.Count)];


		}

		public ChristmasCard(Serial serial) : base(serial)
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
