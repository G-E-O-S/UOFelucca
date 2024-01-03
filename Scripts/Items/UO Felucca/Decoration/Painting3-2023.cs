using System;

namespace Server.Items
{
    [Flipable(0xA6FE, 0xA6FF)]
    public class Painting3 : Item
    {
        [Constructable]
        public Painting3()
            : base(0xA6FE)
        {
            Name = "Fine Art - MrRiots [3/15]";
            Weight = 1;
        }

        public Painting3(Serial serial)
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
