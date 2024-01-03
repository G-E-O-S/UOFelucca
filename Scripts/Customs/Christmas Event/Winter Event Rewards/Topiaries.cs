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
	[Flipable(0xA9C1, 0xA9C2)]
	public class ReindeerTopiary2023 : Item
	{


		[Constructable]
		public ReindeerTopiary2023()
			: base(0xA9C1)
		{
			Weight = 1.0;
			
			Name = "reindeer topiary";

		}

		public ReindeerTopiary2023(Serial serial)
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

	[Flipable(0xA9C3, 0xA9C4)]
	public class LlamaTopiary2023 : Item
	{


		[Constructable]
		public LlamaTopiary2023()
			: base(0xA9C3)
		{
			Weight = 1.0;

			Name = "llama topiary";

		}

		public LlamaTopiary2023(Serial serial)
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

	[Flipable(0xA9C5, 0xA9C6)]
	public class BearTopiary2023 : Item
	{


		[Constructable]
		public BearTopiary2023()
			: base(0xA9C5)
		{
			Weight = 1.0;

			Name = "bear topiary";

		}

		public BearTopiary2023(Serial serial)
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
