using System;
using Server;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x2683, 0x2684)]
    public class HoodedRobe : BaseOuterTorso
    {
        private bool m_DonationItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Donation
        { get { return m_DonationItem; } set { m_DonationItem = value; } }

        [Constructable]
        public HoodedRobe()
            : base(0x2683)
        {
            Weight = 1.0;
            Name = "Hooded Robe";
            Layer = Layer.OuterTorso;
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (Parent != m)
            {
                m.SendMessage("You must be wearing the robe to use it!");
            }
            else
            {
                if (ItemID == 0x2683 || ItemID == 0x2684)
                {
                    m.SendMessage("You lower the hood.");
                    m.PlaySound(0x57);
                    ItemID = 0x1F03;
                    m.NameMod = null;
                    m.RemoveItem(this);
                    m.EquipItem(this);
                }
                else if (ItemID == 0x1F03 || ItemID == 0x1F04)
                {
                    m.SendMessage("You pull the hood over your head.");
                    m.PlaySound(0x57);
                    ItemID = 0x2683;
                    m.RemoveItem(this);
                    m.EquipItem(this);

                }
            }
        }

        //public override bool OnEquip(Mobile from)
        //{
        //    if (ItemID == 0x2683)
        //    {
        //        if (from.AccessLevel == AccessLevel.Counselor)
        //            from.NameMod = "An Unknown Figure";
        //        else if (from.AccessLevel == AccessLevel.GameMaster)
        //            from.NameMod = "A Mysterious Figure";
        //        else if (from.AccessLevel == AccessLevel.Seer)
        //            from.NameMod = "A Godly Figure";
        //        else if (from.AccessLevel == AccessLevel.Administrator)
        //            from.NameMod = "A Legendary Figure";
        //        else if (from.AccessLevel == AccessLevel.Developer)
        //            from.NameMod = "Sage of the Lands";
        //        else if (from.AccessLevel == AccessLevel.Owner)
        //            from.NameMod = "Ancient of the Lands";
        //        else
        //            from.NameMod = "A Shrouded Figure";

        //        from.DisplayGuildTitle = false;
        //        from.Criminal = false;
        //    }
        //    return base.OnEquip(from);
        //}

        //public override void OnRemoved(Object o)
        //{
        //    if (o is Mobile)
        //    {
        //        ((Mobile)o).NameMod = null;
        //    }
        //    if (o is Mobile && ((Mobile)o).Kills >= 5)
        //    {
        //        ((Mobile)o).Criminal = true;
        //    }
        //    if (o is Mobile && ((Mobile)o).GuildTitle != null)
        //    {
        //        ((Mobile)o).DisplayGuildTitle = true;
        //    }

        //    base.OnRemoved(o);
        //}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_DonationItem == true)
                list.Add("Donation Item");
        }

        public HoodedRobe(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((bool)m_DonationItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_DonationItem = reader.ReadBool();
        }
    }
}
