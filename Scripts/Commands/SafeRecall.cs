// Recall Command v1.4.1
// Author: Felladrin
// Started: 2010-06-20
// Updated: 2016-02-08

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Spells;

namespace Felladrin.Commands
{
    public static class Recall
    {
        private static readonly Dictionary<Mobile, DateTime> Cooldowns = new Dictionary<Mobile, DateTime>();
        public static class Config
        {
            public static bool Enabled = true;                                       // Is this command enabled?
            public static bool AllowUsageIfIsOverloaded = false;                      // Should we allow players to use this command if they are overloaded?
            public static bool AllowUsageIfIsInCombat = false;                        // Should we allow players to use this command if they are in combat?
            public static bool DecreaseFameIfUsedInCombat = true;                    // Should we decrease the player fame if he use this command to flee from combat?
            public static bool SpecialEffects = true;                                // Should we use special effects after teleporting the player?
            public static bool BringFollowers = true;                                // Should we also teleport the player's followers?
            public static bool AffectOnlyControlledFollowers = true;                 // Should we only teleport the player's followers that are controlled?
            public static Map TargetMap = Map.Felucca;                               // To what map should we teleport the player?
            public static Point3D TargetLocation = new Point3D(1434, 1702, 14);       // To what coordinates should we teleport the player?
            public static TimeSpan CombatHeatDelay = TimeSpan.FromSeconds(30.0);     // What's the delay for they to be considered out of combat?

            public static TimeSpan RecallCooldown = TimeSpan.FromMinutes(30.0); // Adjust the cooldown duration as needed

        }

        public static void Initialize()
        {
            if (Config.Enabled)
                CommandSystem.Register("SafeRecall", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("Recall")]
        [Description("Teleports you to a safe location.")]
        static void OnCommand(CommandEventArgs e)
        {
            var m = e.Mobile;

            if (Cooldowns.ContainsKey(m) && DateTime.UtcNow < Cooldowns[m])
            {
                var remainingCooldown = Cooldowns[m] - DateTime.UtcNow;
                m.SendMessage($"You must wait {remainingCooldown.Minutes} minutes and {remainingCooldown.Seconds} seconds before using recall again.");
                return;
            }

            if (!Config.AllowUsageIfIsOverloaded && WeightOverloading.IsOverloaded(m))
            {
                m.SendMessage("You can't recall because you are carrying too much weight!");
                return;
            }

            if (!Config.AllowUsageIfIsInCombat && IsInCombat(m))
            {
                m.SendMessage("You can't recall during the heat of battle!");
                return;
            }

            m.Emote("Using safe recall!");
            SendSpecialEffects(m);

            Timer.DelayCall(TimeSpan.FromSeconds(2.5), () =>
            {                
                m.MoveToWorld(Config.TargetLocation, Config.TargetMap);
                SendSpecialEffects(m);

                if (Config.DecreaseFameIfUsedInCombat && SpellHelper.CheckCombat(m))
                    Titles.AwardFame(m, -(m.Fame / 20), true);

                if (Config.BringFollowers)
                {
                    var master = m as PlayerMobile;
                    var followers = master.AllFollowers;

                    if (followers.Count > 0)
                    {
                        foreach (var follower in followers)
                        {
                            var mount = follower as IMount;

                            if (mount != null)
                            {
                                if (mount.Rider == master)
                                    continue;

                                mount.Rider = null;
                            }

                            if (Config.AffectOnlyControlledFollowers)
                            {
                                var baseCreature = follower as BaseCreature;

                                if (baseCreature != null && !baseCreature.Controlled)
                                    continue;
                            }

                            follower.MoveToWorld(master.Location, master.Map);
                        }
                    }
                }
                m.Emote("Using safe recall!");
                Cooldowns[m] = DateTime.UtcNow.Add(Config.RecallCooldown);
            });
        }


        static void SendSpecialEffects(Mobile m)
        {
                 
            if (!Config.SpecialEffects)
                return;            
            m.PlaySound(0x1FC);
            
            Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3745, 16);
            Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 6), m.Map, 0x374F, 16);
        }

        static bool IsInCombat(Mobile m)
        {
            foreach (var info in m.Aggressed)
                if ((DateTime.UtcNow - info.LastCombatTime) < Config.CombatHeatDelay)
                    return true;

            foreach (var info in m.Aggressors)
                if ((DateTime.UtcNow - info.LastCombatTime) < Config.CombatHeatDelay)
                    return true;

            return false;
        }
    }
}
