using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class MasterFinesseRing : BaseArmor, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTailoring.CraftSystem; } }

        [Constructable]
        public MasterFinesseRing()
            : base(0x108a)
        {
            this.Attributes.BonusDex = 5;
            this.Name = "Master Finesse Ring";
            this.LootType = LootType.Newbied;
            this.Quality = ItemQuality.Exceptional;
            this.Hue = 1150;
        }

        public MasterFinesseRing(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 0;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 300;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 500;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 30;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 10;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 5;
            }
        }
        public override double DefaultWeight
        {
            get
            {
                return 0.05;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Leather;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.RegularLeather;
            }
        }
        public override ArmorMeditationAllowance DefMedAllowance
        {
            get
            {
                return ArmorMeditationAllowance.All;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
