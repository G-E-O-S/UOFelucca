/////////////////////////////////////////////////
//
// Automatically generated by the
// AddonGenerator script by Arya
//
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TreasurePile03Addon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new TreasurePile03AddonDeed();
            }
        }

        [Constructable]
        public TreasurePile03Addon()
        {
            AddonComponent ac = null;
            ac = new AddonComponent(6995);
            AddComponent(ac, 0, 1, 0);
            ac = new AddonComponent(6997);
            AddComponent(ac, -1, 1, 0);
            ac = new AddonComponent(6998);
            AddComponent(ac, -1, 0, 0);
            ac = new AddonComponent(6999);
            AddComponent(ac, -1, -1, 0);
            ac = new AddonComponent(7000);
            AddComponent(ac, 0, -1, 0);
            ac = new AddonComponent(7001);
            AddComponent(ac, 0, 0, 0);
            ac = new AddonComponent(7002);
            AddComponent(ac, 1, -1, 0);
            ac = new AddonComponent(7003);
            AddComponent(ac, 1, 0, 0);
            ac = new AddonComponent(6996);
            AddComponent(ac, 1, 1, 0);

        }

        public TreasurePile03Addon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TreasurePile03AddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new TreasurePile03Addon();
            }
        }

        [Constructable]
        public TreasurePile03AddonDeed()
        {
            Name = "TreasurePile03";
        }

        public TreasurePile03AddonDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
