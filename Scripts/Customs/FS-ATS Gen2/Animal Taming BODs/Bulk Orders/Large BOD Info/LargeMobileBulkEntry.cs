using System;
using System.Collections;

namespace Server.Engines.BulkOrders
{
    public class LargeMobileBulkEntry
    {
        private LargeMobileBOD m_Owner;
        private int m_Amount;
        private readonly SmallMobileBulkEntry m_Details;

        public LargeMobileBOD Owner { get => m_Owner; set => m_Owner = value; }
        public int Amount { get => m_Amount; set { m_Amount = value; if (m_Owner != null) m_Owner.InvalidateProperties(); } }
        public SmallMobileBulkEntry Details => m_Details;

        public static SmallMobileBulkEntry[] Mounts => GetEntries("Taming", "mounts");

        public static SmallMobileBulkEntry[] HardMounts => GetEntries("Taming", "hardmounts");

        public static SmallMobileBulkEntry[] Dragons => GetEntries("Taming", "dragons");

        public static SmallMobileBulkEntry[] FarmAnimals => GetEntries("Taming", "farmanimals");

        public static SmallMobileBulkEntry[] Spiders => GetEntries("Taming", "spiders");

        public static SmallMobileBulkEntry[] Canines => GetEntries("Taming", "canines");

        public static SmallMobileBulkEntry[] Felines => GetEntries("Taming", "felines");

        public static SmallMobileBulkEntry[] Bears => GetEntries("Taming", "bears");

        public static SmallMobileBulkEntry[] Birds => GetEntries("Taming", "birds");

        public static SmallMobileBulkEntry[] Rodents => GetEntries("Taming", "rodents");

        public static SmallMobileBulkEntry[] Beetles => GetEntries("Taming", "beetles");

        public static SmallMobileBulkEntry[] Hiryus => GetEntries("Taming", "hiryus");

        private static Hashtable m_Cache;

        public static SmallMobileBulkEntry[] GetEntries(string type, string name)
        {
            if (m_Cache == null)
                m_Cache = new Hashtable();

            Hashtable table = (Hashtable)m_Cache[type];

            if (table == null)
                m_Cache[type] = table = new Hashtable();

            SmallMobileBulkEntry[] entries = (SmallMobileBulkEntry[])table[name];

            if (entries == null)
                table[name] = entries = SmallMobileBulkEntry.LoadEntries(type, name);

            return entries;
        }

        public static LargeMobileBulkEntry[] ConvertEntries(LargeMobileBOD owner, SmallMobileBulkEntry[] small)
        {
            LargeMobileBulkEntry[] large = new LargeMobileBulkEntry[small.Length];

            for (int i = 0; i < small.Length; ++i)
                large[i] = new LargeMobileBulkEntry(owner, small[i]);

            return large;
        }

        public LargeMobileBulkEntry(LargeMobileBOD owner, SmallMobileBulkEntry details)
        {
            m_Owner = owner;
            m_Details = details;
        }

        public LargeMobileBulkEntry(LargeMobileBOD owner, GenericReader reader)
        {
            m_Owner = owner;
            m_Amount = reader.ReadInt();

            Type realType = null;

            string type = reader.ReadString();

            if (type != null)
                realType = ScriptCompiler.FindTypeByFullName(type);

            m_Details = new SmallMobileBulkEntry(realType, reader.ReadString(), reader.ReadInt());
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(m_Amount);
            writer.Write(m_Details.Type == null ? null : m_Details.Type.FullName);
            writer.Write(m_Details.AnimalName);
            writer.Write(m_Details.Graphic);
        }
    }
}