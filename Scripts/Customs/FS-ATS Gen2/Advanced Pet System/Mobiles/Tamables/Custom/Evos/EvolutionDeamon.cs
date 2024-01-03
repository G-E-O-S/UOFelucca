namespace Server.Mobiles
{
    [CorpseName("a demon corpse")]
    public class EvolutionDeamon : BaseCreature
    {
        [Constructable]
        public EvolutionDeamon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a demon baby";
            Body = 317;
            Hue = Utility.RandomRedHue();
            BaseSoundID = 624;

            SetStr(401, 430);
            SetDex(133, 152);
            SetInt(101, 140);

            SetHits(241, 258);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.Wrestling, 65.1, 80.0);
            SetSkill(SkillName.Magery, 5.1, 10.0);
            SetSkill(SkillName.EvalInt, 5.1, 10.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 20;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 96.1;

            F0 = true;
            Evolves = true;

            UsesForm1 = true;
            UsesForm2 = true;
            UsesForm3 = true;
            UsesForm4 = true;
            UsesForm5 = true;
            UsesForm6 = true;
            UsesForm7 = true;
            UsesForm8 = true;
            UsesForm9 = true;

            Form1 = 74;     //Imp
            Form2 = 39;     //Mongbat
            Form3 = 779;    //Bogling
            Form4 = 784;    //Demon
            Form5 = 792;    //Chaos Demon
            Form6 = 755;    //Large Gargyole
            Form7 = 9;      //Deamon
            Form8 = 40; //Balron
            Form9 = 38; //Black Gate

            Sound1 = 422;
            Sound2 = 422;
            Sound3 = 422;
            Sound4 = 357;
            Sound5 = 357;
            Sound6 = 357;
            Sound7 = 357;
            Sound8 = 357;
            Sound9 = 357;

            int totalstats = Str + Dex + Int + HitsMax + StamMax + ManaMax + PhysicalResistance + FireResistance + ColdResistance + EnergyResistance + PoisonResistance + DamageMin + DamageMax + VirtualArmor;
            int nextlevel = totalstats * 10;

            NextLevel = nextlevel;
        }

        public override Poison PoisonImmune => Poison.Lesser;
        public override Poison HitPoison => Poison.Greater;

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;

        public EvolutionDeamon(Serial serial) : base(serial)
        {
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