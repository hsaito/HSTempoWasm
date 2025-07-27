using Fluxor;

namespace HSTempoWasm.Store.VBI
{
    public class Feature : Feature<VBIState>
    {
        public override string GetName()
        {
            return "VBI State";
        }

        protected override VBIState GetInitialState()
        {
            return new VBIState(0);
        }
    }
}