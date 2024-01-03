using System;

namespace Server.Items
{    
    public class CrystalFigure4 : Item
    {
        [Constructable]
        public CrystalFigure4()
            : base(0xA6CF)
        {
            Name = "Crystal Figure";
            Weight = 1;
        }

        public CrystalFigure4(Serial serial)
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
