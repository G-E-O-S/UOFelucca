using System;

namespace Server.Items
{
    [Flipable(0xA6FA, 0xA6FB)]
    public class Painting1 : Item
    {
        [Constructable]
        public Painting1()
            : base(0xA6FA)
        {
            Name = "Fine Art - MrRiots [1/15]";
            Weight = 1;
        }

        public Painting1(Serial serial)
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
