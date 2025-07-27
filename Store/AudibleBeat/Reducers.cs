using Fluxor;

namespace HSTempoWasm.Store.AudibleBeat
{
    // Reducer for AudibleBeat state
    public class AudibleBeatReducers
    {
        [ReducerMethod]
        public static AudibleState ResetAudibleState(AudibleState state, ResetAudible action)
        {
            // Reset AudibleBeat state to default (false)
            return new AudibleState(false);
        }
        
        [ReducerMethod]
        public static AudibleState ToggleAudibleState(AudibleState state, ToggleAudible action)
        {
            // Toggle AudibleBeat state
            return new AudibleState(!state.Audible);
        }
    }
}