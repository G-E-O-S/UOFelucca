using System;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a Evo Pet's corpse")]
    public class EvoSteed3 : BaseMount
    {
        // We make the first two digits the same since they represent Stage 0 and Stage 1
        // If you want to make the body to change when reaching Stage 1, then set the second number to something different
        private List<int> EvoStageBodyValues = new List<int> { 0xD2, 0x4E7, 0x1b0, 0xF8, 0x2CB };

        // Since they are evos, we need to change the ItemID value as well
        private List<int> EvoStageItemValues = new List<int> { 0xD2, 0x4E7, 0x1b0, 0xF8, 0x2CB };

        [Constructable]
        public EvoSteed3() : this("Felucca Steed")
        {
        }

        public override double HealChance { get { return 0.62; } } // default 1.00

        [Constructable]
        public EvoSteed3(string name) : base(name, 0xD2, 0x3EA3, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            IsEvoPet = true;
            EvoStage = 0;
            EvoMaxStage = 4;
            Title = "[Evo Pet]";

            BaseSoundID = 0x63E;

            this.VirtualArmor = Utility.RandomMinMax(88, 96);

            this.HitsMaxSeed = Utility.RandomMinMax(275, 300);

            this.SetMana(125, 135);
            this.SetStr(225, 250);
            this.SetDex(125, 135);
            this.SetInt(125, 135);

            this.SetDamage(12, 18);

            this.SetSkill(SkillName.Wrestling, 100);
            this.SetSkill(SkillName.Tactics, 100);
            this.SetSkill(SkillName.MagicResist, 100);
            this.SetSkill(SkillName.Magery, 100);
            this.SetSkill(SkillName.EvalInt, 100);
            this.SetSkill(SkillName.Meditation, 100);
            this.SetSkill(SkillName.Healing, 100);
            this.SetSkill(SkillName.Anatomy, 100);

            SetSpecialAbility(SpecialAbility.DragonBreath);

            this.Fame = 1500;
            this.Karma = 0;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 110.1;
        }

        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Daemon;
            }
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

        public override void RaiseSkills()
        {
            Server.Skills skills = this.Skills;

            for (int i = 0; i < skills.Length; ++i)
            {
                // ONLY INCREASE SKILLS THAT ALREADY HAVE A VALUE HIGHER THAN 0, EASY WAY TO ENSURE WE DON'T RAISE ALL SKILLS
                if (skills[i].Base > 0)
                    skills[i].Base += 5; // default 15                
            }
        }

        public override void RaiseStats()
        {
            int increaseAmount = 50;

            this.HitsMaxSeed += 50 + increaseAmount;
            this.ManaMaxSeed += 50;
            this.RawStr += 85;
            this.RawDex += 10;
            this.RawInt += 10;
            this.DamageMin += 1;
            this.DamageMax += 2;
            this.VirtualArmor += 1;
        }

        public EvoSteed3(Serial serial)
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
                return true;
            }
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
                return 0x5E0;

            return base.GetAngerSound();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            //if (Paragon.ChestChance > Utility.RandomDouble())
            c.DropItem(new Gold(299, 499));
            if (Utility.RandomDouble() < 0.25)
                c.DropItem(new SocketDeedPlusOne());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);
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
