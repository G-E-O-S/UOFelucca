using System;
using System.Collections.Generic;

// made by zerodowned; for UO Fel

namespace Server.Mobiles
{
    [CorpseName("a evo raven corpse")]
    public class EvoRaven : BaseCreature
    {
        // We make the first two digits the same since they represent Stage 0 and Stage 1
        // If you want to make the body to change when reaching Stage 1, then set the second number to something different
        private List<int> EvoStageBodyValues = new List<int> { 6, 6, 208, 5, 733, 39, 30 };

        [Constructable]
        public EvoRaven() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            IsEvoPet = true;
            EvoStage = 0;
            EvoMaxStage = 6;
            

            this.Hue = 0x901;
            this.Name = "a Evo Raven";

            this.Body = 6;
            this.BaseSoundID = 0x1B;

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

        public override void Evolve( BaseCreature attacker )
        {
            base.Evolve(attacker);

            StageBody();
        }

        public void StageBody()
        {
            if ( this.EvoStageBodyValues.Count > 0 && this.EvoStage <= this.EvoStageBodyValues.Count)
            {
                this.BodyValue = this.EvoStageBodyValues[this.EvoStage];
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

        public EvoRaven(Serial serial) : base(serial)
        {
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