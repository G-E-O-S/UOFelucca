using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class IceGreenPetDye : Item
    {

        [Constructable]
        public IceGreenPetDye() : base(0xE2B)
        {
            Weight = 1.0;
            Movable = true;
            Hue = 1151;
            Name = "pet dye (Ice Green)";
        }

        public IceGreenPetDye(Serial serial) : base(serial)
        {


        }
        public override void OnDoubleClick(Mobile from)
        {

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendMessage("What do you wish to dye?");
                from.Target = new IceGreenDyeTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away. 
            }

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }


        private class IceGreenDyeTarget : Target
        {
            private readonly Mobile m_Owner;

            private readonly IceGreenPetDye m_Powder;

            public IceGreenDyeTarget(IceGreenPetDye charge) : base(10, false, TargetFlags.None)
            {
                m_Powder = charge;
            }

            protected override void OnTarget(Mobile from, object target)
            {

                if (target == from)
                {
                    from.SendMessage("This can only be used on pets.");
                }
                else if (target is PlayerMobile)
                {
                    from.SendMessage("You cannot dye them.");
                }
                else if (target is Item)
                {
                    from.SendMessage("You cannot dye that.");
                }
                else if (target is BaseCreature)
                {
                    BaseCreature c = (BaseCreature)target;
                    if (c.BodyValue == 400 || c.BodyValue == 401 && c.Controlled == false)
                    {
                        from.SendMessage("You cannot dye them.");
                    }
                    else if (c.ControlMaster != from && c.Controlled == false)
                    {
                        from.SendMessage("This is not your pet.");
                    }
                    else if (c.Controlled == true && c.ControlMaster == from)
                    {
                        c.Hue = 1151;
                        from.SendMessage(53, "Your pet has now been dyed.");
                        from.PlaySound(0x23E);
                        m_Powder.Delete();
                    }

                }
            }
        }
    }
}