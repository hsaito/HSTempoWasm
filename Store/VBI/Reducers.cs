using System;
using Fluxor;

namespace HSTempoWasm.Store.VBI
{
    public class Reducers
    {
        [ReducerMethod]
        public static VBIState ResetAudibleState(VBIState state, VBIResetAction action)
        {
            return new VBIState(0);
        }
        
        [ReducerMethod]
        public static VBIState VBITick(VBIState state, VBITick action)
        {
            if (action.VBIMode == 1)
            {
                return new VBIState(5);
            }
            else
            {
                var meter = action.VBIMode;
                return state.VBIStateNumber < meter ? new VBIState(state.VBIStateNumber+1) : new VBIState(1);
            }
        }
        
        [ReducerMethod]
        public static VBIState VBITock(VBIState state, VBITock action)
        {
            return action.VBIMode == 1 ? new VBIState(0) : new VBIState(state.VBIStateNumber);
        }
    }
}