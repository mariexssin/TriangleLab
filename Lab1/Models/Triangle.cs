using System;

namespace TriangleLab.Models
{
    public class Triangle
    {
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }

        public bool IsValid()
        {
            return Side1 > 0 && Side2 > 0 && Side3 > 0 &&
                   (Side1 + Side2 > Side3) &&
                   (Side1 + Side3 > Side2) &&
                   (Side2 + Side3 > Side1);
        }

        public double GetPerimeter()
        {
            return Side1 + Side2 + Side3;
        }

        public double GetArea()
        {
            double p = GetPerimeter() / 2.0;
            return Math.Sqrt(p * (p - Side1) * (p - Side2) * (p - Side3));
        }

        public bool IsRightAngled()
        {
            double[] sides = { Side1, Side2, Side3 };
            Array.Sort(sides);
            return AreEqual(Math.Pow(sides[0], 2) + Math.Pow(sides[1], 2), Math.Pow(sides[2], 2));
        }

        public bool IsEquilateral()
        {
            return AreEqual(Side1, Side2) && AreEqual(Side2, Side3);
        }

        public bool IsIsosceles()
        {
            return AreEqual(Side1, Side2) || AreEqual(Side1, Side3) || AreEqual(Side2, Side3);
        }

        public static bool AreEqual(double a, double b)
        {
            if (a == b) return true;
            double diff = Math.Abs(a - b);
            double tolerance = Math.Abs(a) * 0.00001; // 0.001% = 0.00001
            return diff < tolerance;
        }

        public bool IsCongruentTo(Triangle other)
        {
            double[] s1 = { Side1, Side2, Side3 };
            double[] s2 = { other.Side1, other.Side2, other.Side3 };
            Array.Sort(s1);
            Array.Sort(s2);

            return AreEqual(s1[0], s2[0]) && AreEqual(s1[1], s2[1]) && AreEqual(s1[2], s2[2]);
        }

        public bool IsSimilarTo(Triangle other)
        {
            double[] s1 = { Side1, Side2, Side3 };
            double[] s2 = { other.Side1, other.Side2, other.Side3 };
            Array.Sort(s1);
            Array.Sort(s2);

            double ratio1 = s1[0] / s2[0];
            double ratio2 = s1[1] / s2[1];
            double ratio3 = s1[2] / s2[2];

            return AreEqual(ratio1, ratio2) && AreEqual(ratio2, ratio3);
        }
    }
}