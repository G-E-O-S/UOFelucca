using System;
using Server.Spells;
using System.Collections.Generic;
using System.Linq;

using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAreaDamage : XmlAttachment
    {
        private int chanceToTrigger = 0;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ChanceToTrigger
        {
            get { return chanceToTrigger; }
            set { chanceToTrigger = value; }
        }

        // a serial constructor is REQUIRED
        public XmlAreaDamage(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlAreaDamage(int chance)
        {
            this.chanceToTrigger = chance;
        }

        public static IEnumerable<IDamageable> AcquireIndirectTargets(Mobile m, IPoint3D pnt, int range)
        {
            return AcquireIndirectTargets(m, pnt, m.Map, 5, true);
        }

        public static IEnumerable<IDamageable> AcquireIndirectTargets(Mobile caster, IPoint3D p, Map map, int range, bool losCheck)
        {
            if (map == null)
            {
                yield break;
            }

            IPooledEnumerable eable = map.GetObjectsInRange(new Point3D(p), range);

            foreach (var id in eable.OfType<IDamageable>())
            {
                if (id == caster)
                {
                    continue;
                }

                if (!id.Alive || (losCheck && !caster.InLOS(id)) || !caster.CanBeHarmful(id, false))
                {
                    continue;
                }

                if (id is Mobile && !SpellHelper.ValidIndirectTarget(caster, (Mobile)id))
                {
                    continue;
                }
                if (id is PlayerMobile ||(id is BaseCreature &&((BaseCreature)id).Controlled))
                {
                    continue; // Don't Target
                }

                yield return id;
            }

            eable.Free();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            // version 0
            writer.Write((int)chanceToTrigger);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        chanceToTrigger = reader.ReadInt();
                        break;
                    }
            }
        }
    }
}
