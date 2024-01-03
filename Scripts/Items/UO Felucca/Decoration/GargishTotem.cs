using System;

namespace Server.Items
{
    public class GargishTotem : BaseAddon
    {
        [Constructable]
        public GargishTotem()
        {
            AddComponent(new LocalizedAddonComponent(0xA725, 1156294), 1, 1, 0);
            Name = "Gargish Totem";
        }

        public GargishTotem(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new GargishTotemDeed(); } }

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

    public class GargishTotemDeed : BaseAddonDeed
    {
        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public GargishTotemDeed()
        {
            LootType = LootType.Blessed;
            Hue = 1161;
            Name = "Gargish Totem";
        }

        public GargishTotemDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new GargishTotem(); } }

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
