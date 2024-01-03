namespace Server.Mobiles
{
    [CorpseName("a bull corpse")]
    public class RidableBull : BaseMount
    {
        [Constructable]
        public RidableBull() : this("a ridable bull")
        {
        }

        [Constructable]
        public RidableBull(string name) : base(name, 0xE8, 232, AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x64;
            Hue = Utility.RandomList(0, 1109, 1194, 1175, 1161);

            SetStr(50, 100);
            SetDex(86, 125);
            SetInt(50, 75);

            SetHits(80, 100);

            SetDamage(4, 8);

            //SetDamageType(ResistanceType.Physical, 40);
            //SetDamageType(ResistanceType.Fire, 40);
            //SetDamageType(ResistanceType.Energy, 20);

            //SetResistance(ResistanceType.Physical, 55, 65);
            //SetResistance(ResistanceType.Fire, 30, 40);
            //SetResistance(ResistanceType.Cold, 30, 40);
            //SetResistance(ResistanceType.Poison, 30, 40);
            //SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 85.3, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 95.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);
            SetSkill(SkillName.Anatomy, 80.5, 92.5);

            Fame = 180;
            Karma = -180;

            VirtualArmor = 64;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 00.1;

            int totalstats = Str + Dex + Int + HitsMax + StamMax + ManaMax + PhysicalResistance + FireResistance + ColdResistance + EnergyResistance + PoisonResistance + DamageMin + DamageMax + VirtualArmor;
            int nextlevel = totalstats * 10;

            NextLevel = nextlevel;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override FoodType FavoriteFood => FoodType.GrainsAndHay;

        public RidableBull(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
