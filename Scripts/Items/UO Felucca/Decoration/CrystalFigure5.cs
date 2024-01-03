using System;

namespace Server.Items
{
    public class CrystalFigure5 : Item
    {
        [Constructable]
        public CrystalFigure5()
            : base(0xA6D6)
        {
            Name = "Crystal Figure";
            Weight = 1;
        }

        public CrystalFigure5(Serial serial)
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
