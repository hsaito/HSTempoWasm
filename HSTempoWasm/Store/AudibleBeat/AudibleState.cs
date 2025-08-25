namespace HSTempoWasm.Store.AudibleBeat
{
    public class AudibleState
    {
        public AudibleState(bool audible)
        {
            Audible = audible;
        }

        public bool Audible { get; }
    }
}