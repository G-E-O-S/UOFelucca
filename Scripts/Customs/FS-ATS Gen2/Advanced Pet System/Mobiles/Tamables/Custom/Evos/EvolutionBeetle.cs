namespace Server.Mobiles
{
    [CorpseName("a bug corpse")]
    public class EvolutionBettle : BaseCreature
    {
        [Constructable]
        public EvolutionBettle() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a beetle baby";
            Body = 791;
            BaseSoundID = 0;

            SetStr(95, 115);
            SetDex(55, 100);
            SetInt(15, 35);

            SetHits(75, 150);
            SetMana(0);

            SetDamage(5, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.Wrestling, 50.1, 70.0);
            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.3, 34.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 16;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 65.1;

            F0 = true;
            Evolves = true;

            UsesForm1 = true;
            UsesForm2 = true;
            UsesForm3 = true;

            Form1 = 242;    //Death Watch Beetle
            Form2 = 315;    //Fleshrenderer
            Form3 = 244;    //Rune Beetle

            Sound1 = 0; //See Sounds Below
            Sound2 = 0; //See Sounds Below
            Sound3 = 0; //See Sounds Below

            int totalstats = Str + Dex + Int + HitsMax + StamMax + ManaMax + PhysicalResistance + FireResistance + ColdResistance + EnergyResistance + PoisonResistance + DamageMin + DamageMax + VirtualArmor;
            int nextlevel = totalstats * 10;

            NextLevel = nextlevel;
        }

        public override Poison HitPoison => Poison.Lesser;

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;

        public EvolutionBettle(Serial serial) : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            if (Level == 1)
            {
                return 0x21D;
            }
            if (Level == 2)
            {
                return 0x4F4;
            }
            else if (Level == 3)
            {
                return 0x34C;
            }
            else if (Level >= 4)
            {
                return 0x4E9;
            }
            else
            {
                return 0x21D;
            }
        }

        public override int GetIdleSound()
        {
            if (Level == 1)
            {
                return 0x21D;
            }
            else if (Level == 2)
            {
                return 0x4F3;
            }
            else if (Level == 3)
            {
                return 0x34C;
            }
            else if (Level >= 4)
            {
                return 0x4E8;
            }
            else
            {
                return 0x21D;
            }

        }

        public override int GetAttackSound()
        {
            if (Level == 1)
            {
                return 0x21D;
            }
            else if (Level == 2)
            {
                return 0x4F2;
            }
            else if (Level == 3)
            {
                return 0x34C;
            }
            else if (Level >= 4)
            {
                return 0x4E7;
            }
            else
            {
                return 0x162;
            }

        }

        public override int GetHurtSound()
        {
            if (Level == 1)
            {
                return 0x21D;
            }
            else if (Level == 2)
            {
                return 0x4F5;
            }
            else if (Level == 3)
            {
                return 0x354;
            }
            else if (Level >= 4)
            {
                return 0x4EA;
            }
            else
            {
                return 0x163;
            }

        }

        public override int GetDeathSound()
        {
            if (Level == 1)
            {
                return 0x21D;
            }
            else if (Level == 2)
            {
                return 0x4F1;
            }
            else if (Level == 3)
            {
                return 0x354;
            }
            else if (Level >= 4)
            {
                return 0x4E6;
            }
            else
            {
                return 0x21D;
            }

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}