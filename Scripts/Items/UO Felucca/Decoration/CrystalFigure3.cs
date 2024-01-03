using System;

namespace Server.Items
{
    [Flipable(0xA6D4, 0xA6D5)]
    public class CrystalFigure3 : Item
    {
        [Constructable]
        public CrystalFigure3()
            : base(0xA6D4)
        {
            Name = "Crystal Figure";
            Weight = 1;
        }

        public CrystalFigure3(Serial serial)
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
