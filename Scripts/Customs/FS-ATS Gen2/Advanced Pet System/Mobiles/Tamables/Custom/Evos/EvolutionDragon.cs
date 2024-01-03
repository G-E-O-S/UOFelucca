namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class EvolutionDragon : BaseCreature
    {
        [Constructable]
        public EvolutionDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dragon baby";
            Body = 52;
            Hue = Utility.RandomSnakeHue();
            BaseSoundID = 0xDB;

            SetStr(401, 430);
            SetDex(133, 152);
            SetInt(101, 140);

            SetHits(241, 258);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 20);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
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

            Form1 = 89;     //Giant Serpent
            Form2 = 206;    //Large Lizard
            Form3 = 794;    //Swamp Dragon
            Form4 = 60;     //Drake
            Form5 = 12;     //Dragon
            Form6 = 62; //Wvyern
            Form7 = 103;    //Serpinetine Dragon
            Form8 = 46; //Ancient Wyrm
            Form9 = 172;    //Riktor

            Sound1 = 219;
            Sound2 = 0x5A;
            Sound3 = 0x16A;
            Sound4 = 362;
            Sound5 = 362;
            Sound6 = 362;
            Sound7 = 362;
            Sound8 = 362;
            Sound9 = 362;

            int totalstats = Str + Dex + Int + HitsMax + StamMax + ManaMax + PhysicalResistance + FireResistance + ColdResistance + EnergyResistance + PoisonResistance + DamageMin + DamageMax + VirtualArmor;
            int nextlevel = totalstats * 10;

            NextLevel = nextlevel;
        }

        public override Poison PoisonImmune => Poison.Lesser;
        public override Poison HitPoison => Poison.Greater;

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;

        public EvolutionDragon(Serial serial) : base(serial)
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