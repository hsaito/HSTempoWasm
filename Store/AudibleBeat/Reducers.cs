using Fluxor;

namespace HSTempoWasm.Store.AudibleBeat
{
    public class Reducers
    {
        [ReducerMethod]
        public static AudibleState ResetAudibleState(AudibleState state, ResetAudible action)
        {
            return new AudibleState(false);
        }
        
        [ReducerMethod]
        public static AudibleState ToggleAudibleState(AudibleState state, ToggleAudible action)
        {
            return new AudibleState(!state.Audible);
        }
    }
}