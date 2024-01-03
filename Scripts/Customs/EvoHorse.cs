using System;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a evo horse corpse")]
    public class EvoHorse : BaseMount
    {
        // We make the first two digits the same since they represent Stage 0 and Stage 1
        // If you want to make the body to change when reaching Stage 1, then set the second number to something different
        private List<int> EvoStageBodyValues = new List<int> { 0x74, 0x74, 0xDB, 0xDB, 0xDB, 0xDB, 0xDB };

        // Since they are evos, we need to change the ItemID value as well
        private List<int> EvoStageItemValues = new List<int> { 0x3EA7, 0x3EA7, 0x3EA5, 0x3EA5, 0x3EA5, 0x3EA5, 0x3EA5 };

        [Constructable]
        public EvoHorse() : this("a evo horse")
        {
        }

        [Constructable]
        public EvoHorse(string name) : base(name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            IsEvoPet = true;
            EvoStage = 0;
            EvoMaxStage = 6;

            BaseSoundID = 0x16A;

            this.VirtualArmor = Utility.RandomMinMax(0, 6);

            this.SetStr(10);
            this.SetDex(25, 35);
            this.SetInt(10);

            this.SetDamage(5);

            this.SetSkill(SkillName.Wrestling, 4.2, 6.4);
            this.SetSkill(SkillName.Tactics, 4.0, 6.0);
            this.SetSkill(SkillName.MagicResist, 4.0, 5.0);

            this.Fame = 150;
            this.Karma = 0;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -6.9;
        }

        public override void Evolve(BaseCreature attacker)
        {
            base.Evolve(attacker);

            StageBody();
        }

        public void StageBody()
        {
            if (this.EvoStageBodyValues.Count > 0 && this.EvoStage <= this.EvoStageBodyValues.Count)
            {
                this.BodyValue = this.EvoStageBodyValues[this.EvoStage];
            }

            if (this.EvoStageItemValues.Count > 0 && this.EvoStage <= this.EvoStageItemValues.Count)
            {
                this.ItemID = this.EvoStageItemValues[this.EvoStage];
            }
        }

        // THE SECTIONS BELOW CAN BE USED TO OVERRIDE THE INCREASE VALUES FOUND ON BASECREATURE.CS

        /* public override void RaiseSkills()
        {
            Server.Skills skills = this.Skills;

            for (int i = 0; i < skills.Length; ++i)
            {
                // ONLY INCREASE SKILLS THAT ALREADY HAVE A VALUE HIGHER THAN 0, EASY WAY TO ENSURE WE DON'T RAISE ALL SKILLS
                if(skills[i].Base > 0)
                    skills[i].Base += 15;
            }
        } */

        /* public override void RaiseStats()
        {
            int increaseAmount = 15;

            this.RawStr += increaseAmount;
            this.RawDex += increaseAmount;
            this.RawInt += increaseAmount;
        } */

        public EvoHorse(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 5;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override bool CanAngerOnTame
        {
            get
            {
                return false;
            }
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
                return 0x16A;

            return base.GetAngerSound();
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
