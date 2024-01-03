using Server.Items;
using System;
using System.Collections.Generic;

// made by zerodowned; for UO Fel

namespace Server.Mobiles
{
    [CorpseName("a Evo Pet's corpse")]
    public class EvoBeast6 : BaseCreature
    {
        // We make the first two digits the same since they represent Stage 0 and Stage 1
        // If you want to make the body to change when reaching Stage 1, then set the second number to something different
        private List<int> EvoStageBodyValues = new List<int> { 202, 206, 1292, 244, 256 };

        [Constructable]
        public EvoBeast6() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            IsEvoPet = true;
            EvoStage = 0;
            EvoMaxStage = 4;

            this.Name = "Felucca Beast";
            this.Title = "[Evo Pet]";

            this.Body = 202;
            this.BaseSoundID = 0x571;

            this.VirtualArmor = Utility.RandomMinMax(96, 102);

            this.HitsMaxSeed = Utility.RandomMinMax(725, 750);

            this.SetMana(250, 275);
            this.SetStr(625, 650);
            this.SetDex(125, 135);
            this.SetInt(225, 250);

            this.SetDamage(17, 20);

            this.SetSkill(SkillName.Wrestling, 100);
            this.SetSkill(SkillName.Tactics, 100);
            this.SetSkill(SkillName.MagicResist, 100);
            this.SetSkill(SkillName.Magery, 100);
            this.SetSkill(SkillName.EvalInt, 100);
            this.SetSkill(SkillName.Meditation, 100);
            this.SetSkill(SkillName.Poisoning, 100);
            this.SetSkill(SkillName.Anatomy, 100);            

            SetSpecialAbility(SpecialAbility.DragonBreath);            

            this.Fame = 1500;
            this.Karma = 0;

            this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 110.1;
        }

        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Daemon;
            }
        }

        public override Poison HitPoison
        {
            get
            {
                return Poison.Deadly;
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
        }

        // THE SECTIONS BELOW CAN BE USED TO OVERRIDE THE INCREASE VALUES FOUND ON BASECREATURE.CS

        public override void RaiseSkills()
        {
            Server.Skills skills = this.Skills;

            for (int i = 0; i < skills.Length; ++i)
            {
                // ONLY INCREASE SKILLS THAT ALREADY HAVE A VALUE HIGHER THAN 0, EASY WAY TO ENSURE WE DON'T RAISE ALL SKILLS
                if (skills[i].Base > 0)
                    skills[i].Base += 5;
            }
        }

        public override void RaiseStats()
        {
            int increaseAmount = 50;

            this.HitsMaxSeed += 150 + increaseAmount;
            this.ManaMaxSeed += 100;
            this.RawStr += 60;
            this.RawDex += 10;
            this.RawInt += 10;
            this.DamageMin += 1;
            this.DamageMax += 2;
            this.VirtualArmor += 1;
        }

        public EvoBeast6(Serial serial) : base(serial)
        {
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
                return 0x574;

            return base.GetAngerSound();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            //if (Paragon.ChestChance > Utility.RandomDouble())
            c.DropItem(new Gold(299, 499));
            if (Utility.RandomDouble() < 0.50)
                c.DropItem(new SocketDeedPlusOne());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 4);
        }

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
