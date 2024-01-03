#region Header
//               _,-'/-'/
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2023  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #                                       #
#endregion

using Server.Items;
using Xanthos.ShrinkSystem;

namespace Server.Mobiles
{
    public class KhalEldur : BaseDeviant
    {
        public override DeviationFlags DefaultDeviations
        {
            get { return DeviationFlags.Death | DeviationFlags.Famine | DeviationFlags.Decay; }
        }

        [Constructable]
        public KhalEldur()
            : base(AIType.AI_Mage, FightMode.Closest, 16, 1, 0.1, 0.2)
        {
            Name = "Khal Eldur (Witch Doctor)";            
            SpellHue = 2730;

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 0);
            SetDamageType(ResistanceType.Cold, 0);
            SetDamageType(ResistanceType.Poison, 0);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 100);
            SetResistance(ResistanceType.Poison, 50, 75);
            SetResistance(ResistanceType.Energy, 50, 75);

            SetSkill(SkillName.EvalInt, 30.1, 99.9);
            SetSkill(SkillName.Magery, 30.1, 99.9);
            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 92.5);
        }

        public KhalEldur(Serial serial)
            : base(serial)
        { }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new TerrorCloth());
            if (Utility.RandomDouble() < 0.50)
                c.DropItem(new EvilCloth());
            if (Utility.RandomDouble() < 0.30)
                c.DropItem(new TerrorBackpackDye());
            if (Utility.RandomDouble() < 0.20)
                c.DropItem(new TerrorBeardDye());
            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new TerrorCandle());
            if (Utility.RandomDouble() < 0.08)
                c.DropItem(new TerrorLantern());
            if (Utility.RandomDouble() < 0.06)
                c.DropItem(new TerrorTorch());
            if (Utility.RandomDouble() < 0.04)
                c.DropItem(new TerrorSpellbook());
        }

        protected override int InitBody()
        {
            return 0x5C7;
        }

        public override int GetIdleSound()
        {
            return 466;
        }

        public override int GetAngerSound()
        {
            return 467;
        }

        public override int GetDeathSound()
        {
            return 468;
        }

        public override int GetHurtSound()
        {
            return 469;
        }

        public override int GetAttackSound()
        {
            return Utility.RandomList(568, 569, 571, 572);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}
