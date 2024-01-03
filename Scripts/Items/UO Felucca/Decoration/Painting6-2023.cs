using System;

namespace Server.Items
{
    [Flipable(0xA704, 0xA705)]
    public class Painting6 : Item
    {
        [Constructable]
        public Painting6()
            : base(0xA704)
        {
            Name = "Fine Art - MrRiots [6/15]";
            Weight = 1;
        }

        public Painting6(Serial serial)
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
