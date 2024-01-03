using System;

namespace Server.Items
{
    [Flipable(0xA716, 0xA717)]
    public class Painting13 : Item
    {
        [Constructable]
        public Painting13()
            : base(0xA716)
        {
            Name = "Fine Art - MrRiots [13/15]";
            Weight = 1;
        }

        public Painting13(Serial serial)
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
