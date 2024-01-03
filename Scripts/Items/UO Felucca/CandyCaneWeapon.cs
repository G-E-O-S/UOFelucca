using System;

namespace Server.Items
{
    [FlipableAttribute(0xA7C3, 0xA7C4)]
    public class CandyCaneWeapon : BaseAxe
    {
        [Constructable]
        public CandyCaneWeapon()
            : base(0xA7C3)
        {
            this.Name = "Candy Cane";
            this.Weight = 4.0; 
            this.LootType = LootType.Newbied;
            this.Layer = Layer.TwoHanded;
            this.Skill = SkillName.Swords;
        }

        public CandyCaneWeapon(Serial serial)
            : base(serial)
        {
        }        
          
        public override int OldStrengthReq
        {
            get
            {
                return 35;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 8;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 33;
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
                return 999;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 999;
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
