using System;

namespace Server.Items
{
    [Flipable(0xA718, 0xA719)]
    public class Painting14 : Item
    {
        [Constructable]
        public Painting14()
            : base(0xA718)
        {
            Name = "Fine Art - MrRiots [14/15]";
            Weight = 1;
        }

        public Painting14(Serial serial)
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
