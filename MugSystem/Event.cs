using System;

namespace MugSystem
{
    public class PointEvent<T>
    {
        public BeatTime Time { get; set; }
        public T Value { get; set; }

        public PointEvent(BeatTime time, T value)
        {
            Time = time;
            Value = value;
        }
    }

    public abstract class CurveEvent<T> : PointEvent<T>
    {
        public BeatTime TimeLength { get; set; }
        public T ValueChanged { get; set; }

        public CurveEvent(BeatTime time, BeatTime timeLength, T value, T valueChanged)
            : base(time, value)
        {
            TimeLength = timeLength;
            ValueChanged = valueChanged;
        }

        public abstract T GetValueByMs(double time, BpmEventList bpmEventList);

        public T GetValueByBeatTime(BeatTime time, BpmEventList bpmEventList) =>
            GetValueByMs(bpmEventList.ConvertToMs(time), bpmEventList);

        public T GetValueByBeatTime(double time, BpmEventList bpmEventList) =>
            GetValueByMs(bpmEventList.ConvertToMs(time), bpmEventList);
    }

    public class EasingEvent : CurveEvent<double>
    {
        public TransformType TransformType { get; set; }
        public EasingType EasingType { get; set; }

        public EasingEvent(BeatTime time, BeatTime timeLength, double value, double valueChanged,
            TransformType transformType, EasingType easingType)
            : base(time, timeLength, value, valueChanged)
        {
            TransformType = transformType;
            EasingType = easingType;
        }

        public override double GetValueByMs(double time, BpmEventList bpmEventList)
        {
            double ts = bpmEventList.ConvertToMs(Time);
            double te = bpmEventList.ConvertToMs(Time + TimeLength);

            return Value
                + Easing.GetValue(TransformType, EasingType, (time - ts) / (te - ts))
                * (ValueChanged - Value);
        }
    }

    public class SpeedEvent : CurveEvent<double>
    {
        public SpeedEvent(BeatTime time, BeatTime timeLength, double value, double valueChanged)
            : base(time, timeLength, value, valueChanged) { }

        public override double GetValueByMs(double time, BpmEventList bpmEventList)
        {
            double ts = bpmEventList.ConvertToMs(Time);
            double te = bpmEventList.ConvertToMs(Time + TimeLength);

            return Value + (ValueChanged - Value) * (time - ts) / (te - ts);
        }

        public double GetDisplacementByBeatTime(BeatTime time, BpmEventList bpmEventList) =>
            GetDisplacementByMs(bpmEventList.ConvertToMs(time), bpmEventList);

        public double GetDisplacementByBeatTime(double time, BpmEventList bpmEventList) =>
            GetDisplacementByMs(bpmEventList.ConvertToMs(time), bpmEventList);

        public double GetDisplacementByMs(double time, BpmEventList bpmEventList)
        {
            double ts = bpmEventList.ConvertToMs(Time);
            double te = bpmEventList.ConvertToMs(Time + TimeLength);

            return (Value +
                (Value + (ValueChanged - Value) * (time - ts) / (te - ts))
                ) * (time - ts) / 2;
        }

        public double GetDisplacementAll(BpmEventList bpmEventList)
        {

            double ts = bpmEventList.ConvertToMs(Time);
            double te = bpmEventList.ConvertToMs(Time + TimeLength);

            return (Value + ValueChanged) * (te - ts) / 2;
        }

    }
}
