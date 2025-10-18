using System;

namespace MugSystem
{
    public enum TransformType
    {
        None, Linear, Sine,
        Quad, Cubic, Quart,
        Quint, Expo, Circ,
        Back, Elastic, Bounce
    }

    public enum EasingType { In, Out, InOut }

    public static class Easing
    {
        private const double C1 = 1.70158f;
        private const double C2 = C1 * 1.525f;
        private const double C3 = C1 + 1;
        private const double C4 = 2 * Math.PI / 3;
        private const double C5 = 2 * Math.PI / 4.5f;
        private const double N1 = 7.5625f;
        private const double D1 = 2.75f;

        public static double GetValue(TransformType transformType, EasingType easeType, double x)
        {
            switch (transformType)
            {
                case TransformType.None:
                    return 0;
                case TransformType.Linear:
                    return x;
                case TransformType.Sine:
                    switch (easeType)
                    {
                        case EasingType.In: return InSine(x);
                        case EasingType.Out: return OutSine(x);
                        case EasingType.InOut: return InOutSine(x);
                        default: return x;
                    }
                case TransformType.Quad:
                    switch (easeType)
                    {
                        case EasingType.In: return InQuad(x);
                        case EasingType.Out: return OutQuad(x);
                        case EasingType.InOut: return InOutQuad(x);
                        default: return x;
                    }
                case TransformType.Cubic:
                    switch (easeType)
                    {
                        case EasingType.In: return InCubic(x);
                        case EasingType.Out: return OutCubic(x);
                        case EasingType.InOut: return InOutCubic(x);
                        default: return x;
                    }
                case TransformType.Quart:
                    switch (easeType)
                    {
                        case EasingType.In: return InQuart(x);
                        case EasingType.Out: return OutQuart(x);
                        case EasingType.InOut: return InOutQuart(x);
                        default: return x;
                    }
                case TransformType.Quint:
                    switch (easeType)
                    {
                        case EasingType.In: return InQuint(x);
                        case EasingType.Out: return OutQuint(x);
                        case EasingType.InOut: return InOutQuint(x);
                        default: return x;
                    }
                case TransformType.Expo:
                    switch (easeType)
                    {
                        case EasingType.In: return InExpo(x);
                        case EasingType.Out: return OutExpo(x);
                        case EasingType.InOut: return InOutExpo(x);
                        default: return x;
                    }
                case TransformType.Circ:
                    switch (easeType)
                    {
                        case EasingType.In: return InCirc(x);
                        case EasingType.Out: return OutCirc(x);
                        case EasingType.InOut: return InOutCirc(x);
                        default: return x;
                    }
                case TransformType.Back:
                    switch (easeType)
                    {
                        case EasingType.In: return InBack(x);
                        case EasingType.Out: return OutBack(x);
                        case EasingType.InOut: return InOutBack(x);
                        default: return x;
                    }
                case TransformType.Elastic:
                    switch (easeType)
                    {
                        case EasingType.In: return InElastic(x);
                        case EasingType.Out: return OutElastic(x);
                        case EasingType.InOut: return InOutElastic(x);
                        default: return x;
                    }
                case TransformType.Bounce:
                    switch (easeType)
                    {
                        case EasingType.In: return InBounce(x);
                        case EasingType.Out: return OutBounce(x);
                        case EasingType.InOut: return InOutBounce(x);
                        default: return x;
                    }
                default:
                    return x;
            }
        }

        public static double InSine(double x) =>
            1 - Math.Cos(x * Math.PI / 2);

        public static double OutSine(double x) =>
            Math.Sin(x * Math.PI / 2);

        public static double InOutSine(double x) =>
            -(Math.Cos(Math.PI * x) - 1) / 2;

        public static double InQuad(double x) =>
            x * x;

        public static double OutQuad(double x) =>
            1 - (1 - x) * (1 - x);

        public static double InOutQuad(double x) =>
            x < 0.5 ? 2 * x * x : 1 - Math.Pow(-2 * x + 2, 2) / 2;

        public static double InCubic(double x) =>
            x * x * x;

        public static double OutCubic(double x) =>
            1 - Math.Pow(1 - x, 3);

        public static double InOutCubic(double x) =>
            x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;

        public static double InQuart(double x) =>
            x * x * x * x;

        public static double OutQuart(double x) =>
            1 - Math.Pow(1 - x, 4);

        public static double InOutQuart(double x) =>
            x < 0.5 ? 8 * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 4) / 2;

        public static double InQuint(double x) =>
            x * x * x * x * x;

        public static double OutQuint(double x) =>
            1 - Math.Pow(1 - x, 5);

        public static double InOutQuint(double x) =>
            x < 0.5 ? 16 * x * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 5) / 2;

        public static double InExpo(double x) =>
            x == 0 ? 0 : Math.Pow(2, 10 * x - 10);

        public static double OutExpo(double x) =>
            x == 1 ? 1 : 1 - Math.Pow(2, -10 * x);

        public static double InOutExpo(double x) =>
            x == 0 ? 0 :
            x == 1 ? 1 :
            x < 0.5
            ? Math.Pow(2, 20 * x - 10) / 2
            : (2 - Math.Pow(2, -20 * x + 10)) / 2;

        public static double InCirc(double x) =>
            1 - Math.Sqrt(1 - x * x);

        public static double OutCirc(double x) =>
            Math.Sqrt(1 - Math.Pow(x - 1, 2));

        public static double InOutCirc(double x) =>
            x < 0.5
            ? (1 - Math.Sqrt(1 - Math.Pow(2 * x, 2))) / 2
            : (Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2;

        public static double InBack(double x) =>
            C3 * x * x * x - C1 * x * x;

        public static double OutBack(double x) =>
            1 + C3 * Math.Pow(x - 1, 3) + C1 * Math.Pow(x - 1, 2);

        public static double InOutBack(double x) =>
            x < 0.5
            ? (Math.Pow(2 * x, 2) * (C2 + 1) * 2 * x - C2) / 2
            : (Math.Pow(2 * x - 2, 2) * ((C2 + 1) * (x * 2 - 2) + C2) + 2) / 2;

        public static double InElastic(double x) =>
            x == 0 ? 0 :
            x == 1 ? 1 :
            -Math.Pow(2, 10 * x - 10) * Math.Sin((x * 10 - 10.75f) * C4);

        public static double OutElastic(double x) =>
            x == 0 ? 0 :
            x == 1 ? 1 :
            Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75f) * C4) + 1;

        public static double InOutElastic(double x) =>
            x == 0 ? 0 :
            x == 1 ? 1 :
            x < 0.5
            ? -(Math.Pow(2, 20 * x - 10) * Math.Sin((20 * x - 11.125f) * C5)) / 2
            : Math.Pow(2, -20 * x + 10) * Math.Sin((20 * x - 11.125f) * C5) / 2 + 1;

        public static double InBounce(double x) =>
            1 - OutBounce(1 - x);

        public static double OutBounce(double x) =>
            x < 1 / D1 ? N1 * x * x :
            x < 2 / D1 ? N1 * (x -= 1.5f / D1) * x + 0.75f :
            x < 2.5 / D1
            ? N1 * (x -= 2.25f / D1) * x + 0.9375f
            : N1 * (x -= 2.625f / D1) * x + 0.984375f;

        public static double InOutBounce(double x) =>
            x < 0.5
            ? (1 - OutBounce(1 - 2 * x)) / 2
            : (1 + OutBounce(2 * x - 1)) / 2;
    }
}
