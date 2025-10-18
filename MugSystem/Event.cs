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

        public virtual T GetValue(BeatTime time, BpmEventList bpmList)
        {
            return Value;
        }
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

        public override double GetValue(BeatTime time, BpmEventList bpmList)
        {
            double ts = bpmList.ConvertToMs(Time);
            double te = bpmList.ConvertToMs(Time + TimeLength);
            double t = bpmList.ConvertToMs(time);

            return Value
                + Easing.GetValue(TransformType, EasingType, (t - ts) / (te - ts))
                * (ValueChanged - Value);
        }
    }
}
