using System;
using Server;
using Server.Engines.Craft;
using System.Collections.Generic;

namespace Server.Items
{
    public class SewingMachine : CraftAddon
    {
        public override BaseAddonDeed Deed { get { return new SewingMachineDeed(Tools.Count > 0 ? Tools[0].UsesRemaining : 0); } }
        public override CraftSystem CraftSystem { get { return DefTailoring.CraftSystem; } }

        [Constructable]
        public SewingMachine(bool south, int uses)
        {
            if (south)
            {
                AddCraftComponent(new AddonToolComponent(CraftSystem, 39496, 39480, 1123504, uses, this), 0, 0, 0);
                AddComponent(new ToolDropComponent(39498, 1123522), -1, 0, 0);
            }
            else
            {
                AddCraftComponent(new AddonToolComponent(CraftSystem, 39497, 39488, 1123504, uses, this), 0, 0, 0);
                AddComponent(new ToolDropComponent(39498, 1123522), 0, 1, 0);
            }
        }

        public SewingMachine(Serial serial)
            : base(serial)
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

            int version = reader.ReadInt();
        }
    }

    public class SewingMachineDeed : CraftAddonDeed
    {
        public override int LabelNumber { get { return 1123504; } } // Sewing Machine
        public override BaseAddon Addon { get { return new SewingMachine(_South, UsesRemaining); } }

        private bool _South;

        [Constructable]
        public SewingMachineDeed() : this(9999)
        {
        }

        [Constructable]
        public SewingMachineDeed(int uses) : base(uses)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new SouthEastGump(s =>
                {
                    _South = s;
                    base.OnDoubleClick(from);
                }));
            }
        }

        public SewingMachineDeed(Serial serial)
            : base(serial)
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

            int version = reader.ReadInt();
        }
    }
}
