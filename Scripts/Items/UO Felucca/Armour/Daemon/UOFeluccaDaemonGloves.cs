using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x13dc, 0x13d4)]
    public class UOFeluccaDaemonGloves : BaseArmor, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTailoring.CraftSystem; } }

        public override bool IsArtifact { get { return true; } }
        [Constructable]
        public UOFeluccaDaemonGloves()
            : base(0x1450)
        {
            this.Weight = 1.0;
            this.Hue = Utility.RandomList(1175, 1109, 1910, 1194);
            this.Name = "Daemon Gloves";
            this.Attributes.BonusInt = 1;
            this.LootType = LootType.Newbied;
            this.Quality = ItemQuality.Exceptional;
            this.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
        }

        public UOFeluccaDaemonGloves(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 2;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 4;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 4;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 35;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 45;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 25;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 25;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 16;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Bone;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.None;
            }
        }
        //public override int LabelNumber
        //{
        //    get
        //    {
        //        return 1041493;
        //    }
        //}// studded sleeves, ranger armor
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (this.Weight == 1.0)
                this.Weight = 4.0;
        }
    }
}
