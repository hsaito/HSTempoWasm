using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;
using Fluxor;
using HSTempoWasm.Store.AudibleBeat;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace HSTempoWasm.Pages
{
    public partial class Tempo
    {
        private int currentCount = 0;
        private double currentBPM = 0;
        private int elapsedSecond = 0;

        double[] bpmValue10 = new double[10];
        double[] bpmValue15 = new double[15];
        double[] bpmValue20 = new double[20];

        string bpmAverage10 = "X";
        string bpmAverage15 = "X";
        string bpmAverage20 = "X";

        int bpmPoint10 = 0;
        int bpmPoint15 = 0;
        int bpmPoint20 = 0;

        int averageMS = 0;
        int bpmInterval = 0;

        bool vdiCheck;

        private DateTime currentTime;
        private DateTime startTime;
        private DateTime lastTime;
        private double recentTimeMs;

        [Inject] public IDispatcher Dispatcher { get; set; }
        [Inject] private IState<AudibleState> AudibleState { get; set; }
        
        double stability = 0;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                JSRuntime.InvokeVoidAsync("setFocus", "beatButton");
            }
        }

        void GetKeyPress(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "r":
                case "R":
                {
                    Reset();
                    break;
                }

                case "b":
                case "B":
                {
                    JSRuntime.InvokeVoidAsync("setFocus", "beatButton");
                    break;
                }

                case "u":
                case "U":
                {
                    AdjustUp();
                    break;
                }

                case "d":
                case "D":
                {
                    AdjustDown();
                    break;
                }
            }
        }


        private class MetricInfo
        {
            [JsonPropertyName("elapsed")] public TimeSpan Elapsed { get; set; }
            [JsonPropertyName("count")] public int Count { get; set; }
            [JsonPropertyName("measured_bpm")] public int MeasuredBpm { get; set; }
            [JsonPropertyName("ms")] public int Ms { get; set; }
        }

        private class Record
        {
            [JsonPropertyName("average_bpm")] public int AverageBpm { get; set; }
            [JsonPropertyName("average_ms")] public int AverageMs { get; set; }
            [JsonPropertyName("metric")] public List<MetricInfo> Metric { get; set; }
        }

        private Record _sessionRecord = null;

        private void ExecBeat()
        {
            currentTime = DateTime.Now;
            var accumulatedMs = currentTime - startTime;
            currentBPM = Math.Round((currentCount / accumulatedMs.TotalMilliseconds) * 60000);
            currentCount++;

            if (Timers.BeatTimer == null || Timers.BeatTimer.Enabled != true)
            {
                _sessionRecord = new Record();
                _sessionRecord.Metric = new List<MetricInfo>();
                StartTimer();
            }

            if (currentCount > 1)
            {
                recentTimeMs = UpdateRecentTime().TotalMilliseconds;
                Timers.VdiTock.Enabled = false;
                Timers.VdiTick.Interval = 60000 / currentBPM;
                Timers.VdiTock.Interval = 60000 / currentBPM / 3;
                Timers.VdiTick.Enabled = true;
                bpmInterval = CalculateBPMtoMS();
                averageMS = (int) (accumulatedMs.TotalMilliseconds / currentCount);

                bpmValue10[bpmPoint10] = currentBPM;
                bpmPoint10++;
                if (bpmPoint10 > 9)
                    bpmPoint10 = 0;

                bpmValue15[bpmPoint15] = currentBPM;
                bpmPoint15++;
                if (bpmPoint15 > 14)
                    bpmPoint15 = 0;

                bpmValue20[bpmPoint20] = currentBPM;
                bpmPoint20++;
                if (bpmPoint20 > 19)
                    bpmPoint20 = 0;

                double bpmAverageValue10 = 0, bpmAverageValue15 = 0, bpmAverageValue20 = 0;

                if (currentCount > 10)
                {
                    bpmAverageValue10 = Math.Round(bpmValue10.Sum() / 10);
                    bpmAverage10 = bpmAverageValue10.ToString();
                }

                if (currentCount > 15)
                {
                    bpmAverageValue15 = Math.Round(bpmValue15.Sum() / 15);
                    bpmAverage15 = bpmAverageValue15.ToString();
                }

                if (currentCount > 20)
                {
                    bpmAverageValue20 = Math.Round(bpmValue20.Sum() / 20);
                    bpmAverage20 = bpmAverageValue20.ToString();
                    stability = (20 - Math.Abs(currentBPM -
                                               (bpmAverageValue10 + bpmAverageValue15 + bpmAverageValue20) / 3)) / 20 *
                                100;
                }

                _sessionRecord.AverageBpm = (int) currentBPM;
                _sessionRecord.AverageMs = (int) averageMS;
                var newMetric = new MetricInfo
                {
                    MeasuredBpm = (int) currentBPM,
                    Count = (int) currentCount,
                    Elapsed = new TimeSpan(0, 0, elapsedSecond),
                    Ms = (int) recentTimeMs
                };

                _sessionRecord.Metric.Add(newMetric);
            }
            else
            {
                Timers.VdiTick = new Timer();
                Timers.VdiTock = new Timer();
                Timers.VdiTick.Elapsed += ProcessTick;
                Timers.VdiTick.AutoReset = true;
                Timers.VdiTock.Elapsed += ProcessTock;
            }

            lastTime = currentTime;
        }

        private short CalculateBPMtoMS()
        {
            return currentBPM <= 0 ? (short) 0 : Convert.ToInt16(Math.Round(60000 / currentBPM));
        }


        private void DownloadJson()
        {
            var result = GenerateMetricJson();
            if (result != null)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(result);
                SaveAs(JSRuntime, "result.json", bytes);
            }
        }

        private string GenerateMetricJson()
        {
            if (_sessionRecord != null)
            {
                return JsonSerializer.Serialize(_sessionRecord);
            }

            return null;
        }

        public async static Task SaveAs(IJSRuntime js, string filename, byte[] data)
        {
            await js.InvokeAsync<object>(
                "saveAsFile",
                filename,
                Convert.ToBase64String(data));
        }

        private void Rebase()
        {
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled && currentBPM > 0)
            {
                Timers.VdiTick.Enabled = false;
                Timers.VdiTock.Enabled = false;
                Timers.VdiTick.Enabled = true;
            }
        }

        private void AdjustUp()
        {
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled && currentBPM > 0)
            {
                Timers.VdiTick.Enabled = false;
                Timers.VdiTock.Enabled = false;
                currentBPM++;
                Timers.VdiTick.Interval = 60000 / currentBPM;
                Timers.VdiTock.Interval = 60000 / currentBPM / 3;
                Timers.VdiTick.Enabled = true;
                bpmInterval = CalculateBPMtoMS();
            }
        }

        private void AdjustDown()
        {
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled && currentBPM > 0)
            {
                Timers.VdiTick.Enabled = false;
                Timers.VdiTock.Enabled = false;
                currentBPM--;
                Timers.VdiTick.Interval = 60000 / currentBPM;
                Timers.VdiTock.Interval = 60000 / currentBPM / 3;
                Timers.VdiTick.Enabled = true;
                bpmInterval = CalculateBPMtoMS();
            }
        }

        private void ProcessTock(object sender, ElapsedEventArgs e)
        {
            vdiCheck = false;
            Timers.VdiTock.Enabled = false;
            this.StateHasChanged();
        }

        private TimeSpan UpdateRecentTime()
        {
            return currentTime - lastTime;
        }

        private void TestBeat()
        {
            JSRuntime.InvokeVoidAsync("playBeat");
        }

        private void ProcessTick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (AudibleState.Value.Audible)
            {
                JSRuntime.InvokeVoidAsync("playBeat");
            }

            vdiCheck = true;
            this.StateHasChanged();
            Timers.VdiTock.Enabled = true;
        }

        private void Reset()
        {
            StopTimer();

            if (Timers.VdiTick != null)
            {
                Timers.VdiTick.Enabled = false;
                Timers.VdiTick.Close();
            }

            if (Timers.VdiTock != null)
            {
                Timers.VdiTock.Enabled = false;
                Timers.VdiTock.Close();
            }

            Dispatcher.Dispatch(new ResetAudible());
            vdiCheck = false;

            currentCount = 0;
            averageMS = 0;
            bpmInterval = 0;

            JSRuntime.InvokeVoidAsync("setFocus", "beatButton");

            stability = 0;
            bpmAverage10 = "X";
            bpmAverage15 = "X";
            bpmAverage20 = "X";
        }

        private void StartTimer()
        {
            startTime = DateTime.Now;
            Timers.BeatTimer = new Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            Timers.BeatTimer.Elapsed += ProcessSecond;
            Timers.BeatTimer.Enabled = true;
        }

        private void ProcessSecond(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            elapsedSecond++;
            this.StateHasChanged();
        }

        private void StopTimer()
        {
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled)
            {
                Timers.BeatTimer.Enabled = false;
                Timers.BeatTimer.Stop();
                Timers.BeatTimer.Close();
            }

            elapsedSecond = 0;
            currentBPM = 0;
            recentTimeMs = 0;
        }

        private void ToggleAudibleCheck()
        {
            Dispatcher.Dispatch(new ToggleAudible());
            InvokeAsync(StateHasChanged);
        }
    }
}