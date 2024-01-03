using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace YourServerNamespace
{
    public class GoldSafe : Item
    {
        public override string DefaultName { get { return "Exceptional Safe"; } }

        private DateTime m_LastGenerated;
        private int m_CurrentGold;

        [Constructable]
        public GoldSafe() : base(0x9C19)
        {
            Hue = 1161;
            m_CurrentGold = 0;

            StartTimer();
        }

        public GoldSafe(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            //if (!IsChildOf(from.Backpack))
            //{
            //    from.SendMessage("The gold generator must be in your backpack to collect gold.");
            //    return;
            //}

            if (m_CurrentGold <= 0)
            {
                from.SendMessage("There is no gold to collect.");
                return;
            }

            from.SendMessage($"You collect {m_CurrentGold} gold from the gold generator.");

            from.AddToBackpack(new BankCheck(m_CurrentGold));
            m_CurrentGold = 0;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version

            writer.Write(m_LastGenerated);
            writer.Write(m_CurrentGold);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_LastGenerated = reader.ReadDateTime();
            m_CurrentGold = reader.ReadInt();

            StartTimer();
        }

        private void StartTimer()
        {
            TimeSpan timeSinceLastGenerated = DateTime.UtcNow - m_LastGenerated;
            TimeSpan timeToNextGeneration = TimeSpan.FromHours(2) - timeSinceLastGenerated;

            if (timeToNextGeneration <= TimeSpan.Zero)
                GenerateGold();
            else
                Timer.DelayCall(timeToNextGeneration, GenerateGold);
        }

        private void GenerateGold()
        {
            m_CurrentGold += 12000;
            m_LastGenerated = DateTime.UtcNow;

            if (m_CurrentGold >= 2500000)
                StopTimer();
            else
                StartTimer();
        }

        private void StopTimer()
        {
            // Optionally, you can add code to handle stopping the gold generation permanently.
            // This could involve removing the item from the player's backpack or disabling further generation.
        }
    }
}
