using System;

namespace Server.Items
{
    [Flipable(0xA710, 0xA711)]
    public class Painting11 : Item
    {
        [Constructable]
        public Painting11()
            : base(0xA710)
        {
            Name = "Fine Art - MrRiots [11/15]";
            Weight = 1;
        }

        public Painting11(Serial serial)
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
