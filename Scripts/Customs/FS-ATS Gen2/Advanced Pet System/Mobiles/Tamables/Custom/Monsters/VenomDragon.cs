namespace Server.Mobiles
{
    [CorpseName("a venom dragon corpse")]
    public class VenomDragon : BaseCreature
    {
        private static readonly string[] m_RandName = new string[]
        {
            "a venom dragon",
            "a acid dragon",
            "a toxic dragon",
            "a poison dragon"
        };

        [Constructable]
        public VenomDragon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = m_RandName[Utility.Random(m_RandName.Length)];
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = Utility.RandomList(64, 1367, 1368, 1369, 1370, 1371, 1372);

            SetStr(796, 825);
            SetDex(86, 105);
            SetInt(436, 475);

            SetHits(478, 495);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 12, 32);

            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Poisoning, 85.1, 111.2);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 92.5);

            if (Utility.Random(100) < 4)
            {
                AI = AIType.AI_Mage;
                SetSkill(SkillName.EvalInt, 30.1, 40.0);
                SetSkill(SkillName.Magery, 30.1, 40.0);
            }

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 100.9;

            int totalstats = Str + Dex + Int + HitsMax + StamMax + ManaMax + PhysicalResistance + FireResistance + ColdResistance + EnergyResistance + PoisonResistance + DamageMin + DamageMax + VirtualArmor;
            int nextlevel = totalstats * 10;

            NextLevel = nextlevel;

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 8);
        }

        //public override bool HasBreath{ get{ return true; } } // fire breath enabled
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;
        public override int Meat => 19;
        public override int Hides => 20;
        public override HideType HideType => HideType.Barbed;
        public override int Scales => 7;
        public override ScaleType ScaleType => (Body == 12 ? ScaleType.Yellow : ScaleType.Green);
        public override FoodType FavoriteFood => FoodType.Meat;
        public override Poison HitPoison => Poison.Deadly;

        public VenomDragon(Serial serial) : base(serial)
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