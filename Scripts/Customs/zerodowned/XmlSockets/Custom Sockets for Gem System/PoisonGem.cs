using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PoisonGem : BaseSocketAugmentation, IMythicAugment
    {

        [Constructable]
        public PoisonGem() : base(0x4B48)
        {
            Name = "Pet Posion Immune Gem";
            Hue = 1272;
        }

        public PoisonGem(Serial serial) : base(serial)
        {
        }

        public override int SocketsRequired { get { return 1; } }
        public override int Icon { get { return 0x9a8; } } // gump ID
        public override bool UseGumpArt { get { return true; } }
        public override int IconXOffset { get { return 15; } }
        public override int IconYOffset { get { return 15; } }


        public override string OnIdentify(Mobile from)
        {
            return "Pet Posion Immune Gem";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;
                XmlPoison xp = (XmlPoison)XmlAttach.FindAttachment(bc, typeof(XmlPoison));
                
                if (xp == null)
                {
                    XmlAttach.AttachTo(bc, new XmlPoison(1));
                    return false;
                }
                if (xp != null)
                {
                    int currentImmune = xp.ImmuneLevel;                    
                    xp.Delete();

                    currentImmune += 4; // default 1                    
                    XmlAttach.AttachTo(bc, new XmlPoison(currentImmune));
                    bc.SetResistance(ResistanceType.Poison, 10, 20);
                }

            }
            else
                return false;

            return true;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            return (target is BaseCreature);
        }

        public override bool OnRecover(Mobile from, object target, int version)
        {
            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;
                XmlPoison xp = (XmlPoison)XmlAttach.FindAttachment(bc, typeof(XmlPoison));

                if (xp != null)
                {
                    int currentImmune = xp.ImmuneLevel;
                    xp.Delete();

                    currentImmune -= 1;
                    XmlAttach.AttachTo(bc, new XmlPoison(currentImmune));
                }
                else
                    from.SendMessage("This is an issue with attaching the gem, please contact staff for assistance.");

            }
            else
                return false;

            return true;
        }

        public override bool CanRecover(Mobile from, object target, int version)
        {
            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage(1161, "Add a socket & use the 'Content' option to attach this gem.");
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
