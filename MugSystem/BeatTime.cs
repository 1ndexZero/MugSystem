using System;
using System.Text.RegularExpressions;

namespace MugSystem
{
    public struct BeatTime
    {
        public int I { get; set; }
        public int N { get; set; }
        public int D { get; set; }

        public BeatTime(int i)
        {
            I = i;
            N = 0;
            D = 1;
        }

        public BeatTime(int i, int n, int d)
        {
            if (d == 0)
                throw new ArgumentException("分母不能为 0");

            I = i;
            N = n;
            D = d;
        }

        public BeatTime(double t, int d)
        {
            if (d == 0)
                throw new ArgumentException("分母不能为 0");

            I = (int)t;
            N = (int)((t - I) * d);
            D = d;
        }

        public BeatTime(string exp)
        {
            Regex regex = new Regex(@"^([+-]?\d+)\+([+-]?\d+)/([+-]?\d+)$");
            Match match = regex.Match(exp);

            if (!match.Success)
                throw new ArgumentException("表达式格式错误");

            string iStr = match.Groups[1].Value;
            string nStr = match.Groups[2].Value;
            string dStr = match.Groups[3].Value;

            if (!int.TryParse(iStr, out int i))
                throw new ArgumentException("表达式中 I 不是个整数");
            if (!int.TryParse(nStr, out int n))
                throw new ArgumentException("表达式中 N 不是个整数");
            if (!int.TryParse(dStr, out int d))
                throw new ArgumentException("表达式中 D 不是个整数");
            if (d == 0)
                throw new ArgumentException("分母不能为 0");

            I = i;
            N = n;
            D = d;
        }

        public static bool operator ==(BeatTime a, BeatTime b) =>
            a.I == b.I && a.N * b.D == a.D * b.N;

        public static bool operator !=(BeatTime a, BeatTime b) =>
            a.I != b.I || a.N * b.D != a.D * b.N;

        public static bool operator >(BeatTime a, BeatTime b) =>
            (double)a > (double)b;

        public static bool operator <(BeatTime a, BeatTime b) =>
            (double)a < (double)b;

        public static bool operator >=(BeatTime a, BeatTime b) =>
            (double)a >= (double)b;

        public static bool operator <=(BeatTime a, BeatTime b) =>
            (double)a <= (double)b;

        public static double operator +(BeatTime a, BeatTime b) =>
            (double)a + (double)b;

        public static double operator -(BeatTime a, BeatTime b) =>
            (double)a - (double)b;

        public static explicit operator double(BeatTime t) =>
            t.I + (double)t.N / t.D;

        public double ToMs(double bpm) =>
            (I + (double)N / D) * 60000.0 / bpm;

        public override string ToString() =>
            $"{I}+{N}/{D}";

        public override bool Equals(object obj)
        {
            if (obj is BeatTime other)
                return this == other;
            return false;
        }

        public override int GetHashCode() =>
             (I, N, D).GetHashCode();
    }
}
