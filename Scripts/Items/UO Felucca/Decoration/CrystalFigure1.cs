using System;

namespace Server.Items
{
    [Flipable(0xA6D0, 0xA6D1)]
    public class CrystalFigure1 : Item
    {
        [Constructable]
        public CrystalFigure1()
            : base(0xA6D0)
        {
            Name = "Crystal Figure";
            Weight = 1;
        }

        public CrystalFigure1(Serial serial)
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
