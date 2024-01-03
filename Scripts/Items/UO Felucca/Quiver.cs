using Server.Items;
using Server;
using Ultima;

public class Quiver : BaseQuiver
{

    [Constructable]
    public Quiver() : base(0x2B02)
    {
        Hue = 0;
        Name = "Quiver";
        //DamageIncrease = 10;
        WeightReduction = 30;
        Attributes.BonusDex = 5;
        LootType = LootType.Blessed;
        //SkillBonuses.SetValues(0, SkillName.Archery, 5.0);
        //Attributes.ReflectPhysical = 5;
        //Attributes.AttackChance = 5;
        //LowerAmmoCost = 30;

        //switch (Utility.Random(5))
        //{
        //    case 0: Resistances.Physical = 10; break;
        //    case 1: Resistances.Fire = 10; break;
        //    case 2: Resistances.Cold = 10; break;
        //    case 3: Resistances.Poison = 10; break;
        //    case 4: Resistances.Energy = 10; break;
        //}
    }

    public Quiver(Serial serial) : base(serial)
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
        int v = reader.ReadInt();
    }
}
