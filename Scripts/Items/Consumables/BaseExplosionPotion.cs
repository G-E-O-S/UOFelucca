#region References
using System;
using System.Linq;
using System.Collections;

using Server.Network;
using Server.Spells;
using Server.Targeting;
#endregion

namespace Server.Items
{
    public abstract class BaseExplosionPotion : BasePotion
    {
        private const int ExplosionRange = 2; // How long is the blast radius?
        private Timer m_Timer;

        public BaseExplosionPotion(PotionEffect effect)
            : base(0xF0D, effect)
        { }

        public BaseExplosionPotion(Serial serial)
            : base(serial)
        { }


        public abstract int MinDamage { get; }
        public abstract int MaxDamage { get; }
        public override bool RequireFreeHand { get { return false; } }

        public static bool HasExploded = false;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public virtual object FindParent(Mobile from)
        {
            Mobile m = HeldBy;

            if (m != null && m.Holding == this)
            {
                return m;
            }

            object obj = RootParent;

            if (obj != null)
            {
                return obj;
            }

            if (Map == Map.Internal)
            {
                return from;
            }

            return this;
        }

        #region Delay
        private static readonly Hashtable m_Delay = new Hashtable();

        public static void AddDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null)
                timer.Stop();

            m_Delay[m] = Timer.DelayCall(TimeSpan.FromSeconds(7), new TimerStateCallback(EndDelay_Callback), m);
        }

        public static int GetDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null && timer.Next > DateTime.UtcNow)
                return (int)(timer.Next - DateTime.UtcNow).TotalSeconds;

            return 0;
        }

        private static void EndDelay_Callback(object obj)
        {
            if (obj is Mobile)
                EndDelay((Mobile)obj);
        }

        public static void EndDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null)
            {
                timer.Stop();
                m_Delay.Remove(m);
            }
        }

        #endregion

        public override void Drink(Mobile from)
        {
            if (Core.AOS && (from.Paralyzed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)))
            {
                from.SendLocalizedMessage(1062725); // You can not use a purple potion while paralyzed.
                return;
            }

            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1062334);
                return;
            }

            int delay = GetDelay(from);

            if (delay > 0)
            {
                from.SendLocalizedMessage(1072529, String.Format("{0}\t{1}", delay, delay > 1 ? "seconds." : "second.")); // You cannot use that for another ~1_NUM~ ~2_TIMEUNITS~
                return;
            }

            ThrowTarget targ = from.Target as ThrowTarget;
            Stackable = false; // Scavenged explosion potions won't stack with those ones in backpack, and still will explode.

            if (targ != null && targ.Potion == this)
            {
                return;
            }

            from.RevealingAction();
            from.Target = new ThrowTarget(this);

            if (m_Timer == null)
            {
                from.SendLocalizedMessage(500236); // You should throw it now!

                if (Core.LBR)
                {
                    m_Timer = Timer.DelayCall(
                        TimeSpan.FromSeconds(0.0),
                        TimeSpan.FromSeconds(1.0),
                        5,
                        new TimerStateCallback(Detonate_OnTick),
                        new object[] { from, 4 }); // 3.6 seconds explosion delay
                }
            }
            BaseExplosionPotion.AddDelay(from);
        }

        public void Explode(Mobile from, bool direct, Point3D loc, Map map)
        {
            /*if (Deleted)
            {
                return;
            }*/

            bool damageThrower = false;

            /*if (from != null)
            {
                if (from.Target is ThrowTarget && ((ThrowTarget)from.Target).Potion == this) //Double-Click pot - let time run out.
                {
                    damageThrower = true;
                    Target.Cancel(from); //removing the target queue.
                    from.Target = null;
                    Consume();
                }

                if (IsChildOf(from.Backpack) || Parent == from) //Escape while throwing the pot.
                {
                    Target.Cancel(from); //removing the target queue.
                }

                if (from.Target == (ThrowTarget)from.Target) //Throws the pot on the floor
                {
                    damageThrower = true;
                }
            }
            Consume();*/

            var list = SpellHelper.AcquireIndirectTargets(from, loc, map, ExplosionRange, false).OfType<Mobile>().ToList();

            if (from != null && from.Target is ThrowTarget throwTarget && throwTarget.Potion == this)
            {
                damageThrower = true;
                Target.Cancel(from); //removing the target queue.
                from.Target = null;
                Consume();
            }

            if (from.Target is ThrowTarget)
            {
                damageThrower = true;
                Consume(); // Move Consume() inside the condition
            }

            if (from != null && damageThrower && !(list.Contains(from) && from.InRange(loc, ExplosionRange)))
            {
                list.Add(from);
                Consume(); // Move Consume() inside the condition
            }

            if (map == null)
            {
                return;
            }

            Effects.PlaySound(loc, map, 0x307);

            Effects.SendLocationEffect(loc, map, 0x36B0, 9, 10, 0, 0);
            int alchemyBonus = 0;

            if (direct)
            {
                alchemyBonus = (int)(from.Skills.Alchemy.Value / 7);
            }

            int min = Scale(from, MinDamage);
            int max = Scale(from, MaxDamage);


            if (from != null && /*damageThrower &&*/ !list.Contains(from) && from.InRange(loc, ExplosionRange))
            {
                list.Add(from);
            }

            foreach (var m in list)
            {
                if (from != null)
                {
                    from.DoHarmful(m);
                }

                int damage = Utility.RandomMinMax(min, max);

                damage += alchemyBonus;

                if (!Core.AOS && damage > 40)
                {
                    damage = Utility.RandomMinMax(26, 30);
                }
                else if (Core.AOS && list.Count > 2)
                {
                    damage /= list.Count - 1;
                }

                AOS.Damage(m, from, damage, 0, 100, 0, 0, 0, Server.DamageType.SpellAOE);
            }

            list.Clear();
            Consume();
        }


        private void Detonate_OnTick(object state)
        {
            if (Deleted)
            {
                return;
            }

            var states = (object[])state;
            Mobile from = (Mobile)states[0];
            int timer = (int)states[1];

            object parent = FindParent(from);

            if (timer == 0)
            {
                HasExploded = true;
                Point3D loc;
                Map map;

                if (parent is Item)
                {
                    Item item = (Item)parent;

                    loc = item.GetWorldLocation();
                    map = item.Map;
                }
                else if (parent is Mobile)
                {
                    Mobile m = (Mobile)parent;

                    loc = m.Location;
                    map = m.Map;
                }
                else
                {
                    return;
                }
                Explode(from, true, loc, map);
                m_Timer = null;
            }
            else
            {
                HasExploded = false;
                if (parent is Item)
                {
                    ((Item)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
                }
                else if (parent is Mobile)
                {
                    ((Mobile)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
                }

                states[1] = timer - 1;
            }
        }

        private void Reposition_OnTick(object state)
        {
            if (Deleted)
            {
                return;
            }

            var states = (object[])state;
            Mobile from = (Mobile)states[0];
            IPoint3D p = (IPoint3D)states[1];
            Map map = (Map)states[2];

            Point3D loc = new Point3D(p);
            MoveToWorld(loc, map);
        }

        private class ThrowTarget : Target
        {
            private readonly BaseExplosionPotion m_Potion;

            public ThrowTarget(BaseExplosionPotion potion)
                : base(12, true, TargetFlags.None)
            {
                m_Potion = potion;
            }

            /*protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                base.OnTargetCancel(from, cancelType);
                if (Potion != null && Potion.m_Timer != null)
                {
                    if (Potion.m_Timer != null && BaseExplosionPotion.HasExploded == false)
                    {
                        Potion.Consume();
                        Potion.m_Timer.Stop();
                        Potion.m_Timer = null;
                        from.SendLocalizedMessage(1042021);
                        BaseExplodingTarPotion.AddDelay(from);
                    }
                    else
                    {
                        Potion.m_Timer.Stop();
                        Potion.m_Timer = null;
                        BaseExplosionPotion.AddDelay(from);
                    }

                }
            }*/

            /*protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                if (Potion != null && Potion.m_Timer != null)
                {
                    Potion.Consume();
                    Potion.m_Timer.Stop();
                    Potion.m_Timer = null;
                    from.SendLocalizedMessage(500446); // That is too far away.
                    BaseExplosionPotion.AddDelay(from);
                }
            }*/

            /*protected override void OnTargetOutOfLOS(Mobile from, object Targeted)
            {
                base.OnTargetOutOfLOS(from, Targeted);
                if (Potion != null)
                {

                    //Potion.Consume(); //Should pots be consumed when thrown out of line of sight?
                    //Potion.m_Timer.Stop();
                    //Potion.m_Timer = null;
                    //BaseExplosionPotion.AddDelay(from);
                }
            }*/

            public BaseExplosionPotion Potion { get { return m_Potion; } }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Potion.Deleted || m_Potion.Map == Map.Internal)
                {
                    return;
                }

                IPoint3D p = targeted as IPoint3D;

                if (p == null)
                {
                    return;
                }

                // Add delay
                /*if (from.AccessLevel >= AccessLevel.Player)
                {
                    BaseExplosionPotion.AddDelay(from);
                }*/

                Map map = from.Map;

                if (map == null)
                {
                    return;
                }

                SpellHelper.GetSurfaceTop(ref p);

                from.RevealingAction();

                IEntity to;

                to = new Entity(Serial.Zero, new Point3D(p), map);

                if (p is Mobile)
                {
                    to = (Mobile)p;
                }

                Effects.SendMovingEffect(from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0);

                if (m_Potion.Amount > 1)
                {
                    Mobile.LiftItemDupe(m_Potion, 1);
                }

                m_Potion.Internalize();
                Timer.DelayCall(
                    TimeSpan.FromSeconds(0.0), new TimerStateCallback(m_Potion.Reposition_OnTick), new object[] { from, p, map });
            }
        }
    }

}
