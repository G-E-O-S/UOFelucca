using System;

namespace Server.Items
{
    //Based off Cleaver
    [FlipableAttribute(0x48AE, 0x48AF)]
    public class GargishCleaver : BaseKnife
    {
        [Constructable]
        public GargishCleaver()
            : base(0x48AE)
        {
            Weight = 2.0;
        }

        public GargishCleaver(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.BleedAttack;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.InfectiousStrike;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 10;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 10;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 14;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 46;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.50f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 10;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 2;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 13;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 40;
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
                return 50;
            }
        }
        public override Race RequiredRace
        {
            get
            {
                return Race.Human;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
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
