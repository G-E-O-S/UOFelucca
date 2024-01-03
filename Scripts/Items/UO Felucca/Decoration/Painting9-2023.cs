using System;

namespace Server.Items
{
    [Flipable(0xA70C, 0xA70D)]
    public class Painting9 : Item
    {
        [Constructable]
        public Painting9()
            : base(0xA70C)
        {
            Name = "Fine Art - MrRiots [9/15]";
            Weight = 1;
        }

        public Painting9(Serial serial)
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
