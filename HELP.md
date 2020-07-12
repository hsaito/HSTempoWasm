# Help

WARNING: Various factors may affect accuracy of
the measurement. Do not rely on this system for mission critical
applications. The HSTempoWasm developers are not responsible for any
damages caused by use of this system.

How to use
----------

tl;dr: Tap the *Beat* button to the subject you are measuring. Press the
*Reset* button to reset.

## The HSTempoWasm interface

### Buttons {#buttons .card-title}

-   *Beat* button \-- Press to measure beat.
-   *Reset* button \-- Reset to to start new measurement.
-   *Audible Beat* \-- Enables the audible beat (tick).
-   *Test Beat* \-- Sound the beat to adjust its volume. Mobile user
    should press this button before enabling *Audible Beat* for this
    feature to work.
-   *Download Metric* \-- Download a metric file containing the nature
    of the measurement you\'ve taken.
-   *Adjust Up* button \-- Adjust the current BPM up one, changes the
    *VBI* and *BPM Interval* display.
-   *Adjust Down* button \-- Adjust the current BPM down one, changes
    the *VBI* and *BPM Interval* display.
-   *Rebase* button \-- Reset the beat timing without resetting the
    beat. (If the measured beat is not aligned with the subject, use
    this button.)

### Indicators and Display Elements

Because the measurement requires two beats registered to measure an
interval, most of the display below only activates after the second beat
is registered. (Except for *Stability* which requires 20.)

-   *VBI* (Visual Beat Indicator) \-- Displays current beat visually.
    *Adjust Up* and *Adjust Down* will change this interval. Use a
    dropdown on the right to select singular VBI, meter of 2/4, 3/4,
    4/4.
-   *Elapsed Time* \-- Displays second since the start of the
    measurement.
-   *Beat Count* \-- Displays the number of beat registered.
-   *Average (10/15/20)* \-- Displays 10, 15, 20 beats BPM average.
-   *BPM* \-- Displays current BPM (Beats-Per-Minute).
-   *Stability* \-- Displays how stable (consistent) the measurement is.
    Based on *Average (10/15/20)* measurements. This will only activate
    only after 20th measurements.
-   *Last* \-- Displays milliseconds of the last measurement.
-   *Average* \-- Displays average milliseconds of the measurements.
-   *BPM Interval* \-- Displays milliseconds interval based on the
    *BPM*. *Adjust Up* and *Adjust Down* will change this interval.

## How to get the measurement

Press the *Beat* button with the rhythm. Interval is displayed as
current BPM. The *BPM* displayed is the averaged BPM over time.
Therefore, more beat you register, the measurement is more stable
representation of the intervals you are trying to register. Use the
*Adjust Up* and *Adjust Down* buttons to fine tune measured *BPM* and
compare *VBI* and audible beat (if enabled) with the subject you are
trying to measure.

### Elements that can affect accurate measurements

There are various elements that can affect accuracy of the measurements.
But here are some examples:

-   Variance of the subject is fluctuating \-- this is most prominent
    for live music and classical music.
-   Clock shift in your computer \-- Clock on your computer is only
    accurate to certain extent and can present inaccurate readings.
-   Instable input \-- It is possible your input is not consistent.

### How to get better measurements

-   Try multiple measurements \-- see if multiple measurements are
    consistent.
-   Try different part of subject \-- some parts are easier to measure
    than others. Do note some subject may have fluctuating rhythms.