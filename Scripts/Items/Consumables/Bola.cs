using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
    public class Bola : Item
    {
        [Constructable]
        public Bola()
            : this(1)
        {
        }

        [Constructable]
        public Bola(int amount)
            : base(0x26AC)
        {
            Weight = 2.0;
            Stackable = true;
            Amount = amount;
        }

        public Bola(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1040019, from.NetState); // The bola must be in your pack to use it.
            }
            else if (!from.CanBeginAction(typeof(Bola)))
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1049624, from.NetState); // // You have to wait a few moments before you can use another bola!
            }
            else if (from.Target is BolaTarget)
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1049631, from.NetState); // This bola is already being used.
            }
            else if (from.Mounted)
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1042053, from.NetState); // You can't use this while on a mount!
            }
            else if (from.Flying)
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1113414, from.NetState); // You can't use this while flying!
            }
            else if (AnimalForm.UnderTransformation(from))
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, 1070902, from.NetState); // You can't use this while in an animal form!
            }
            else if (from.Skills.Tactics.Value < 100)
            {
                this.PrivateOverheadMessage(MessageType.Regular, 946, false, "You require higher skill in Tactics", from.NetState);
            }
            else
            {
                EtherealMount.StopMounting(from);

                if (Core.AOS)
                {
                    Item one = from.FindItemOnLayer(Layer.OneHanded);
                    Item two = from.FindItemOnLayer(Layer.TwoHanded);

                    if (one != null)
                        from.AddToBackpack(one);

                    if (two != null)
                        from.AddToBackpack(two);
                }

                from.Target = new BolaTarget(this);
                from.LocalOverheadMessage(MessageType.Emote, 201, 1049632); // * You begin to swing the bola...*
                from.NonlocalOverheadMessage(MessageType.Emote, 201, 1049633, from.Name); // ~1_NAME~ begins to menacingly swing a bola...
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private static void ReleaseBolaLock(object state)
        {
            ((Mobile)state).EndAction(typeof(Bola));
        }

        private static void FinishThrow(object state)
        {
            object[] states = (object[])state;

            Mobile from = (Mobile)states[0];
            Mobile to = (Mobile)states[1];
            Item bola = (Item)states[2];

            if (!from.Alive)
            {
                return;
            }
            if (!bola.IsChildOf(from.Backpack))
            {
                bola.PrivateOverheadMessage(MessageType.Regular, 946, 1040019, from.NetState); // The bola must be in your pack to use it.
            }
            else if (!from.InRange(to, 15) || !from.InLOS(to) || !from.CanSee(to))
            {
                from.PrivateOverheadMessage(MessageType.Regular, 946, 1042060, from.NetState); // You cannot see that target!
            }
            else if (!to.Mounted && !to.Flying && (!Core.ML || !AnimalForm.UnderTransformation(to)))
            {
                to.PrivateOverheadMessage(MessageType.Regular, 946, 1049628, from.NetState); // You have no reason to throw a bola at that.
            }
            else
            {
                bola.Consume();

                from.Direction = from.GetDirectionTo(to);
                from.Animate(AnimationType.Attack, 4);
                from.MovingEffect(to, 0x26AC, 10, 0, false, false);

                // new Bola().MoveToWorld(to.Location, to.Map);

                if (to is Neira || to is ChaosDragoon || to is ChaosDragoonElite)
                {
                    to.PrivateOverheadMessage(MessageType.Regular, 946, 1042047, from.NetState); // You fail to knock the rider from its mount.
                }
                else
                {
                    if (CheckHit(to, from))
                    {
                        // to.Damage(Utility.RandomMinMax(0, 3), from);

                        if (from.Flying)
                            to.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1113590, from.Name); // You have been grounded by ~1_NAME~!
                        else
                            to.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1049623, from.Name); // You have been knocked off of your mount by ~1_NAME~!

                        BaseMount.Dismount(to);

                        BaseMount.SetMountPrevention(to, BlockMountType.Dazed, TimeSpan.FromSeconds(10.0));
                    }
                }
            }
        }

        private static bool CheckHit(Mobile to, Mobile from)
        {
            if (!Core.LBR)
                return true;

            double tacticsLevel = from.Skills.Tactics.Value;

            double toChance = 100;

            double fromChance = Math.Max(10, tacticsLevel);

            double hitChance = fromChance / toChance;

            //Console.WriteLine("toChance: " + toChance + " fromChance: " + fromChance + " hitChance: " + hitChance);

            if (Utility.RandomDouble() < hitChance)
            {
                if (BaseWeapon.CheckParry(to))
                {
                    to.FixedEffect(0x37B9, 10, 16);
                    to.Animate(AnimationType.Parry, 0);
                    return false;
                }

                return true;
            }

            to.NonlocalOverheadMessage(MessageType.Emote, 0x3B2, false, "*miss*");
            return false;
        }

        private class BolaTarget : Target
        {
            private readonly Bola m_Bola;
            public BolaTarget(Bola bola)
                : base(20, false, TargetFlags.Harmful)
            {
                m_Bola = bola;
            }

            protected override void OnTarget(Mobile from, object obj)
            {
                if (m_Bola.Deleted)
                    return;

                if ((obj is Item))
                {
                    ((Item)obj).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1049628, from.NetState); // You have no reason to throw a bola at that.
                    return;
                }

                if (obj is Mobile)
                {
                    Mobile to = (Mobile)obj;

                    if (!m_Bola.IsChildOf(from.Backpack))
                    {
                        m_Bola.PrivateOverheadMessage(MessageType.Regular, 946, 1040019, from.NetState); // The bola must be in your pack to use it.
                    }
                    else if (from.Mounted)
                    {
                        m_Bola.PrivateOverheadMessage(MessageType.Regular, 946, 1042053, from.NetState); // You can't use this while on a mount!
                    }
                    else if (from.Flying)
                    {
                        m_Bola.PrivateOverheadMessage(MessageType.Regular, 946, 1113414, from.NetState); // You can't use this while flying!
                    }
                    else if (from == to)
                    {
                        from.SendLocalizedMessage(1005576); // You can't throw this at yourself.
                    }
                    else if (AnimalForm.UnderTransformation(from))
                    {
                        from.PrivateOverheadMessage(MessageType.Regular, 946, 1070902, from.NetState); // You can't use this while in an animal form!
                    }
                    else if (!to.Mounted && !to.Flying && (!Core.ML || !AnimalForm.UnderTransformation(to)))
                    {
                        to.PrivateOverheadMessage(MessageType.Regular, 946, 1049628, from.NetState); // You have no reason to throw a bola at that.
                    }
                    else if (!from.CanBeHarmful(to))
                    {
                    }
                    else if (from.BeginAction(typeof(Bola)))
                    {
                        from.RevealingAction();

                        EtherealMount.StopMounting(from);

                        Item one = from.FindItemOnLayer(Layer.OneHanded);
                        Item two = from.FindItemOnLayer(Layer.TwoHanded);

                        if (one != null)
                            from.AddToBackpack(one);

                        if (two != null)
                            from.AddToBackpack(two);

                        from.DoHarmful(to);

                        BaseMount.SetMountPrevention(from, BlockMountType.BolaRecovery, TimeSpan.FromSeconds(10.0));
                        Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(ReleaseBolaLock), from); // default 10.0
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(FinishThrow), new object[] { from, to, m_Bola });
                    }
                    else
                    {
                        m_Bola.PrivateOverheadMessage(MessageType.Regular, 946, 1049624, from.NetState); // You have to wait a few moments before you can use another bola!
                    }
                }
            }
        }
    }
}
