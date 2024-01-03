using System;

namespace Server.Items
{
    [Flipable(0xA7CF, 0xA7D0)]
    public class ChristmasTapestry2 : Item
    {
        [Constructable]
        public ChristmasTapestry2()
            : base(0xA7CF)
        {
            Name = "Christmas Tapestry";
            Weight = 1;
        }

        public ChristmasTapestry2(Serial serial)
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
