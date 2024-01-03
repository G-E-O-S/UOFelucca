using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    [FlipableAttribute(0x46A5, 0x46A6)]
    public class SmallChristmasPresent : BaseContainer
    {
        private bool _itemsDropped;

        [Constructable]
        public SmallChristmasPresent() : base(Utility.RandomBool() ? 0x46A5 : 0x46A6)
        {
            Name = "Small Christmas Present";
            GumpID = 0x777A;
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        private void DropRandomItem()
        {
            switch (Utility.Random(27))
            {
                case 0: DropItem(new ChristmasBandana()); break;
                case 1: DropItem(new ChristmasBonnet()); break;
                case 2: DropItem(new ChristmasCap()); break;
                case 3: DropItem(new ChristmasDoublet()); break;
                case 4: DropItem(new ChristmasFancyShirt()); break;
                case 5: DropItem(new ChristmasFeatheredHat()); break;
                case 6: DropItem(new ChristmasBodySash()); break;
                case 7: DropItem(new ChristmasBoots()); break;
                case 8: DropItem(new ChristmasCloak()); break;
                case 9: DropItem(new ChristmasFancyDress()); break;
                case 10: DropItem(new ChristmasFullApron()); break;
                case 11: DropItem(new ChristmasHalfApron()); break;
                case 12: DropItem(new ChristmasJesterHat()); break;
                case 13: DropItem(new ChristmasKilt()); break;
                case 14: DropItem(new ChristmasLongPants()); break;
                case 15: DropItem(new ChristmasPlainDress()); break;
                case 16: DropItem(new ChristmasShirt()); break;
                case 17: DropItem(new ChristmasShoes()); break;
                case 18: DropItem(new ChristmasShortPants()); break;
                case 19: DropItem(new ChristmasSkirt()); break;
                case 20: DropItem(new ChristmasSkullcap()); break;
                case 21: DropItem(new ChristmasSurcoat()); break;
                case 22: DropItem(new ChristmasTricorneHat()); break;
                case 23: DropItem(new ChristmasTunic()); break;
                case 24: DropItem(new ChristmasWideBrimHat()); break;
                case 25: DropItem(new ChristmasWizardsHat()); break;
                case 26: DropItem(new ChristmasBodySash()); break;
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

        public SmallChristmasPresent(Serial serial) : base(serial)
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
