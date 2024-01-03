using System;

namespace Server.Items
{
    [Flipable(0xA706, 0xA707)]
    public class Painting7 : Item
    {
        [Constructable]
        public Painting7()
            : base(0xA706)
        {
            Name = "Fine Art - MrRiots [7/15]";
            Weight = 1;
        }

        public Painting7(Serial serial)
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
