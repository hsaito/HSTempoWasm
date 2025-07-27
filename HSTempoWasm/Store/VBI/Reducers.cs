using System;
using Fluxor;

namespace HSTempoWasm.Store.VBI
{
    // VBI state reducer
    public class VBIReducers
    {
        private const int VbiModeSingle = 1;
        private const int VbiTickValue = 5;

        [ReducerMethod]
        public static VBIState ResetVBIState(VBIState state, VBIResetAction action)
        {
            // Reset VBI state
            return new VBIState(0);
        }
        
        [ReducerMethod]
        public static VBIState VBITick(VBIState state, VBITick action)
        {
            if (action.VBIMode == VbiModeSingle)
            {
                // Single mode: set to fixed value
                return new VBIState(VbiTickValue);
            }
            else
            {
                var meter = action.VBIMode;
                // If less than meter, increment; otherwise reset to 1
                return state.VBIStateNumber < meter ? new VBIState(state.VBIStateNumber+1) : new VBIState(1);
            }
        }
        
        [ReducerMethod]
        public static VBIState VBITock(VBIState state, VBITock action)
        {
            // Single mode: reset to 0; otherwise keep current state
            return action.VBIMode == VbiModeSingle ? new VBIState(0) : new VBIState(state.VBIStateNumber);
        }
    }
}