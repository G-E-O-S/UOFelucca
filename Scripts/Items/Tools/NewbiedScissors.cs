using Server;
using Server.Items;

namespace YourNamespace
{
    public class NewbiedScissors : Scissors
    {
        [Constructable]
        public NewbiedScissors()
            : base()
        {
            LootType = LootType.Newbied; // Set the loot type to Newbied
            UsesRemaining = 5000;
        }

        public NewbiedScissors(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
