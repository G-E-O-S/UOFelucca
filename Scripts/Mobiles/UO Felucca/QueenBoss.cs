using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Prompts;
using System.IO;
using Server.ContextMenus;
using Server.Targeting;
using Server.Engines.CannedEvil;
using Server.Spells.Fourth;
using Server.Spells.Fifth;

namespace Server.Mobiles
{	
	[CorpseName("Queen's corpse")]
	public class QueenBoss : BaseCreature
	{
		private const int MAX_HITS = 99999;
		private const int DAEMON_HUE = 1259;
		private const int GARGOYLE_HUE = 2658;
		private const int DRAGON_HUE = 1271;
		private const int ARCANEDEMON_HUE = 2759;
		private const int SPECIAL_PLAYER_DAMAGE = 10;
		private const int SPECIAL_PET_DAMAGE = 22;
		private const int SPECIAL_RANGE = 16;
		
		private const long SPECIAL_COOLDOWN = 10000;
		
		public enum QueenBossStage
		{
			Human,
			Daemon,			
			Gargoyle,
			Dragon,
			ArcaneDemon
		}
		
		private QueenBossStage m_QueenBossStage;
		private List<Mobile> m_Enemies;
		private int m_SpeechHue;
		private long m_TryNextTeleport;
		private long m_NextSpecialAbility;
		
		[Constructable]
		public QueenBoss()
			: base(AIType.AI_Melee, FightMode.Closest, 12, 12)
		{
			m_QueenBossStage = QueenBossStage.Human;
			m_Enemies = new List<Mobile>();
			
			Name = "Queen";
			NameHue = 16;
			Body = 0x190;
			Hue = 1175;
			VirtualArmor = 80;
			Kills = 100;
			m_SpeechHue = 43;
			
			SetSkill(SkillName.Wrestling, 100.0);
			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.MagicResist, 100.0);
			
			SetDamage(15, 30);
            
			SetStr(650);
			SetDex(250);
			SetInt(500);
			SetHits(MAX_HITS);
            SetStam(250);

            InitOutfit();
			
			m_TryNextTeleport = Core.TickCount;
			m_NextSpecialAbility = Core.TickCount;
		}
		
		public QueenBoss(Serial serial)
			: base(serial)
		{
			
		}
		
		// TODO Setup Boss Reward 
		// Reward will go to any player that attacked the boss.
		public void GiveReward()
		{
			foreach (Mobile m in m_Enemies)
			{
				Robe reward = new Robe();
				reward.Name = "UO: Felucca";
                reward.Hue = Utility.RandomList(2767, 2768, 2769, 2775, 2779, 2785, 2787, 2811, 2812, 2821, 2824);
				
				m.AddToBackpack(reward);
			}            
        }
		
		public override bool CanPeace { get { return false; } }
        public override bool CanProvoke { get { return false; } }
		public override bool CanDiscord { get { return false; } }
		
		private void InitOutfit()
		{
			HoodedShroudOfShadows shroud = new HoodedShroudOfShadows(1175);
			AddItem(shroud);
			shroud.Movable = false;
			
			Sandals sandals = new Sandals(1161);			
			AddItem(sandals);
			sandals.Movable = false;
			
			HalfApron halfApron = new HalfApron(1161);
			AddItem(halfApron);
			halfApron.Movable = false;            
        }
		
		// First Boss Stage
		private void SetDaemonState()
		{
			m_QueenBossStage = QueenBossStage.Daemon;
			Body = 102;
			Hue = DAEMON_HUE;
			Title = "the Infernal";
			m_SpeechHue = 38;
	
			Effects.SendTargetEffect(this, 0x37CC, 10, 30, DAEMON_HUE - 1, 2);
			Effects.PlaySound(this, Map, 0x591);
			
			DaemonStageSpecial();
			Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(CreateFireField)).Start();
		}
		
		// Second Boss Stage
		private void SetGargoyleState()
		{
			m_QueenBossStage = QueenBossStage.Gargoyle;
			Body = 755;
			Hue = GARGOYLE_HUE;
			Title = "the Frozen-hearted";
			m_SpeechHue = 88;
			
			Effects.SendTargetEffect(this, 0x37CC, 10, 30, GARGOYLE_HUE - 1, 2);
			Effects.PlaySound(this, Map, 0x2B8);
			
			GargoyleStageSpecial();
		}
		
		// Third Boss Stage
		private void SetDragonState()
		{
			m_QueenBossStage = QueenBossStage.Dragon;
			Body = 46;
			Hue = DRAGON_HUE;
			Title = "the Putrid";
			m_SpeechHue = 63;

			Effects.SendTargetEffect(this, 0x37CC, 10, 30, DRAGON_HUE - 1, 2);
			Effects.PlaySound(this, Map, 0x231);
			
			DragonStageSpecial();
			Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(CreatePoisonField)).Start();
		}
		
		// Forth Boss Stage
		private void SetArcaneDemonState()
		{
			m_QueenBossStage = QueenBossStage.ArcaneDemon;
			Body = 784;
			Hue = ARCANEDEMON_HUE;
			Title = "the Accursed";
			m_SpeechHue = 132;
			
			Effects.SendTargetEffect(this, 0x37CC, 10, 30, ARCANEDEMON_HUE - 1, 2);
			Effects.PlaySound(this, Map, 0x567);
			
			Say("Your souls belong to me!", m_SpeechHue, true);
			
			ArcaneDemonStageSpecial(this);
		}
		
		private void DaemonStageSpecial()
		{
			Say("RAIN FIRE!", m_SpeechHue, true);
			
			List<Mobile> targets = GetAreaTargets(SPECIAL_RANGE);
			
			foreach (Mobile m in targets)
			{
				if (m == null || m.Deleted)
				{
					continue;
				}
				
                if (m.Alive)
				{
					this.DoHarmful(m);
					
					Effects.SendMovingParticles(this, m, 0x36D4, 2, 750, false, true, DAEMON_HUE - 1, 2, 0, 0x36B0, 0x11D, 0);
					
					if (m is PlayerMobile)
					{
						AOS.Damage(m, this, SPECIAL_PLAYER_DAMAGE, 100, 0, 0, 0, 0);
					}
					else
					{
						AOS.Damage(m, this, SPECIAL_PET_DAMAGE, 100, 0, 0, 0, 0);					
					}

					if (m.Body.IsHuman && !m.Mounted)
					{
						m.Animate(20, 7, 1, true, false, 0); // take hit
					}
				}
			}
		}
		
		private void CreateFireField()
		{
			Say("BURN BURN BURN!", m_SpeechHue, true);
			int seconds = 10;			
			
			for(int x = this.Location.X - 10; x <= this.Location.X + 10; x++)
			{
				for(int y = this.Location.Y - 10; y <= this.Location.Y + 10; y++)
				{
					new FireFieldSpell.FireFieldItem(0x398C, new Point3D(x, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
				}
			}
			
			for(int x = this.Location.X - 9; x <= this.Location.X + 9; x++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(x, this.Location.Y + 11, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 9; x <= this.Location.X + 9; x++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(x, this.Location.Y - 11, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 9; y <= this.Location.Y + 9; y++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(this.Location.X + 11, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 9; y <= this.Location.Y + 9; y++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(this.Location.X - 11, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 7; x <= this.Location.X + 7; x++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(x, this.Location.Y + 12, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 7; x <= this.Location.X + 7; x++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(x, this.Location.Y - 12, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 7; y <= this.Location.Y + 7; y++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(this.Location.X + 12, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 7; y <= this.Location.Y + 7; y++)
			{
				new FireFieldSpell.FireFieldItem(0x398C, new Point3D(this.Location.X - 12, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
		}
		
		private void GargoyleStageSpecial()
		{
			Say("YOUR BLOOD WILL FREEZE!", m_SpeechHue, true);
			
			List<Mobile> targets = GetAreaTargets(SPECIAL_RANGE);
			
			foreach (Mobile m in targets)
			{
				if (m == null || m.Deleted)
				{
					continue;
				}
				
                if (m.Alive)
				{
					this.DoHarmful(m);
					
					Effects.SendMovingParticles(this, m, 0x1153, 2, 750, false, true, GARGOYLE_HUE - 1, 2, 0, 0x37C4, 0x46F, 0);
					
					if (m is PlayerMobile)
					{
						AOS.Damage(m, this, SPECIAL_PLAYER_DAMAGE, 100, 0, 0, 0, 0); // 50 damage to player
					}
					else
					{
						AOS.Damage(m, this, SPECIAL_PET_DAMAGE, 100, 0, 0, 0, 0); // 150 damage to pet						
					}
					
					m.Paralyze(TimeSpan.FromSeconds(3));
				}
			}
		}
		
		private void DragonStageSpecial()
		{
			Say("PESTILENCE WILL CLEANSE THIS LAND!", m_SpeechHue, true);
			
			List<Mobile> targets = GetAreaTargets(SPECIAL_RANGE);
			Poison p = Poison.Deadly;
			
			foreach (Mobile m in targets)
			{
				if (m == null || m.Deleted)
				{
					continue;
				}
				
                if (m.Alive)
				{
					this.DoHarmful(m);
					
					Effects.SendMovingParticles(this, m, 0x36E4, 2, 750, false, true, DRAGON_HUE - 1, 2, 0, 0x0923, 0x5CC, 0);
					
					m.ApplyPoison(this, p);

					if (m.Body.IsHuman && !m.Mounted)
					{
						m.Animate(20, 7, 1, true, false, 0); // take hit
					}
				}
			}
		}
		
		private void CreatePoisonField()
		{
			Say("DROWN IN POISON!", m_SpeechHue, true);
			int seconds = 10;
			
			for(int x = this.Location.X - 10; x <= this.Location.X + 10; x++)
			{
				for(int y = this.Location.Y - 10; y <= this.Location.Y + 10; y++)
				{
					new PoisonFieldSpell.InternalItem(0x3915, new Point3D(x, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
				}
			}
			
			for(int x = this.Location.X - 9; x <= this.Location.X + 9; x++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(x, this.Location.Y + 11, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 9; x <= this.Location.X + 9; x++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(x, this.Location.Y - 11, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 9; y <= this.Location.Y + 9; y++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(this.Location.X + 11, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 9; y <= this.Location.Y + 9; y++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(this.Location.X - 11, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 7; x <= this.Location.X + 7; x++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(x, this.Location.Y + 12, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int x = this.Location.X - 7; x <= this.Location.X + 7; x++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(x, this.Location.Y - 12, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 7; y <= this.Location.Y + 7; y++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(this.Location.X + 12, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
			
			for(int y = this.Location.Y - 7; y <= this.Location.Y + 7; y++)
			{
				new PoisonFieldSpell.InternalItem(0x3915, new Point3D(this.Location.X - 12, y, this.Location.Z), this, this.Map, TimeSpan.FromSeconds(seconds));
			}
		}
		
		public void ArcaneDemonStageSpecial(Mobile from)
		{
			if (from is BaseCreature 
				&& (((BaseCreature)from).Controlled || ((BaseCreature)from).Summoned))
			{
				Mobile master = ((BaseCreature)from).GetMaster();
				
				if (master is PlayerMobile)
				{
					from = master;
				}
			}
			
			BaseCreature creature = new AncientLich();
			
			creature.Name = "a soul eater";
			creature.Hue = ARCANEDEMON_HUE;
			creature.Kills = 5;
			creature.MoveToWorld(Location, Map);
			
			if(from is PlayerMobile && !from.Deleted && from.Alive
				&& from.Location is object && from.Map is object)
			{
				List<Mobile> targets = GetAreaTargets(SPECIAL_RANGE);
				
				if(targets.Contains(from))
				{
					creature.MoveToWorld(from.Location, from.Map);					
				}
			}
			
			Effects.SendTargetEffect(creature, 0x3728, 10, 30, ARCANEDEMON_HUE - 1, 0);
			creature.Say("I will devour your soul!", m_SpeechHue, true);
		}
		
		public void TryStageSpecial(Mobile from)
		{
			long tc = Core.TickCount;
			
			if( tc >= m_NextSpecialAbility )
			{
				m_NextSpecialAbility = tc + SPECIAL_COOLDOWN;
				
				switch (m_QueenBossStage)
				{
					case QueenBossStage.Daemon:
						DaemonStageSpecial();
						break;
					case QueenBossStage.Gargoyle:
						GargoyleStageSpecial();
						break;
					case QueenBossStage.Dragon:
						DragonStageSpecial();
						break;
					case QueenBossStage.ArcaneDemon:
						ArcaneDemonStageSpecial(from);
						break;
				}
			}
		}
		
		public void ChainLightning()
		{			
			foreach (Mobile m in m_Enemies)
			{
				if (m == null || m.Deleted)
				{
					continue;
				}
				
                if (m.Alive)
				{
					Effects.SendBoltEffect(m);
					AOS.Damage(m, this, 10, 100, 0, 0, 0, 0); // 10 damage
				}
			}
		}
		
		private void CheckState()
		{
			if (Hits < MAX_HITS * 0.9 
				&& Hits >= MAX_HITS * 0.7 
				&& m_QueenBossStage != QueenBossStage.Daemon)
			{	
				m_NextSpecialAbility = Core.TickCount + SPECIAL_COOLDOWN;
				SetDaemonState();
			} 
			else if (Hits < MAX_HITS * 0.7 
				&& Hits >= MAX_HITS * 0.5 
				&& m_QueenBossStage != QueenBossStage.Gargoyle)
			{
				m_NextSpecialAbility = Core.TickCount + SPECIAL_COOLDOWN;
				SetGargoyleState();
			}
			else if (Hits < MAX_HITS * 0.5 
				&& Hits >= MAX_HITS * 0.3 
				&& m_QueenBossStage != QueenBossStage.Dragon)
			{
				m_NextSpecialAbility = Core.TickCount + SPECIAL_COOLDOWN;
				SetDragonState();
			}
			else if (Hits < MAX_HITS * 0.3 
				&& m_QueenBossStage != QueenBossStage.ArcaneDemon)
			{
				m_NextSpecialAbility = Core.TickCount + SPECIAL_COOLDOWN;
				SetArcaneDemonState();
			}
		}
		
		public override bool OnBeforeDeath()
		{
			GoldShower.Do(Location, Map, 80, 999, 9999);
			
			// Hit all players that did damage with 5 lightning strikes.
			Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), 5, new TimerCallback(ChainLightning));
			
			// Give out reward to all players that did damage.
			Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(GiveReward));

			return base.OnBeforeDeath();
		}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            
            c.DropItem(new EvilCloth(4));

            c.DropItem(new SocketDeedPlusOne());
            c.DropItem(new SocketDeedPlusOne());
            c.DropItem(new GemRecoveryHammer());

            c.DropItem(new Shield25());
            c.DropItem(new DeathStatue1());
            c.DropItem(new DeathStatue2());
            c.DropItem(new GargishTotemDeed());

            c.DropItem(new Painting1());
            c.DropItem(new Painting2());
            c.DropItem(new Painting3());
            c.DropItem(new Painting4());
            c.DropItem(new Painting5());
            c.DropItem(new Painting6());
            c.DropItem(new Painting7());
            c.DropItem(new Painting8());
            c.DropItem(new Painting9());
            c.DropItem(new Painting10());
            c.DropItem(new Painting11());
            c.DropItem(new Painting12());
            c.DropItem(new Painting13());
            c.DropItem(new Painting14());
            c.DropItem(new Painting15());

            c.DropItem(new GardenShedDeed());                       

            if (Utility.RandomDouble() < 1.00)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        c.DropItem(new GoldGem());
                        break;
                    case 1:
                        c.DropItem(new PoisonGem());
                        break;
                    case 2:
                        c.DropItem(new AttackGem());
                        break;
                }
            }      
        }

        // Add player to enemy list once they've done damage (used for rewards).
        public void AddToEnemies(Mobile from)
		{
			if (from is BaseCreature 
				&& (((BaseCreature)from).Controlled || ((BaseCreature)from).Summoned))
			{
				Mobile master = ((BaseCreature)from).GetMaster();
				
				if (master is PlayerMobile)
				{
					from = master;
				}
			}
			
			if(from is PlayerMobile && !m_Enemies.Contains(from))
			{
				m_Enemies.Add(from);
			}
		}
		
		public void TryTeleportEnemy()
		{
			long tc = Core.TickCount;
			
			if( tc >= m_TryNextTeleport )
			{
				m_TryNextTeleport = tc + 6000;
				
				List<Mobile> targets = GetAreaTargets(SPECIAL_RANGE);
			
				foreach (Mobile m in targets)
				{
					if(m is PlayerMobile && !m.Deleted && m.Alive && (m.Kills >= 5 || m.Criminal))
					{
						m.MoveToWorld(Location, Map);
						m.RevealingAction();
						Combatant = m;
						m.PlaySound(0x1FE);
						Say("You've been a very naughty " + (m.Female ? "girl" : "boy") + ", " + m.Name + "!", m_SpeechHue, true);
						
						return;
					}
				}
			}
		}

		public override void OnThink()
		{
			base.OnThink();
			TryTeleportEnemy();
		}
		
		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			base.OnDamage(amount, from, willKill);
			
			AddToEnemies(from);			
			CheckState();
			TryStageSpecial(from);
		}
		
		public override void OnHeal(ref int amount, Mobile from)
		{
			base.OnHeal(ref amount, from);
			
			CheckState();
		}
		
		// Get all Mobiles within range of boss.
		public List<Mobile> GetAreaTargets(int range)
		{
			List<Mobile> targets = new List<Mobile>();

			IPooledEnumerable eable = GetMobilesInRange(range);

			foreach (Mobile m in eable)
			{
				if (m == this || !this.CanBeHarmful(m))
				{
					continue;
				}

				if (m is BaseCreature 
					&& (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned))
				{
					targets.Add(m);
				}
				else if (m is PlayerMobile)
				{
					targets.Add(m);
				}
			}

			eable.Free();
			
			return targets;
		}
		
		public override ApplyPoisonResult ApplyPoison(Mobile from, Poison poison)
		{
			return ApplyPoisonResult.Immune;
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Enemies = new List<Mobile>();
			m_SpeechHue = 43;
			m_TryNextTeleport = Core.TickCount;
			m_NextSpecialAbility = Core.TickCount;
			m_QueenBossStage = QueenBossStage.Human;
			SetHits(MAX_HITS);
		}
	}
}
