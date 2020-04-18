using TaleWorlds.CampaignSystem;

namespace BearMyBanner.wrappers
{
    class MbParty : IParty
    {
        private readonly PartyBase _wrappedParty;

        public MbParty(PartyBase wrappedParty)
        {
            _wrappedParty = wrappedParty;
        }

        public string Name => _wrappedParty.Name.ToString();

        public override int GetHashCode()
        {
            return _wrappedParty.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is MbParty other)
            {
                return _wrappedParty.Equals(other._wrappedParty);
            }

            return false;
        }
    }
}