using System;

namespace Server.Items
{
    public class FemaleGargishPlateKilt : BaseArmor
    {
        [Constructable]
        public FemaleGargishPlateKilt()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishPlateKilt(int hue)
            : base(0x30B)
        {
            Weight = 5.0;
            Hue = hue;
        }

        public FemaleGargishPlateKilt(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 6; } }
        public override int BaseEnergyResistance { get { return 5; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int AosStrReq { get { return 80; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        public override Race RequiredRace { get { return Race.Human; } }
        public override bool CanBeWornByGargoyles { get { return true; } }

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
