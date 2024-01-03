using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class DexOrb : BaseTalisman, Server.Engines.Craft.IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTailoring.CraftSystem; } }

        [Constructable]
        public DexOrb()
            : base(4246)
        {
            this.Attributes.BonusDex = 5;
            this.Name = "Dexterity Orb";
            this.LootType = LootType.Blessed;       
            this.Hue = Utility.RandomList(1150, 1175, 1109, 1194, 1161, 1910);
        }

        public DexOrb(Serial serial)
            : base(serial)
        {
        }        
        
        public override int InitMinHits
        {
            get
            {
                return 500;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 999;
            }
        }
        
        public override double DefaultWeight
        {
            get
            {
                return 5.0;
            }
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
