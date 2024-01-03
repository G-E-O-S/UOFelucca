using System;

namespace Server.Items
{
    public class DaemonicTempleTorch : Torch
    {
        [Constructable]
        public DaemonicTempleTorch()
            : base()
        {
            this.Hue = 1910;
            this.LootType = LootType.Blessed;
        }

        public DaemonicTempleTorch(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Daemonic Temple";
            }
        }
        public override bool AllowEquipedCast(Mobile from)
        {
            return true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060482); // Spell Channeling
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
