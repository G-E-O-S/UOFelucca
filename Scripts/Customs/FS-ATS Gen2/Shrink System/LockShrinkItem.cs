using Server.Items;

namespace Server.ContextMenus
{
    public class LockShrinkItem : ContextMenuEntry
    {
        private readonly Mobile m_From;
        private readonly ShrinkItem m_ShrinkItem;

        public LockShrinkItem(Mobile from, ShrinkItem shrink) : base(2029, 5)
        {
            m_From = from;
            m_ShrinkItem = shrink;
        }

        public override void OnClick()
        {
            if (m_From != m_ShrinkItem.PetOwner)
            {
                m_From.SendMessage("You do not own this pet.");
            }
            else
            {
                m_ShrinkItem.Lock = true;
                m_From.SendMessage(38, "You have locked the pet so only you can reclaim it.");
            }
        }
    }
}