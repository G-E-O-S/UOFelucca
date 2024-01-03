
using System;
using System.Collections.Generic;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x9A8, 0xE80)]
    public class MetalBossBox : LockableContainer
    {

        private Type m_ItemToGive;
        private Type m_MobileToSpawn;
        private int m_AmountNeeded;
        private Point3D m_SpawnLocation;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemToGive
        {
            get { return m_ItemToGive; }
            set { m_ItemToGive = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Type MobileToSpawn
        {
            get { return m_MobileToSpawn; }
            set { m_MobileToSpawn = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded
        {
            get { return m_AmountNeeded; }
            set { m_AmountNeeded = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SpawnLocation
        {
            get { return m_SpawnLocation; }
            set { m_SpawnLocation = value; }
        }
        [Constructable]
        public MetalBossBox()
            : base(0x9A8)
        {
            Hue = Utility.RandomBrightHue();
            Name = "Boss Drop Box";
            Movable = false;
            m_AmountNeeded = 1;
        }

        public MetalBossBox(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight => 5;

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel != AccessLevel.Player)
                base.OnDoubleClick(from);
            else
                from.SendMessage("You can not open the boss chest!!");
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            if (m_ItemToGive != null && m_MobileToSpawn != null)
            {
                if (dropped.GetType() == m_ItemToGive)
                {
                    if (dropped.Amount >= m_AmountNeeded)
                    {
                        Mobile m = Activator.CreateInstance(m_MobileToSpawn) as Mobile;

                        m.Map = this.Map;

                        if (m_SpawnLocation == Point3D.Zero)
                        {

                            m.Location = this.Location;
                        }
                        else
                        {
                            m.Location = m_SpawnLocation;
                        }

                        from.SendMessage("You have awakened the boss!!!!");

                        Region region = m.Region;

                        List<Mobile> players = region.GetPlayers();

                        foreach (Mobile mob in players)
                        {
                            mob.PublicOverheadMessage(Network.MessageType.System, 1161, false, String.Format("{0} has been been summoned!!!", m.Name));
                        }

                        if (dropped.Amount > m_AmountNeeded)
                        {
                            dropped.Amount -= m_AmountNeeded;

                            from.SendMessage(string.Format("{0} {1} returned to you.", dropped.Amount, dropped.Name));
                            from.AddToBackpack(dropped);
                        }
                        else
                            dropped.Delete();

                        return true;
                    }
                    else
                    {
                        from.SendMessage(String.Format("You do not have enough. You need {0}", m_AmountNeeded));
                        return false;
                    }
                }
                else
                {
                    from.SendMessage("This is not the correct item to activate the boss chest.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("This boss chest is not setup yet.");
                return false;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
            writer.Write(this.m_ItemToGive == null ? null : this.m_ItemToGive.FullName);
            writer.Write(this.m_MobileToSpawn == null ? null : this.m_MobileToSpawn.FullName);
            writer.Write(this.m_AmountNeeded);
            writer.Write(this.m_SpawnLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_ItemToGive = ScriptCompiler.FindTypeByFullName(reader.ReadString());
            m_MobileToSpawn = ScriptCompiler.FindTypeByFullName(reader.ReadString());
            m_AmountNeeded = reader.ReadInt();
            m_SpawnLocation = reader.ReadPoint3D();
        }
    }
}
