using System;
using Server.Engines.Craft;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class AncientMinerCap : BaseHat, IRepairable
    {

        public CraftSystem RepairSystem { get { return DefTailoring.CraftSystem; } }

        [Constructable]
        public AncientMinerCap()
            : base(0x1544)
        {
            this.Name = "Ancient Miner's Cap";
            this.Hue = 2739;
            this.SkillBonuses.SetValues(0, SkillName.Mining, 10);
        }

        public AncientMinerCap(Serial serial)
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

        public override double DefaultWeight
        {
            get
            {
                return 0.05;
            }
        }

        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.None;
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
