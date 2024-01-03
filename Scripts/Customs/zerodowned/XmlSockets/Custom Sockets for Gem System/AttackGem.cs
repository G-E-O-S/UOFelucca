using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AttackGem : BaseSocketAugmentation, IMythicAugment
    {

        [Constructable]
        public AttackGem() : base(0x4B48)
        {
            Name = "Pet Area Attack Gem";
            Hue = 1266;
        }

        public AttackGem(Serial serial) : base(serial)
        {
        }

        public override int SocketsRequired { get { return 1; } }
        public override int Icon { get { return 0x9a8; } } // gump ID
        public override bool UseGumpArt { get { return true; } }
        public override int IconXOffset { get { return 15; } }
        public override int IconYOffset { get { return 15; } }

        public override string OnIdentify(Mobile from)
        {
            return "Pet Area Attack Gem";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;
                XmlAreaDamage xp = (XmlAreaDamage)XmlAttach.FindAttachment(bc, typeof(XmlAreaDamage));

                if (xp == null)
                {
                    XmlAttach.AttachTo(bc, new XmlAreaDamage(1)); // default 1
                    return false;
                }
                if (xp != null)
                {
                    int currentChance = xp.ChanceToTrigger;
                    xp.Delete();

                    currentChance += 1; // default 1
                    XmlAttach.AttachTo(bc, new XmlAreaDamage(currentChance));
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
                XmlAreaDamage xp = (XmlAreaDamage)XmlAttach.FindAttachment(bc, typeof(XmlAreaDamage));

                if (xp != null)
                {
                    int currentChance = xp.ChanceToTrigger;
                    xp.Delete();

                    currentChance -= 1;
                    XmlAttach.AttachTo(bc, new XmlAreaDamage(currentChance));
                }
                else
                    from.SendMessage("This is an issue with attaching the gem, please contact staff for assistance.");

            }
            else
                return false;

            return true;
        }

        public override void OnDoubleClick(Mobile from) 
        {
            from.SendMessage(1161,"Add a socket & use the 'Content' option to attach this gem.");
        }

        public override bool CanRecover(Mobile from, object target, int version)
        {
            return true;
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
