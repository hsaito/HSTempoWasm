using Fluxor;

namespace HSTempoWasm.Store.AudibleBeat
{
    // Reducer for AudibleBeat state
    public class AudibleBeatReducers
    {
        [ReducerMethod]
        public AudibleState ResetAudibleState(AudibleState state, ResetAudible action)
        {
            // Reset state to default (false)
            return new AudibleState(false);
        }
        
        [ReducerMethod]
        public AudibleState ToggleAudibleState(AudibleState state, ToggleAudible action)
        {
            // Toggle state
            return new AudibleState(!state.Audible);
        }
    }
}