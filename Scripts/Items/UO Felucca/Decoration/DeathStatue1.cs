using System;

namespace Server.Items
{
    [Flipable(0xA8D1, 0xA8D2)]
    public class DeathStatue1 : Item
    {
        [Constructable]
        public DeathStatue1()
            : base(0xA8D1)
        {
            Name = "Death";
            Weight = 1;
        }

        public DeathStatue1(Serial serial)
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
