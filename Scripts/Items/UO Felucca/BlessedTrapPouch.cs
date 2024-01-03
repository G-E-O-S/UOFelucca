using System;
using System.Collections.Generic;
using System.Collections;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public class BlessedTrapPouch : TrapableContainer
    {
        private const int MAX_CHARGES = 25;
        private int m_Charges;

        [Constructable]
        public BlessedTrapPouch()
            : base(0xE79)
        {
            Weight = 1.0;
            Hue = 1174;
            Name = "Blessed Trap Pouch";
            LootType = LootType.Blessed;

            TrapType = TrapType.ExplosionTrap;
            TrapPower = 1;
            TrapLevel = 0;

            m_Charges = 9;
        }

        public BlessedTrapPouch(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get
            {
                return this.m_Charges;
            }
            set
            {
                this.m_Charges = value;
                this.InvalidateProperties();
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Charges > 0)
            {
                base.OnDoubleClick(from);

                TrapType = TrapType.ExplosionTrap;
                TrapPower = 1;
                TrapLevel = 0;

                m_Charges -= 1;
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (this.IsChildOf(from.Backpack)
                && dropped is RewardScroll) // RewardScroll
            {
                if (m_Charges >= MAX_CHARGES)
                {
                    return false;
                }

                if (m_Charges + dropped.Amount <= MAX_CHARGES)
                {
                    m_Charges += dropped.Amount;
                    dropped.Delete();

                    return true;
                }

                dropped.Amount -= (MAX_CHARGES - m_Charges);
                m_Charges = MAX_CHARGES;

                return false;
            }

            return false;
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            ArrayList attrs = new ArrayList();
            attrs.Add(new EquipInfoAttribute(1011296, m_Charges));

            EquipmentInfo eqInfo = new EquipmentInfo(1041000, null, false, (EquipInfoAttribute[])attrs.ToArray(typeof(EquipInfoAttribute)));

            from.Send(new DisplayEquipmentInfo(this, eqInfo));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Charges = reader.ReadInt();
        }
    }
}
