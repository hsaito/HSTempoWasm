using Fluxor;

namespace HSTempoWasm.Store.AudibleBeat
{
    public class Feature : Feature<AudibleState>
    {
        public override string GetName()
        {
            return "Audible Mode";
        }

        protected override AudibleState GetInitialState()
        {
            return new AudibleState(false);
        }
    }
}