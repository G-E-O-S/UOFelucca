using System;

namespace Server.Items
{
    [Flipable(0xA6EC, 0xA6ED)]
    public class TeddyBear3 : Item
    {
        [Constructable]
        public TeddyBear3()
            : base(0xA6EC)
        {
            Name = "Teddy Bear";
            Weight = 1;
        }

        public TeddyBear3(Serial serial)
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
