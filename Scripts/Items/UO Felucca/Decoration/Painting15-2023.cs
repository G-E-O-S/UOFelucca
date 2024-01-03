using System;

namespace Server.Items
{
    [Flipable(0xAA28, 0xAA29)]
    public class Painting15 : Item
    {
        [Constructable]
        public Painting15()
            : base(0xAA28)
        {
            Name = "Fine Art - MrRiots [15/15]";
            Weight = 1;
        }

        public Painting15(Serial serial)
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
