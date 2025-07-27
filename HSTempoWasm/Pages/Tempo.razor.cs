using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;
using Fluxor;
using HSTempoWasm.Store.AudibleBeat;
using HSTempoWasm.Store.VBI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HSTempoWasm.Pages
{
    public partial class Tempo
    {
        // Fields (no redundant initialization)
        private int currentCount;
        private int elapsedSecond;
        private double currentBpm;
        private int averageMs;

        // Constants for BPM array sizes
        private const int BpmArraySize10 = 10;
        private const int BpmArraySize15 = 15;
        private const int BpmArraySize20 = 20;

        // BPM values and averages
        private double[] bpmValues10 = new double[BpmArraySize10];
        private double[] bpmValues15 = new double[BpmArraySize15];
        private double[] bpmValues20 = new double[BpmArraySize20];

        private string bpmAverage10 = "X";
        private string bpmAverage15 = "X";
        private string bpmAverage20 = "X";

        private int bpmIndex10;
        private int bpmIndex15;
        private int bpmIndex20;

        // Timing and calculation fields
        private int bpmInterval;
        private int jitter;
        private double stability;

        // Styles
        private const string VbiInactiveStyle = "btn btn-dark";
        private const string VbiActiveStyle = "btn btn-success";

        // Time tracking
        private DateTime currentTime;
        private DateTime startTime;
        private DateTime lastTime;
        private double recentTimeMs;

        // Meter mode
        private string meterBoxMode = "VBI";
        private int meterBoxModeNumeric = 1;

        // UI references
        private ElementReference beatButton;

        // State injection (nullable for safety)
        [Inject] public IDispatcher? Dispatcher { get; set; }
        [Inject] private IState<AudibleState>? AudibleState { get; set; }
        [Inject] private IState<VBIState>? VbiState { get; set; }

        private async Task InitializeFocus()
        {
            await beatButton.FocusAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                InitializeFocus();
            }
        }

        private string vbi1 = VbiInactiveStyle;
        private string vbi2 = VbiInactiveStyle;
        private string vbi3 = VbiInactiveStyle;
        private string vbi4 = VbiInactiveStyle;

        void UpdateVbi(int mode)
        {
            switch (mode)
            {
                case 0:
                {
                    vbi1 = VbiInactiveStyle;
                    vbi2 = VbiInactiveStyle;
                    vbi3 = VbiInactiveStyle;
                    vbi4 = VbiInactiveStyle;
                    break;
                }
                case 1:
                {
                    vbi1 = VbiActiveStyle;
                    vbi2 = VbiInactiveStyle;
                    vbi3 = VbiInactiveStyle;
                    vbi4 = VbiInactiveStyle;
                    break;
                }
                case 2:
                {
                    vbi1 = VbiInactiveStyle;
                    vbi2 = VbiActiveStyle;
                    vbi3 = VbiInactiveStyle;
                    vbi4 = VbiInactiveStyle;
                    break;
                }
                case 3:
                {
                    vbi1 = VbiInactiveStyle;
                    vbi2 = VbiInactiveStyle;
                    vbi3 = VbiActiveStyle;
                    vbi4 = VbiInactiveStyle;
                    break;
                }
                case 4:
                {
                    vbi1 = VbiInactiveStyle;
                    vbi2 = VbiInactiveStyle;
                    vbi3 = VbiInactiveStyle;
                    vbi4 = VbiActiveStyle;
                    break;
                }
                case 5:
                {
                    vbi1 = VbiActiveStyle;
                    vbi2 = VbiActiveStyle;
                    vbi3 = VbiActiveStyle;
                    vbi4 = VbiActiveStyle;
                    break;
                }
            }
        }

        // Fix GetKeyPress to async
        async void GetKeyPress(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "r":
                case "R":
                {
                    await Reset();
                    break;
                }

                case "b":
                case "B":
                {
                    await InitializeFocus();
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
            [JsonPropertyName("jitter")] public int Jitter { get; set; }
        }

        private class Record
        {
            [JsonPropertyName("average_bpm")] public int AverageBpm { get; set; }
            [JsonPropertyName("average_ms")] public int AverageMs { get; set; }
            [JsonPropertyName("metric")] public List<MetricInfo> Metric { get; set; }
        }

        private Record? sessionRecord;

        private void ExecBeat()
        {
            currentTime = DateTime.Now;
            var accumulatedMs = currentTime - startTime;
            currentBpm = Math.Round((currentCount / accumulatedMs.TotalMilliseconds) * 60000);
            currentCount++;

            if (Timers.BeatTimer == null || Timers.BeatTimer.Enabled != true)
            {
                sessionRecord = new Record();
                sessionRecord.Metric = new List<MetricInfo>();
                StartTimer();
            }

            if (currentCount > 1)
            {
                recentTimeMs = UpdateRecentTime().TotalMilliseconds;
                Timers.VdiTock.Enabled = false;
                Timers.VdiTick.Interval = 60000 / currentBpm;
                Timers.VdiTock.Interval = 60000 / currentBpm / 3;
                Timers.VdiTick.Enabled = true;
                bpmInterval = CalculateBPMtoMS();
                averageMs = (int) (accumulatedMs.TotalMilliseconds / currentCount);
                // Calculate jitter
                jitter = (int) Math.Abs(averageMs - bpmInterval);

                bpmValues10[bpmIndex10] = currentBpm;
                bpmIndex10++;
                if (bpmIndex10 > BpmArraySize10 - 1)
                    bpmIndex10 = 0;

                bpmValues15[bpmIndex15] = currentBpm;
                bpmIndex15++;
                if (bpmIndex15 > BpmArraySize15 - 1)
                    bpmIndex15 = 0;

                bpmValues20[bpmIndex20] = currentBpm;
                bpmIndex20++;
                if (bpmIndex20 > BpmArraySize20 - 1)
                    bpmIndex20 = 0;

                double bpmAverageValue10 = 0, bpmAverageValue15 = 0, bpmAverageValue20 = 0;

                if (currentCount > BpmArraySize10)
                {
                    bpmAverageValue10 = Math.Round(bpmValues10.Sum() / BpmArraySize10);
                    bpmAverage10 = bpmAverageValue10.ToString();
                }

                if (currentCount > BpmArraySize15)
                {
                    bpmAverageValue15 = Math.Round(bpmValues15.Sum() / BpmArraySize15);
                    bpmAverage15 = bpmAverageValue15.ToString();
                }

                if (currentCount > BpmArraySize20)
                {
                    bpmAverageValue20 = Math.Round(bpmValues20.Sum() / BpmArraySize20);
                    bpmAverage20 = bpmAverageValue20.ToString();
                    stability = (20 - Math.Abs(currentBpm -
                                               (bpmAverageValue10 + bpmAverageValue15 + bpmAverageValue20) / 3)) / 20 *
                                100;
                }

                sessionRecord.AverageBpm = (int) currentBpm;
                sessionRecord.AverageMs = (int) averageMs;

                var newMetric = new MetricInfo
                {
                    MeasuredBpm = (int) currentBpm,
                    Count = (int) currentCount,
                    Elapsed = new TimeSpan(0, 0, elapsedSecond),
                    Ms = (int) recentTimeMs,
                    Jitter = (int) jitter
                };

                sessionRecord.Metric.Add(newMetric);
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
            Dispatcher.Dispatch(new VBIResetAction());
            UpdateVbi(VbiState.Value.VBIStateNumber);
        }

        private short CalculateBPMtoMS()
        {
            return currentBpm <= 0 ? (short) 0 : Convert.ToInt16(Math.Round(60000 / currentBpm));
        }


        private async Task DownloadJson()
        {
            var result = GenerateMetricJson();
            if (result != null)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(result);
                await SaveAs(JSRuntime, "result.json", bytes);
            }
        }

        private string GenerateMetricJson()
        {
            return sessionRecord != null ? JsonSerializer.Serialize(sessionRecord) : null;
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
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled && currentBpm > 0)
            {
                Dispatcher.Dispatch(new VBIResetAction());
                UpdateVbi(VbiState.Value.VBIStateNumber);
                Timers.VdiTick.Enabled = false;
                Timers.VdiTock.Enabled = false;
                Timers.VdiTick.Enabled = true;
            }
        }

        private void AdjustUp()
        {
            if (Timers.BeatTimer != null && Timers.BeatTimer.Enabled && currentBpm > 0)
            {
                Timers.VdiTick.Enabled = false;
                Timers.VdiTock.Enabled = false;
                currentBpm++;
                Timers.VdiTick.Interval = 60000 / currentBpm;
                Timers.VdiTock.Interval = 60000 / currentBpm / 3;
                Timers.VdiTick.Enabled = true;
                bpmInterval = CalculateBPMtoMS();
            }
        }

        private void AdjustDown()
        {
            if (Timers.BeatTimer == null || !Timers.BeatTimer.Enabled || !(currentBpm > 0)) return;
            Timers.VdiTick.Enabled = false;
            Timers.VdiTock.Enabled = false;
            currentBpm--;
            Timers.VdiTick.Interval = 60000 / currentBpm;
            Timers.VdiTock.Interval = 60000 / currentBpm / 3;
            Timers.VdiTick.Enabled = true;
            bpmInterval = CalculateBPMtoMS();
        }

        // Event handler nullability
        private void ProcessTock(object? sender, ElapsedEventArgs e)
        {
            Dispatcher?.Dispatch(new VBITock(meterBoxModeNumeric));
            Timers.VdiTock.Enabled = false;
            UpdateVbi(VbiState?.Value.VBIStateNumber ?? 0);
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

        // Event handler nullability
        private void ProcessTick(object? sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (AudibleState?.Value.Audible == true)
            {
                JSRuntime.InvokeVoidAsync("playBeat");
            }
            Dispatcher?.Dispatch(new VBITick(meterBoxModeNumeric));
            UpdateVbi(VbiState?.Value.VBIStateNumber ?? 0);
            this.StateHasChanged();
            Timers.VdiTock.Enabled = true;
        }

        private async Task Reset()
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
            Dispatcher.Dispatch(new VBIResetAction());
            UpdateVbi(VbiState.Value.VBIStateNumber);

            currentCount = 0;
            averageMs = 0;
            bpmInterval = 0;
            jitter = 0;

            stability = 0;
            bpmAverage10 = "X";
            bpmAverage15 = "X";
            bpmAverage20 = "X";
            await beatButton.FocusAsync();
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

        private void ProcessSecond(object? sender, ElapsedEventArgs elapsedEventArgs)
        {
            elapsedSecond++;
            StateHasChanged();
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
            currentBpm = 0;
            recentTimeMs = 0;
        }

        private void ToggleAudibleCheck()
        {
            Dispatcher.Dispatch(new ToggleAudible());
            InvokeAsync(StateHasChanged);
        }


        private async Task HandleMeterUpdate(ChangeEventArgs arg)
        {
            meterBoxMode = (string) arg.Value;

            meterBoxModeNumeric = meterBoxMode switch
            {
                "VBI" => 1,
                "2/4" => 2,
                "3/4" => 3,
                "4/4" => 4,
                _ => meterBoxModeNumeric
            };

            Dispatcher.Dispatch(new VBIResetAction());
            UpdateVbi(VbiState.Value.VBIStateNumber);
            await Task.CompletedTask;
        }
    }
}
