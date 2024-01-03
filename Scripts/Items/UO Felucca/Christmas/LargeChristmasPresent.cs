using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{    
    public class LargeChristmasPresent : BaseContainer
    {
        private bool _itemsDropped;

        [Constructable]
        public LargeChristmasPresent() : base(0x46A7)
        {
            Name = "Large Christmas Present";
            GumpID = 0x777A;
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        private void DropRandomItem()
        {
            switch (Utility.Random(28))
            {
                case 0: DropItem(new ChristmasSandals()); break;
                case 1: DropItem(new ChristmasKasa()); break;
                case 2: DropItem(new ChristmasRobe()); break;
                case 3: DropItem(new ChristmasShroud()); break;
                case 4: DropItem(new ChristmasHoodedRobe()); break;
                case 5: DropItem(new PureWhiteCloth(2)); break;
                case 6: DropItem(new FallonCloth(4)); break;
                case 7: DropItem(new ChristmasStallEastAddonDeed()); break;
                case 8: DropItem(new ChristmasStallSouthAddonDeed()); break;
                case 9: DropItem(new TeddyBear1()); break;
                case 10: DropItem(new TeddyBear2()); break;
                case 11: DropItem(new Icicles2023()); break;
                case 12: DropItem(new NutcrackerLarge2023()); break;
                case 13: DropItem(new BearTopiary2023()); break;
                case 14: DropItem(new LlamaTopiary2023()); break;
                case 15: DropItem(new ReindeerTopiary2023()); break;
                case 16: DropItem(new HolidayTreeDeed()); break;
                case 17: DropItem(new JollyHolidayTreeDeed()); break;
                case 18: DropItem(new HolidaysSign()); break;
                case 19: DropItem(new WinterGiftPackage2023()); break;
                case 20: DropItem(new SnowTreeDeed()); break;
                case 21: DropItem(new TeddyBear3()); break;
                case 22: DropItem(new Gnome()); break;
                case 23: DropItem(new CrystalFigure1()); break;
                case 24: DropItem(new CrystalFigure2()); break;
                case 25: DropItem(new CrystalFigure3()); break;
                case 26: DropItem(new CrystalFigure4()); break;
                case 27: DropItem(new CrystalFigure5()); break;
            }
            _itemsDropped = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (!_itemsDropped)
            {
                DropRandomItem();
            }

            if (from.IsStaff() || RootParent is PlayerVendor ||
                (from.InRange(GetWorldLocation(), 2) && (Parent != null || (Z >= from.Z - 8 && Z <= from.Z + 16))))
            {
                Open(from);
                from.PlaySound(0x669);
                from.Say("*Ho-Ho-Ho!*");
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
        }

        public LargeChristmasPresent(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(1); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
