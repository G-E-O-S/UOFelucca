using System;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class AncientMinerPickaxe : BaseAxe, IUsesRemaining
    {
        [Constructable]
        public AncientMinerPickaxe()
            : this(9999)
        {
        }

        [Constructable]
        public AncientMinerPickaxe(int uses)
            : base(0xE86)
        {
            Weight = 1.0;
            Hue = 2739;
            UsesRemaining = uses;
            ShowUsesRemaining = true;
            LootType = LootType.Blessed;
        }

        public AncientMinerPickaxe(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Ancient Miner's Pickaxe";
            }
        }
        public override HarvestSystem HarvestSystem
        {
            get
            {
                return Mining.System;
            }
        }

        public override int AosStrengthReq
        {
            get
            {
                return 50;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 13;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 15;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 35;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 3.00f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 25;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 1;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 15;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 35;
            }
        }

        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 60;
            }
        }

        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Slash1H;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
