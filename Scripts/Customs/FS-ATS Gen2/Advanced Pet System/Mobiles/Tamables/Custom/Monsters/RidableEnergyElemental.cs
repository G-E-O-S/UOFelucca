namespace Server.Mobiles
{
    [CorpseName("a energy elemental corpse")]
    public class RidableEnergyElemental : BaseMount
    {
        [Constructable]
        public RidableEnergyElemental() : this("a ridable energy elemental")
        {
        }

        [Constructable]
        public RidableEnergyElemental(string name) : base(name, 0xD, 0x3EA6, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 655;        
            Hue = Utility.RandomList(1378, 1109, 1194, 1175, 1161);

            SetStr(100, 250);
            SetDex(86, 150);
            SetInt(100, 150);

            SetHits(325, 425);

            SetDamage(16, 22);

            //SetDamageType(ResistanceType.Physical, 40);
            //SetDamageType(ResistanceType.Fire, 40);
            //SetDamageType(ResistanceType.Energy, 20);

            //SetResistance(ResistanceType.Physical, 55, 65);
            //SetResistance(ResistanceType.Fire, 30, 40);
            //SetResistance(ResistanceType.Cold, 30, 40);
            //SetResistance(ResistanceType.Poison, 30, 40);
            //SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 10.4, 50.0);
            SetSkill(SkillName.Magery, 10.4, 50.0);
            SetSkill(SkillName.MagicResist, 85.3, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 95.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);
            SetSkill(SkillName.Anatomy, 80.5, 92.5);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 64;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 99.9;

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

        public override FoodType FavoriteFood => FoodType.Meat;

        public RidableEnergyElemental(Serial serial) : base(serial)
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
