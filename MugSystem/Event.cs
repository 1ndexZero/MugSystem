namespace MugSystem
{
    public interface IIntervalEvent<T>
    {
        BeatTime TimeStart { get; set; }
        BeatTime TimeEnd { get; set; }
        T ValueStart { get; set; }
        T ValueEnd { get; set; }

        T GetValue(BeatTime time, BpmEventList bpmList);
    }

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

    public class EasingEvent : IIntervalEvent<double>
    {
        public BeatTime TimeStart { get; set; }
        public BeatTime TimeEnd { get; set; }
        public double ValueStart { get; set; }
        public double ValueEnd { get; set; }
        public TransformType TransformType { get; set; }
        public EasingType EasingType { get; set; }

        public EasingEvent(BeatTime timeStart, BeatTime timeEnd,
            double valueStart, double valueEnd,
            TransformType transformType, EasingType easingType)
        {
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            ValueStart = valueStart;
            ValueEnd = valueEnd;
            TransformType = transformType;
            EasingType = easingType;
        }

        public double GetValue(BeatTime time, BpmEventList bpmList)
        {
            double ts = bpmList.ConvertToMs(TimeStart);
            double te = bpmList.ConvertToMs(TimeEnd);
            double t = bpmList.ConvertToMs(time);

            return ValueStart
                + Easing.GetValue(TransformType, EasingType, (t - ts) / (te - ts))
                * (ValueEnd - ValueStart);
        }
    }
}
