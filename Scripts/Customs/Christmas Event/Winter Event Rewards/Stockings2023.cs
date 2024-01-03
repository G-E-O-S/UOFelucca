/*
 (¯`·.¸¸.·´¯`·.¸¸.·´¯)
 ( \               / )
  ( \ ) Made by ( / )
   ( ) (MrRiots  ) ( )
  ( / )         ( \ )
 ( /               \ )
 (_.·´¯`·.¸¸.·´¯`·.¸_)
*/

namespace Server.Items
{
	[Flipable(0x2bd9, 0x2bda)]
	public class GreenStocking2023 : BaseContainer
	{
		[Constructable]
		public GreenStocking2023()
			: base(Utility.Random(0x2BD9, 2))
		{
		}

		public GreenStocking2023(Serial serial)
			: base(serial)
		{
		}

		public override int DefaultGumpID => 0x103;

		public override int DefaultDropSound => 0x42;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void AddWeightProperty(ObjectPropertyList list)
		{
			base.AddWeightProperty(list);
			list.Add("<basefont color=#8BDFFC>Wintertide 2023<basefont color=#FFFFFF>");
		}


		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	[Flipable(0x2bdb, 0x2bdc)]
	public class RedStocking2023 : BaseContainer
	{
		[Constructable]
		public RedStocking2023()
			: base(Utility.Random(0x2BDB, 2))
		{
		}

		public RedStocking2023(Serial serial)
			: base(serial)
		{
		}

		public override void AddWeightProperty(ObjectPropertyList list)
		{
			base.AddWeightProperty(list);
			list.Add("<basefont color=#8BDFFC>Wintertide 2023<basefont color=#FFFFFF>");
		}


		public override int DefaultGumpID => 0x103;

		public override int DefaultDropSound => 0x42;

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
