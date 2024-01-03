using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Prompts;
using System.IO;
using Server.ContextMenus;
using Server.Targeting;

namespace Server.Mobiles
{
    public class Butler : Mobile
    {
        public ButlerDeed m_ButlerDeed;
        public Mobile m_Owner;

        public List<Mobile> Friends { get; set; }

        public Dictionary<int, Dictionary<InventoryType, int>> RestockPresets { get; set; }

        [Constructable]
        public Butler(ButlerDeed butlerDeed, Mobile owner)
        {
            m_ButlerDeed = butlerDeed;
            m_Owner = owner;

            InitStats(100, 100, 25);

            Name = "Alfred";
            Title = "the Butler";
            NameHue = 1266;
            Hue = 0;

            Body = 0x190;

            AddItem(new FancyShirt(1150));
            AddItem(new LongPants(1175));
            AddItem(new Boots(1175));
            AddItem(new Doublet(1175));
            AddItem(new FeatheredHat(1175));

            HairItemID = 0x203B;
            HairHue = 1175;

            Direction = Direction.South;

            Friends = new List<Mobile>();

            SetupRestockPresets();
        }

        public Butler(Serial serial)
            : base(serial)
        {

        }

        public override bool CanBeDamaged() { return false; }
        public override bool CanBeHarmful(IDamageable damageable, bool message, bool ignoreOurBlessedness) { return false; }
        public override bool CanBeHarmedBy(Mobile from, bool message) { return false; }

        private void SetupRestockPresets()
        {
            RestockPresets = new Dictionary<int, Dictionary<InventoryType, int>>();

            for (int preset = 1; preset <= 8; preset++)
            {
                Dictionary<InventoryType, int> RestockPreset = new Dictionary<InventoryType, int>();

                foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
                {
                    RestockPreset.Add(type, 0);
                }

                RestockPresets.Add(preset, RestockPreset);
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (m_Owner == null || m_Owner.Deleted)
            {
                from.SendMessage(0, "Alfred: My owner has abandoned me! I shall come with you " + from.Name + ".");
                Redeed(from);
                return;
            }

            base.GetContextMenuEntries(from, list);

            if (m_Owner.Account == from.Account || Friends.Contains(from))
            {
                list.Add(new SimpleContextMenuEntry(from, 1154277, m => // Open Inventory
                {
                    OpenInventory(from);
                }, enabled: true));

                list.Add(new SimpleContextMenuEntry(from, 3006230, m => // Refill from stock
                {
                    RefillFromStock(1, from);
                }, enabled: true));
            }

            if (m_Owner.Account == from.Account)
            {
                list.Add(new SimpleContextMenuEntry(from, 1060676, m => // Grant Access
                {
                    GrantAccess(from);
                }, enabled: true));

                list.Add(new SimpleContextMenuEntry(from, 1060677, m => // Revoke Access
                {
                    RevokeAccess(from);
                }, enabled: Friends.Count > 0));

                list.Add(new SimpleContextMenuEntry(from, 1151586, m => // Rotate
                {
                    Rotate();
                    Animate(32, 5, 1, true, false, 0); // bow
                }, enabled: true));

                list.Add(new SimpleContextMenuEntry(from, 1151601, m => // Redeed
                {
                    Redeed(from);
                }, enabled: true));
            }
        }

        protected List<Item> GetItems()
        {
            List<Item> list = new List<Item>();

            foreach (Item item in Items)
            {
                if (item.Movable
                    && item != Backpack
                    && item.Layer != Layer.Hair
                    && item.Layer != Layer.FacialHair)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public void Redeed(Mobile from)
        {
            from.AddToBackpack(m_ButlerDeed);
            m_ButlerDeed.Movable = true;
            m_ButlerDeed.Visible = true;

            List<Item> list = GetItems();
            Bag bag = new Bag();
            foreach (Item item in list)
            {
                bag.DropItem(item);
            }

            from.AddToBackpack(bag);
            m_ButlerDeed.RemoveButler();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Owner == null || m_Owner.Deleted)
            {
                from.SendMessage(0, "Alfred: My owner has abandoned me! I shall come with you " + from.Name + ".");
                Redeed(from);
                return;
            }

            if (m_Owner.Account == from.Account || Friends.Contains(from))
            {
                OpenInventory(from);
            }
        }

        private void OpenInventory(Mobile from)
        {
            SayTo(from, "Have a look!");
            from.CloseGump(typeof(ButlerInventoryGump));
            from.SendGump(new ButlerInventoryGump(from, this, 1));
        }

        public void RefillFromStock(int preset, Mobile from)
        {
            foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
            {
                RefillItem(preset, from, type);
            }

            SayTo(from, "I have restocked your backpack from preset " + preset + ".");
        }

        public void SetRestockPresetValue(int preset, InventoryType type, int Amount)
        {
            RestockPresets[preset][type] = Amount;
        }

        public int GetRestockPresetValue(int preset, InventoryType type)
        {
            return RestockPresets[preset][type];
        }

        private void RefillItem(int preset, Mobile from, InventoryType type)
        {
            // If preset value is 0, do nothing.
            int AmountToRestock = GetRestockPresetValue(preset, type);
            if (AmountToRestock == 0)
            {
                return;
            }

            // If inventory amount is 0, do nothing.
            int AmountInInventory = GetInventoryAmount(type);
            if (AmountInInventory == 0)
            {
                return;
            }

            // Refill existing item in player's backpack from inventory.
            if (type != InventoryType.GreaterExplosionPotion)  // Remove if explosion potions are stackable.
            {
                Item RefillItem = SearchBackpack(from, type);
                if (RefillItem is object)
                {
                    if (RefillItem.Amount < AmountToRestock)
                    {
                        AmountToRestock = AmountToRestock - RefillItem.Amount;

                        if (AmountToRestock > AmountInInventory)
                        {
                            AmountToRestock = AmountInInventory;
                        }

                        RefillItem.Amount += AmountToRestock;
                        SubtractInventoryAmount(type, AmountToRestock);
                    }

                    return;
                }
            }
            else // Remove if explosion potions are stackable.
            {
                AmountToRestock = AmountToRestock - CountExplosionPotions(from);
            }

            // Create new item if it doesn't already exist in player's backpack.
            if (AmountToRestock > AmountInInventory)
            {
                AmountToRestock = AmountInInventory;
            }

            AddToBackpack(from, type, AmountToRestock);
            SubtractInventoryAmount(type, AmountToRestock);
        }

        private void AddToBackpack(Mobile from, InventoryType type, int AmountToRestock)
        {
            switch (type)
            {
                case InventoryType.MandrakeRoot:
                    from.AddToBackpack(new MandrakeRoot(AmountToRestock));
                    break;
                case InventoryType.Bloodmoss:
                    from.AddToBackpack(new Bloodmoss(AmountToRestock));
                    break;
                case InventoryType.BlackPearl:
                    from.AddToBackpack(new BlackPearl(AmountToRestock));
                    break;
                case InventoryType.Garlic:
                    from.AddToBackpack(new Garlic(AmountToRestock));
                    break;
                case InventoryType.Ginseng:
                    from.AddToBackpack(new Ginseng(AmountToRestock));
                    break;
                case InventoryType.Nightshade:
                    from.AddToBackpack(new Nightshade(AmountToRestock));
                    break;
                case InventoryType.SulfurousAsh:
                    from.AddToBackpack(new SulfurousAsh(AmountToRestock));
                    break;
                case InventoryType.SpidersSilk:
                    from.AddToBackpack(new SpidersSilk(AmountToRestock));
                    break;
                case InventoryType.GreaterExplosionPotion:
                    // Use below code if explosion potions are stackable.
                    // GreaterExplosionPotion gePotion = new GreaterExplosionPotion();
                    // gePotion.Amount = AmountToRestock;
                    // from.AddToBackpack(gePotion);

                    // Use below code if explosion potions are not stachable.
                    for (int i = 0; i < AmountToRestock; i++)
                    {
                        from.AddToBackpack(new GreaterExplosionPotion());
                    }
                    break;
                case InventoryType.GreaterCurePotion:
                    GreaterCurePotion gcPotion = new GreaterCurePotion();
                    gcPotion.Amount = AmountToRestock;
                    from.AddToBackpack(gcPotion);
                    break;
                case InventoryType.GreaterHealPotion:
                    GreaterHealPotion ghPotion = new GreaterHealPotion();
                    ghPotion.Amount = AmountToRestock;
                    from.AddToBackpack(ghPotion);
                    break;
                case InventoryType.TotalRefreshPotion:
                    TotalRefreshPotion trPotion = new TotalRefreshPotion();
                    trPotion.Amount = AmountToRestock;
                    from.AddToBackpack(trPotion);
                    break;
                case InventoryType.GreaterStrengthPotion:
                    GreaterStrengthPotion gsPotion = new GreaterStrengthPotion();
                    gsPotion.Amount = AmountToRestock;
                    from.AddToBackpack(gsPotion);
                    break;
                case InventoryType.GreaterAgilityPotion:
                    GreaterAgilityPotion gaPotion = new GreaterAgilityPotion();
                    gaPotion.Amount = AmountToRestock;
                    from.AddToBackpack(gaPotion);
                    break;
                case InventoryType.DeadlyPoisonPotion:
                    DeadlyPoisonPotion dpPotion = new DeadlyPoisonPotion();
                    dpPotion.Amount = AmountToRestock;
                    from.AddToBackpack(dpPotion);
                    break;
                case InventoryType.Bolt:
                    from.AddToBackpack(new Bolt(AmountToRestock));
                    break;
                case InventoryType.Arrow:
                    from.AddToBackpack(new Arrow(AmountToRestock));
                    break;
                case InventoryType.Bandage:
                    from.AddToBackpack(new Bandage(AmountToRestock));
                    break;
            }
        }

        private Item SearchBackpack(Mobile from, InventoryType type)
        {
            Container pack = from.Backpack;

            // Search top level player's backpack first
            foreach (Item item in pack.Items)
            {
                if (String.Equals(item.GetType().Name, type.ToString()))
                {
                    return item;
                }
            }

            // Search any unlocked / untrapped containers in player's backpack
            foreach (Item container in pack.Items)
            {
                if (container is BaseContainer)
                {
                    if (container is LockableContainer && ((LockableContainer)container).Locked)
                    {
                        continue;
                    }

                    if (container is TrapableContainer && ((TrapableContainer)container).TrapType != TrapType.None)
                    {
                        continue;
                    }

                    foreach (Item item in container.Items)
                    {
                        if (String.Equals(item.GetType().Name, type.ToString()))
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        // Remove if explosion potions are stackable.
        private int CountExplosionPotions(Mobile from)
        {
            int count = 0;
            Container pack = from.Backpack;

            // Search top level player's backpack first
            foreach (Item item in pack.Items)
            {
                if (item is GreaterExplosionPotion)
                {
                    count++;
                }
            }

            // Search any unlocked / untrapped containers in player's backpack
            foreach (Item container in pack.Items)
            {
                if (container is BaseContainer)
                {
                    if (container is LockableContainer && ((LockableContainer)container).Locked)
                    {
                        continue;
                    }

                    if (container is TrapableContainer && ((TrapableContainer)container).TrapType != TrapType.None)
                    {
                        continue;
                    }

                    foreach (Item item in container.Items)
                    {
                        if (item is GreaterExplosionPotion)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private void GrantAccess(Mobile from)
        {
            SayTo(from, "Who would you like to add?");
            from.Target = new AddFriendTarget(this);
        }

        private void RevokeAccess(Mobile from)
        {
            SayTo(from, "Who would you like to remove?");
            from.CloseGump(typeof(ButlerRevokeGump));
            from.SendGump(new ButlerRevokeGump(from, this, 1));
        }

        private bool AddFriend(Mobile m)
        {
            if (m_Owner.Account == m.Account || Friends.Contains(m))
            {
                return false;
            }

            Friends.Add(m);
            return true;
        }

        private void Rotate()
        {
            switch (Direction)
            {
                case Direction.North:
                    Direction = Direction.Right;
                    return;
                case Direction.Right:
                    Direction = Direction.East;
                    return;
                case Direction.East:
                    Direction = Direction.Down;
                    return;
                case Direction.Down:
                    Direction = Direction.South;
                    return;
                case Direction.South:
                    Direction = Direction.Left;
                    return;
                case Direction.Left:
                    Direction = Direction.West;
                    return;
                case Direction.West:
                    Direction = Direction.Up;
                    return;
                case Direction.Up:
                    Direction = Direction.North;
                    return;
            }
        }

        public override bool AllowEquipFrom(Mobile from)
        {
            if (m_Owner.Account == from.Account)
            {
                return true;
            }

            return base.AllowEquipFrom(from);
        }

        public override bool CheckNonlocalLift(Mobile from, Item item)
        {
            if (m_Owner.Account == from.Account)
            {
                return true;
            }

            return base.CheckNonlocalLift(from, item);
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (m_Owner == null || m_Owner.Deleted)
            {
                from.SendMessage(0, "Alfred: My owner has abandoned me! I shall come with you " + from.Name + ".");
                Redeed(from);
                return false;
            }

            if ((m_Owner.Account == from.Account || Friends.Contains(from))
                && dropped is BaseContainer)
            {
                if (dropped is LockableContainer && ((LockableContainer)dropped).Locked)
                {
                    SayTo(from, "Call the locksmith!");
                    return false;
                }

                if (dropped is TrapableContainer && ((TrapableContainer)dropped).TrapType != TrapType.None)
                {
                    SayTo(from, "That's a nasty trap in there.");
                    return false;
                }

                AddItemsToInventory((BaseContainer)dropped);
                SayTo(from, "I have added the contents to my inventory.");
            }

            return false;
        }

        private void AddItemsToInventory(BaseContainer container)
        {
            List<Item> list = container.Items;

            int index = 0;
            while (index < list.Count)
            {
                Item item = list[index];

                bool isCommodityDeed = false;
                if (item is CommodityDeed
                    && ((CommodityDeed)item).Commodity is object)
                {
                    item = ((CommodityDeed)item).Commodity;
                    isCommodityDeed = true;
                }

                if (IsValidItem(item))
                {
                    AddItemToInventory(item, container, isCommodityDeed, list[index]);
                    index = 0;
                }
                else
                {
                    index++;
                }
            }

            container.UpdateTotals();
        }

        private void AddItemToInventory(Item item, BaseContainer container, bool isCommodityDeed, Item commodityDeed)
        {
            // Add item amount to existing stock.
            InventoryType type = (InventoryType)Enum.Parse(typeof(InventoryType), item.GetType().Name);
            m_ButlerDeed.Inventory[type] += item.Amount;

            if (isCommodityDeed)
            {
                // Remove commodity deed from original container and delete.
                container.Items.Remove(commodityDeed);
                commodityDeed.Delete();
            }
            else
            {
                // Remove item from original container and delete.
                container.Items.Remove(item);
                item.Delete();
            }
        }

        private bool IsValidItem(Item item)
        {
            return Enum.IsDefined(typeof(InventoryType), item.GetType().Name);
        }

        public void SubtractInventoryAmount(InventoryType type, int Amount)
        {
            m_ButlerDeed.Inventory[type] -= Amount;
        }

        public int GetInventoryAmount(InventoryType type)
        {
            return m_ButlerDeed.Inventory[type];
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.WriteItem(m_ButlerDeed);
            writer.WriteMobile(m_Owner);

            writer.WriteMobileList(Friends, true);

            for (int preset = 1; preset <= 8; preset++)
            {
                foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
                {
                    writer.Write(RestockPresets[preset][type]);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_ButlerDeed = (ButlerDeed)reader.ReadItem();
            m_Owner = reader.ReadMobile();

            Friends = reader.ReadStrongMobileList();

            RestockPresets = new Dictionary<int, Dictionary<InventoryType, int>>();
            for (int preset = 1; preset <= 8; preset++)
            {
                Dictionary<InventoryType, int> RestockPreset = new Dictionary<InventoryType, int>();
                foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
                {
                    RestockPreset.Add(type, reader.ReadInt());
                }

                RestockPresets.Add(preset, RestockPreset);
            }
        }

        private class AddFriendTarget : Target
        {
            private readonly Butler m_Butler;

            public AddFriendTarget(Butler butler)
                : base(50, false, TargetFlags.None)
            {
                m_Butler = butler;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    bool playerAdded = m_Butler.AddFriend((Mobile)targeted);

                    if (playerAdded)
                    {
                        m_Butler.SayTo(from, "Player has been granted access.");
                    }
                    else
                    {
                        m_Butler.SayTo(from, "This player already as access.");
                    }
                }
                else
                {
                    m_Butler.SayTo(from, "That's not a player.");
                }
            }
        }
    }
}
