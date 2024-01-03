using System;

namespace Server.Items
{
    [Flipable(0xA70E, 0xA70F)]
    public class Painting10 : Item
    {
        [Constructable]
        public Painting10()
            : base(0xA70E)
        {
            Name = "Fine Art - MrRiots [10/15]";
            Weight = 1;
        }

        public Painting10(Serial serial)
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
