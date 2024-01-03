using System;

namespace Server.Items
{
    [Flipable(0xA6EA, 0xA6EB)]
    public class TeddyBear2 : Item
    {
        [Constructable]
        public TeddyBear2()
            : base(0xA6EA)
        {
            Name = "Teddy Bear";
            Weight = 1;
        }

        public TeddyBear2(Serial serial)
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
