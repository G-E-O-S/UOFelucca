using System;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x9A94)]
    public class DonationBox3 : BaseContainer
    {

        [Constructable]
        public DonationBox3()
            : this(0x9A94)
        {
        }

        [Constructable]
        public DonationBox3(int hue)
            : base(0x9A94)
        {
            this.Weight = 2.0;
            this.Hue = 1001;
            this.Name = "UO: Felucca";
            this.ItemID = 39572;
            this.GumpID = 258;
        }

        public DonationBox3(Serial serial)
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
