using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class UOFeluccaPhoenixGorget : BaseArmor, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefBlacksmithy.CraftSystem; } }

        [Constructable]
        public UOFeluccaPhoenixGorget()
            : base(0x1413)
        {
            this.Attributes.BonusStr = 1;
            this.Name = "Phoenix Gorget";
            this.LootType = LootType.Newbied;
            this.Quality = ItemQuality.Exceptional;
            this.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
            this.Hue = 1359;
        }

        public UOFeluccaPhoenixGorget(Serial serial)
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
                return 10;
            }
        }
        public override double DefaultWeight
        {
            get
            {
                return 1;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Plate;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.Iron;
            }
        }
        public override ArmorMeditationAllowance DefMedAllowance
        {
            get
            {
                return ArmorMeditationAllowance.Half;
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
