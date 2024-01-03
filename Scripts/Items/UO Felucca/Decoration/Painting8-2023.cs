using System;

namespace Server.Items
{
    [Flipable(0xA70A, 0xA70B)]
    public class Painting8 : Item
    {
        [Constructable]
        public Painting8()
            : base(0xA70A)
        {
            Name = "Fine Art - MrRiots [8/15]";
            Weight = 1;
        }

        public Painting8(Serial serial)
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
