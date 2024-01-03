using Server;
using Server.Items;

using System.Linq;

using Xanthos.Interfaces;

namespace Xanthos.ShrinkSystem
{
    [Flipable(0x14E8, 0x14E7)]
    public class NewHitchingPost : AddonComponent, IShrinkTool
    {
        private int m_ShrinkCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges
        {
            get { return m_ShrinkCharges; }
            set
            {
                m_ShrinkCharges = value;

                if (m_ShrinkCharges == 0 && Addon == null && World.Loaded)
                    Delete();
                else
                    InvalidateProperties();
            }
        }

        bool IShrinkTool.DeleteWhenEmpty { get => World.Loaded && Addon == null; set { } }

        protected virtual bool DeleteIfUnlinked => false;

        public override bool ForceShowProperties => ObjectPropertyList.Enabled;

        [Constructable]
        public NewHitchingPost()
            : this(0x14E7)
        { }

        [Constructable]
        public NewHitchingPost(int itemID)
            : this(itemID, ShrinkConfig.ShrinkCharges)
        { }

        [Constructable]
        public NewHitchingPost(int itemID, int charges)
            : base(itemID)
        {
            m_ShrinkCharges = charges;
        }

        public NewHitchingPost(Serial serial)
            : base(serial)
        { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_ShrinkCharges >= 0)
                list.Add(1060658, "Charges\t{0}", m_ShrinkCharges.ToString());
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Movable && !IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else
                ShrinkTarget.Begin(from, this);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write(m_ShrinkCharges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();

            m_ShrinkCharges = reader.ReadInt();
        }
    }

    #region Addons

    public class NewHitchingPostEastAddon : BaseAddon
    {
        private int m_ShrinkCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges => Deleted ? m_ShrinkCharges : Components.OfType<IShrinkTool>().Sum(c => c.ShrinkCharges);

        public override BaseAddonDeed Deed => new NewHitchingPostEastDeed(ShrinkCharges);

        public override bool RetainDeedHue => true;

        [Constructable]
        public NewHitchingPostEastAddon()
            : this(ShrinkConfig.ShrinkCharges)
        { }

        [Constructable]
        public NewHitchingPostEastAddon(int charges)
        {
            AddComponent(new NewHitchingPost(0x14E7, m_ShrinkCharges = charges), 0, 0, 0);
        }

        public NewHitchingPostEastAddon(Serial serial)
            : base(serial)
        { }

        public override void OnChop(Mobile from)
		{
            m_ShrinkCharges = ShrinkCharges;

            //base.OnChop(from, tool);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }

    public class NewHitchingPostSouthAddon : BaseAddon
    {
        private int m_ShrinkCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges => Deleted ? m_ShrinkCharges : Components.OfType<IShrinkTool>().Sum(c => c.ShrinkCharges);

        public override BaseAddonDeed Deed => new NewHitchingPostSouthDeed(ShrinkCharges);

        public override bool RetainDeedHue => true;

        [Constructable]
        public NewHitchingPostSouthAddon()
            : this(ShrinkConfig.ShrinkCharges)
        { }

        [Constructable]
        public NewHitchingPostSouthAddon(int charges)
        {
            AddComponent(new NewHitchingPost(0x14E8, m_ShrinkCharges = charges), 0, 0, 0);
        }

        public NewHitchingPostSouthAddon(Serial serial)
            : base(serial)
        { }

        public override void OnChop(Mobile from)
		{
            m_ShrinkCharges = ShrinkCharges;

            //base.OnChop(from, tool);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }

    #endregion

    #region Deeds

    public class NewHitchingPostEastDeed : BaseAddonDeed
    {
        private int m_ShrinkCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges
        {
            get { return m_ShrinkCharges; }
            set
            {
                m_ShrinkCharges = value;

                InvalidateProperties();
            }
        }

        public override BaseAddon Addon => new NewHitchingPostEastAddon(m_ShrinkCharges);

        [Constructable]
        public NewHitchingPostEastDeed()
            : this(ShrinkConfig.ShrinkCharges)
        { }

        [Constructable]
        public NewHitchingPostEastDeed(int charges)
        {
            m_ShrinkCharges = charges;

            Name = "Hitching Post (East)";
        }

        public NewHitchingPostEastDeed(Serial serial)
            : base(serial)
        { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_ShrinkCharges >= 0)
                list.Add(1060658, "Charges\t{0}", m_ShrinkCharges.ToString());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(m_ShrinkCharges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var v = reader.ReadInt();

            m_ShrinkCharges = reader.ReadInt();

            //if (v > 0 && Insensitive.Contains(Tooltip1, "Charges"))
            //    Tooltip1 = null;
        }
    }

    public class NewHitchingPostSouthDeed : BaseAddonDeed
    {
        private int m_ShrinkCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges
        {
            get { return m_ShrinkCharges; }
            set
            {
                m_ShrinkCharges = value;

                InvalidateProperties();
            }
        }

        public override BaseAddon Addon => new NewHitchingPostSouthAddon(m_ShrinkCharges);

        [Constructable]
        public NewHitchingPostSouthDeed()
            : this(ShrinkConfig.ShrinkCharges)
        { }

        [Constructable]
        public NewHitchingPostSouthDeed(int charges)
        {
            m_ShrinkCharges = charges;

            Name = "Hitching Post (South)";
        }

        public NewHitchingPostSouthDeed(Serial serial)
            : base(serial)
        { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_ShrinkCharges >= 0)
                list.Add(1060658, "Charges\t{0}", m_ShrinkCharges.ToString());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(m_ShrinkCharges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var v = reader.ReadInt();

            m_ShrinkCharges = reader.ReadInt();

            //if (v > 0 && Insensitive.Contains(Tooltip1, "Charges"))
            //    Tooltip1 = null;
        }
    }

    #endregion
}
