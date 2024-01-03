using Server.Gumps;
using Server.Mobiles;
using Server.SkillHandlers;

namespace Server.ContextMenus
{
    public class PetMenu : ContextMenuEntry
    {
        private readonly Mobile m_From;
        private readonly BaseCreature m_Bc;

        public PetMenu(Mobile from, BaseCreature Bc) : base(5031, 5)
        {
            m_From = from;
            m_Bc = Bc;
        }

        public override void OnClick()
        {
            m_From.SendGump(new PetLevelGump(m_Bc));
            m_From.SendGump(new AnimalLoreGump(m_Bc));
            //m_From.SendGump(new PetStatGump(m_Bc, m_From));
        }
    }
}
