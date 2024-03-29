using System;
using Server;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Engines.VvV
{
    public class WrongTileAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new WrongTileDeed(); } }

        public TileType TileType { get; set; }

        private int offset;

        [Constructable]
        public WrongTileAddon(TileType type)
        {
            TileType = type;

            offset = 0;

            if (type != TileType.North)
            {
                offset = 4;
            }

            AddComponent(new AddonComponent(39412 + offset), 0, 0, 0);
            AddComponent(new AddonComponent(39413 + offset), 1, 0, 0);
            AddComponent(new AddonComponent(39414 + offset), 0, 1, 0);
            AddComponent(new AddonComponent(39415 + offset), 1, 1, 0);

            //AddComponent(new AddonComponent(39428 + offset), 0, 0, 0);
            //AddComponent(new AddonComponent(39429 + offset), 1, 0, 0);
            //AddComponent(new AddonComponent(39430 + offset), 0, 1, 0);
            //AddComponent(new AddonComponent(39431 + offset), 1, 1, 0);
        }

        public WrongTileAddon(Serial serial)
            : base(serial)
        {
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

    public class WrongTileDeed : BaseAddonDeed, IRewardOption
    {
        public override BaseAddon Addon { get { return new WrongTileAddon(TileType); } }
        public override int LabelNumber { get { return 1155523; } } // Wrong Tile

        public TileType TileType { get; set; }

        [Constructable]
        public WrongTileDeed()
        {
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(RewardOptionGump));
                from.SendGump(new RewardOptionGump(this));
            }
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.       	
        }

        public WrongTileDeed(Serial serial)
            : base(serial)
        {
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

        public void GetOptions(RewardOptionList list)
        {
            list.Add((int)TileType.North, "Wrong (North)");
            list.Add((int)TileType.West, "Wrong (East)");
        }


        public void OnOptionSelected(Mobile from, int choice)
        {
            TileType = (TileType)choice;

            if (!Deleted)
                base.OnDoubleClick(from);
        }
    }
}
