namespace HSTempoWasm
{
    using System;

    public static class TempoLogic
    {
        public static short CalculateBPMtoMS(double bpm)
        {
            if (bpm <= 0)
                return 0;
            var ms = Math.Round(60000 / bpm);
            if (ms > short.MaxValue)
                return short.MaxValue;
            if (ms < short.MinValue)
                return short.MinValue;
            return Convert.ToInt16(ms);
        }
    }
}