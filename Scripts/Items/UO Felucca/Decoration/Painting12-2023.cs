using System;

namespace Server.Items
{
    [Flipable(0xA714, 0xA715)]
    public class Painting12 : Item
    {
        [Constructable]
        public Painting12()
            : base(0xA714)
        {
            Name = "Fine Art - MrRiots [12/15]";
            Weight = 1;
        }

        public Painting12(Serial serial)
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
