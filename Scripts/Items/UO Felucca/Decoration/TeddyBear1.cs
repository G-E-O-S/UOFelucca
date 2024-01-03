using System;

namespace Server.Items
{
    [Flipable(0xA6B8, 0xA6B9)]
    public class TeddyBear1 : Item
    {       
        [Constructable]
        public TeddyBear1()
            : base(0xA6B8)
        {
            Name = "Teddy Bear";
            Weight = 1;
        }

        public TeddyBear1(Serial serial)
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
