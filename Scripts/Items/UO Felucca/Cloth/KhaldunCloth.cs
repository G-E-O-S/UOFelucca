using System;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0x1765, 0x1767)]
    public class KhaldunCloth : Item, IScissorable, IDyable, ICommodity
    {
        [Constructable]
        public KhaldunCloth()
            : this(1)
        {
        }

        [Constructable]
        public KhaldunCloth(int amount)
            : base(0x1767)
        {
            this.Stackable = true;
            this.Amount = amount;
            this.Hue = 2756;
        }

        public KhaldunCloth(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get
            {
                return 0.01;
            }
        }
        TextDefinition ICommodity.Description
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return true;
            }
        }
        public bool Dye(Mobile from, DyeTub sender)
        {
            if (this.Deleted)
                return false;

            this.Hue = sender.DyedHue;

            return false;
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

        public override void OnSingleClick(Mobile from)
        {
            int number = (this.Amount == 1) ? 1049124 : 1049123;

            from.Send(new MessageLocalized(this.Serial, this.ItemID, MessageType.Regular, 0x3B2, 3, number, "", this.Amount.ToString()));
        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (this.Deleted || !from.CanSee(this))
                return false;

            base.ScissorHelper(from, new Bandage(), 1);

            return true;
        }
    }
}
