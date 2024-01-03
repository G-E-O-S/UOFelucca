using System;

namespace Server.Items
{
    [Flipable(0xA6D2, 0xA6D3)]
    public class CrystalFigure2 : Item
    {
        [Constructable]
        public CrystalFigure2()
            : base(0xA6D2)
        {
            Name = "Crystal Figure";
            Weight = 1;
        }

        public CrystalFigure2(Serial serial)
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
