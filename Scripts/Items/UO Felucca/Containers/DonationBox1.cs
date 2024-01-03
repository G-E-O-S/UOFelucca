using System;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x9A92)]
    public class DonationBox1 : BaseContainer
    {

        [Constructable]
        public DonationBox1()
            : this(0x9A92)
        {
        }

        [Constructable]
        public DonationBox1(int hue)
            : base(0x9A92) 
        {
            this.Weight = 2.0;
            this.Hue = 1001;
            this.Name = "UO: Felucca";
            this.ItemID = 39570;
            this.GumpID = 258;
        }

        public DonationBox1(Serial serial)
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
