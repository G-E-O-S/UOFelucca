using System;

namespace Server.Items
{
    [Flipable(0xA7CD, 0xA7CE)]
    public class ChristmasTapestry1 : Item
    {
        [Constructable]
        public ChristmasTapestry1()
            : base(0xA7CD)
        {
            Name = "Christmas Tapestry";
            Weight = 1;
        }

        public ChristmasTapestry1(Serial serial)
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
