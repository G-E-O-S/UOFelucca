using System;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x9A93)]
    public class DonationBox2 : BaseContainer
    {

        [Constructable]
        public DonationBox2()
            : this(0x9A93)
        {
        }

        [Constructable]
        public DonationBox2(int hue)
            : base(0x9A93)
        {
            this.Weight = 2.0;
            this.Hue = 1001;
            this.Name = "UO: Felucca";
            this.ItemID = 39571;
            this.GumpID = 258;
        }

        public DonationBox2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
