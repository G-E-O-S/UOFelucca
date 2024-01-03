using System;
using System.Collections;
using System.IO;

namespace Server.Engines.BulkOrders
{
    public class SmallMobileBulkEntry
    {
        private readonly Type m_Type;
        private readonly string m_AnimalName;
        private readonly int m_Graphic;

        public Type Type => m_Type;
        public string AnimalName => m_AnimalName;
        public int Graphic => m_Graphic;

        public SmallMobileBulkEntry(Type type, string animalname, int graphic)
        {
            m_Type = type;
            m_AnimalName = animalname;
            m_Graphic = graphic;
        }

        public static SmallMobileBulkEntry[] TamingMounts => GetEntries("Taming", "animals");

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
                table[name] = entries = LoadEntries(type, name);

            return entries;
        }

        public static SmallMobileBulkEntry[] LoadEntries(string type, string name)
        {
            return LoadEntries(string.Format("Data/Bulk Orders/{0}/{1}.cfg", type, name));
        }

        public static SmallMobileBulkEntry[] LoadEntries(string path)
        {
            path = Path.Combine(Core.BaseDirectory, path);

            ArrayList list = new ArrayList();

            if (File.Exists(path))
            {
                using (StreamReader ip = new StreamReader(path))
                {
                    string line;

                    while ((line = ip.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line.StartsWith("#"))
                            continue;

                        try
                        {
                            string[] split = line.Split('\t');

                            if (split.Length >= 2)
                            {
                                Type type = ScriptCompiler.FindTypeByName(split[0]);
                                int graphic = Utility.ToInt32(split[split.Length - 1]);

                                if (type != null && graphic > 0)
                                    list.Add(new SmallMobileBulkEntry(type, type.Name, graphic));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return (SmallMobileBulkEntry[])list.ToArray(typeof(SmallMobileBulkEntry));
        }
    }
}