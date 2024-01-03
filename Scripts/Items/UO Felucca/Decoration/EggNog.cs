using System;

namespace Server.Items
{
    public class EggNog : BaseAddon
    {
        [Constructable]
        public EggNog()
        {
            AddComponent(new LocalizedAddonComponent(0xA6CB, 1007150), 1, 1, 0);
            Name = "Eggnog";
        }

        public EggNog(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new EggNogDeed(); } }

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

    public class EggNogDeed : BaseAddonDeed
    {
        public override int LabelNumber { get { return 1153417; } } // Drink Up!

        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public EggNogDeed()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
            Name = "Eggnog";
        }

        public EggNogDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new EggNog(); } }

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
