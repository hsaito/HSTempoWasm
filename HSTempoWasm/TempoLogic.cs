namespace HSTempoWasm
{
    using System;

    public static class TempoLogic
    {
        public static short CalculateBPMtoMS(double bpm)
        {
            return bpm <= 0 ? (short)0 : Convert.ToInt16(Math.Round(60000 / bpm));
        }
    }
}

