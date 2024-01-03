using System;

namespace Server.Items
{
    [Flipable(0xA6FC, 0xA6FD)]
    public class Painting2 : Item
    {
        [Constructable]
        public Painting2()
            : base(0xA6FC)
        {
            Name = "Fine Art - MrRiots [2/15]";
            Weight = 1;
        }

        public Painting2(Serial serial)
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
