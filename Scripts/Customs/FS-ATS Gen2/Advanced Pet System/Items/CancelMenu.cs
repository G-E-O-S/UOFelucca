using Server.Items;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class BreedingCancelMenu : ContextMenuEntry
    {
        private readonly Mobile m_From;
        private readonly PetClaimTicket m_Cancel;

        public BreedingCancelMenu(Mobile from, PetClaimTicket cancel) : base(0091, 5)
        {
            m_From = from;
            m_Cancel = cancel;
        }

        public override void OnClick()
        {
            if (m_From is PlayerMobile)
            {
                BaseCreature bc = (BaseCreature)m_Cancel.Pet;
                PlayerMobile pm = (PlayerMobile)m_From;
                bc.IsStabled = true;
                pm.Stabled.Add(m_Cancel.Pet);

                m_From.SendMessage("You have canceled breeding, Your pet has been returned to the stable, You may claim it their.");
                m_Cancel.Delete();
            }
        }
    }
}