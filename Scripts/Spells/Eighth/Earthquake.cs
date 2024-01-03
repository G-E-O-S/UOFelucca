using System;
using System.Collections.Generic;

namespace Server.Spells.Eighth
{
    public class EarthquakeSpell : MagerySpell
    {
        public override DamageType SpellDamageType { get { return DamageType.SpellAOE; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
            "Earthquake", "In Vas Por",
            233,
            9012,
            false,
            Reagent.Bloodmoss,
            Reagent.Ginseng,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh);

        public EarthquakeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Eighth;
            }
        }
        public override bool DelayedDamage
        {
            get
            {
                return !Core.AOS;
            }
        }
        public override void OnCast()
        {
            if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                int radius = 10;

                foreach (Mobile m in Caster.GetMobilesInRange(radius))
                {
                    if (m != null && m != Caster && Caster.CanBeHarmful(m))
                    {
                        int damage;

                        if (Core.AOS)
                        {
                            damage = m.Hits / 2;

                            if (!m.Player)
                                damage = Math.Max(Math.Min(damage, 100), 15);
                            damage += Utility.RandomMinMax(0, 15);
                        }
                        else
                        {
                            damage = (m.Hits * 6) / 10;

                            if (!m.Player && damage < 10)
                                damage = 10;
                            else if (damage > 50)
                                damage = 50;
                        }

                        Caster.DoHarmful(m);
                        SpellHelper.Damage(this, m, damage, 100, 0, 0, 0, 0);
                    }
                }
            }

            FinishSequence();
        }

    }
}

