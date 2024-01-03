using System;

namespace Server.Items
{
    [Flipable(0xA700, 0xA701)]
    public class Painting4 : Item
    {
        [Constructable]
        public Painting4()
            : base(0xA700)
        {
            Name = "Fine Art - MrRiots [4/15]";
            Weight = 1;
        }

        public Painting4(Serial serial)
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
