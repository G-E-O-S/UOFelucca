using System;

namespace Server.Items
{
    [Flipable(0xA929, 0xA92A)]
    public class Shield25 : Item
    {
        [Constructable]
        public Shield25()
            : base(0xA929)
        {
            Name = "25 Years of Ultima Online!";
            Weight = 1;
        }

        public Shield25(Serial serial)
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
