using System;

namespace Server.Items
{    
    public class SantasSack : Item
    {
        [Constructable]
        public SantasSack()
            : base(0x9DB5)
        {
            Name = "Santa's Huge Sack";
            Weight = 1;
        }

        public SantasSack(Serial serial)
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
