using System;

namespace Server.Items
{
    [Flipable(0xA8D3, 0xA8D4)]
    public class DeathStatue2 : Item
    {
        [Constructable]
        public DeathStatue2()
            : base(0xA8D3)
        {
            Name = "Death";
            Weight = 1;
        }

        public DeathStatue2(Serial serial)
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
