using Server.Targeting;

namespace Server.Engines.BulkOrders
{
    public class LargeMobileBODTarget : Target
    {
        private readonly LargeMobileBOD m_Deed;

        public LargeMobileBODTarget(LargeMobileBOD deed) : base(18, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Deed.Deleted || !m_Deed.IsChildOf(from.Backpack))
                return;

            m_Deed.EndMobileCombine(from, targeted);
        }
    }
}