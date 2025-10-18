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

    public class EasingEvent : PointEvent<double>
    {
        public BeatTime TimeLength { get; set; }
        public double ValueChanged { get; set; }
        public TransformType TransformType { get; set; }
        public EasingType EasingType { get; set; }

        public EasingEvent(BeatTime time, BeatTime timeLength, double value, double valueChanged,
            TransformType transformType, EasingType easingType)
            : base(time, value)
        {
            Time = time;
            TimeLength = timeLength;
            Value = value;
            ValueChanged = valueChanged;
            TransformType = transformType;
            EasingType = easingType;
        }

        public double GetValue(BeatTime time, BpmEventList bpmList)
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
