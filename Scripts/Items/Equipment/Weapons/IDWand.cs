using System;

namespace Server.Items
{
    public class IDWand : BaseWand
    {
        [Constructable]
        public IDWand()
            : base(WandEffect.Identification, 175, 200)
        {
        }

        public IDWand(Serial serial)
            : base(serial)
        {
        }

        public override TimeSpan GetUseDelay
        {
            get
            {
                return TimeSpan.Zero;
            }
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

        public override bool OnWandTarget(Mobile from, object o)
        {
            if (o is BaseWeapon)
                ((BaseWeapon)o).Identified = true;
            else if (o is BaseArmor)
                ((BaseArmor)o).Identified = true;
            else if (o is BaseContainer)
                IdentifyContainedItems(from, o as BaseContainer);

            if (!Core.AOS && o is Item)
                ((Item)o).OnSingleClick(from);

            return (o is Item);
        }

        private void IdentifyContainedItems(Mobile from, BaseContainer container)
        {
            if (container != null && container.Items.Count > 0)
            {
                if (container is ILockable && ((ILockable)container).Locked)
                    from.SendMessage("You need to unlock the container first.");
                else
                {
                    var undefinedItems = container.Items.FindAll(x => (x is BaseWeapon && !(x as BaseWeapon).Identified)
                   || (x is BaseArmor && !(x as BaseArmor).Identified));

                    foreach (var item in undefinedItems)
                    {
                        if (item is BaseWeapon)
                            ((BaseWeapon)item).Identified = true;
                        else if (item is BaseArmor)
                            ((BaseArmor)item).Identified = true;

                        if (!Core.AOS)
                            item.OnSingleClick(from);

                        Charges--;

                        if (Charges <= 0)
                        {
                            from.SendMessage("You don't have enought charges to identify other items in this container!");
                            break;
                        }

                        //else
                        //{
                        //    from.SendLocalizedMessage(500353); // You are not certain...
                        //}
                    }
                }
            }

        }
    }
}
