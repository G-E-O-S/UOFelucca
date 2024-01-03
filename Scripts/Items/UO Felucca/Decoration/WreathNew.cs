using System;

namespace Server.Items
{
    [Flipable(0xA6B4, 0xA6B5)]
    public class WreathNew : Item
    {
        [Constructable]
        public WreathNew()
            : base(0xA6B4)
        {
            Name = "Wreath";
            Weight = 1;
        }

        public WreathNew(Serial serial)
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
