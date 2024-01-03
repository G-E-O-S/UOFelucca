using System;

namespace Server.Items
{
    [Flipable(0xA702, 0xA703)]
    public class Painting5 : Item
    {
        [Constructable]
        public Painting5()
            : base(0xA702)
        {
            Name = "Fine Art - MrRiots [5/15]";
            Weight = 1;
        }

        public Painting5(Serial serial)
            : base(serial)
        {
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
