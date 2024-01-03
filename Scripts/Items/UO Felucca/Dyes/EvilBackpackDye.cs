using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class EvilBackpackDye : Item
    {
        [Constructable]
        public EvilBackpackDye()
            : base(0x0EFB)
        {
            Hue = 2068;
            LootType = LootType.Regular;
            Name = "Backpack Dye";
        }

        public EvilBackpackDye(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (IsChildOf(m.Backpack))
            {
                var backpack = m.Backpack as BaseContainer;

                if (backpack != null && backpack is Backpack)
                {
                    // Apply the dye's hue to the player's backpack
                    backpack.Hue = this.Hue;

                    // Play the success sound
                    m.PlaySound(0x23E);

                    // Notify the player that the dye has been applied
                    m.SendMessage("You have dyed your backpack.");

                    // Delete the backpack dye item since it's a one-time use
                    this.Delete();
                }
            }
            else
            {
                m.SendLocalizedMessage(1042010); // You must have the object in your backpack to use it.
            }
        }

        private class InternalTarget : Target
        {
            private readonly BackpackDye m_Dye;

            public InternalTarget(BackpackDye dye) : base(1, false, TargetFlags.None)
            {
                m_Dye = dye;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseContainer container)
                {
                    if (container is Backpack)
                    {
                        // Apply the dye's hue to the targeted backpack
                        container.Hue = m_Dye.Hue;

                        // Notify the player that the dye has been applied
                        from.SendMessage("You have dyed the backpack!");

                        // Delete the backpack dye item since it's a one-time use
                        m_Dye.Delete();
                    }
                    else
                    {
                        from.SendMessage("You can only dye a backpack.");
                    }
                }
                else
                {
                    from.SendMessage("You can only dye a backpack.");
                }
            }
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
