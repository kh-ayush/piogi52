using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainGame.Classes
{
    public class CBigNum
    {
        private int[] number;
        private const int _base = 1000;

        public CBigNum(string num)
        {
            num = num.TrimStart('0');
            if (num.Length == 0) num = "0";

            int length = num.Length;
            int segments = (length + 2) / 3; 
            number = new int[segments];

            for (int i = 0; i < segments; i++)
            {
                int start = Math.Max(0, length - (i + 1) * 3);
                int end = length - i * 3;
                int segmentLength = end - start;
                string segmentStr = num.Substring(start, segmentLength);
                number[segments - 1 - i] = int.Parse(segmentStr);
            }
            number = TrimLeadingZeros(number);
        }

        private CBigNum(int[] digs)
        {
            number = TrimLeadingZeros(digs);
        }

        public CBigNum Clone()
        {
            int[] clownedDigs = new int[number.Length];
            Array.Copy(number, clownedDigs, number.Length);
            return new CBigNum(clownedDigs);
        }

        public override string ToString()
        {
            if (number.Length == 0 || (number.Length == 1 && number[0] == 0))
                return "0";

            StringBuilder sb = new StringBuilder();
            sb.Append(number[0].ToString()); // Старший разряд без ведущих нулей

            for (int i = 1; i < number.Length; i++)
            {
                sb.Append(number[i].ToString("D3")); // Младшие разряды с ведущими нулями
            }

            return sb.ToString();
        }

        private CBigNum Add(CBigNum bnum)
        {
            int maxLength = Math.Max(number.Length, bnum.number.Length);
            int[] result = new int[maxLength + 1];
            int carry = 0, a = 0, b = 0;

            for (int i = 0; i < maxLength; i++)
            {
                if (i < number.Length) a = number[number.Length - 1 - i];
                else a = 0;

                if (i < bnum.number.Length) b = bnum.number[bnum.number.Length - 1 - i];
                else b = 0;

                int sum = a + b + carry;
                result[result.Length - 1 - i] = sum % _base;
                carry = sum / _base;
            }
            result[0] = carry;

            return new CBigNum(result);
        }

        private CBigNum Subtract(CBigNum bnum)
        {
            if (CompareTo(bnum) < 0) throw new InvalidOperationException("Result is negative");

            int[] result = new int[number.Length];
            int borrow = 0;

            for (int i = 0; i < number.Length; i++)
            {
                int a = number[number.Length - 1 - i];
                int b = i < bnum.number.Length ? bnum.number[bnum.number.Length - 1 - i] : 0;

                int diff = a - b - borrow;
                if (diff < 0)  {
                    diff += _base;
                    borrow = 1;
                }
                else {  borrow = 0; }

                result[result.Length - 1 - i] = diff;
            }

            return new CBigNum(result);
        }

        private CBigNum Multiply(double multiplier)
        {
            if (multiplier == 0) return new CBigNum("0");
            if (multiplier == 1) return this.Clone(); 

            int[] result = new int[number.Length + Convert.ToString(multiplier).Length];
            int carry = 0, a = 0;

            for (int i = 0; i < number.Length; i++)
            {
                a = number[number.Length - 1 - i];
                double multy = (a * multiplier) + carry;
                result[result.Length - 1 - i] = (int) multy % _base;
                carry = (int) multy / _base;
            }
            result[0] = carry;

            return new CBigNum(result);
        }

        private CBigNum Divide(double divisor)
        {
            if (divisor == 0) throw new DivideByZeroException("Division by zero");

            int[] result = new int[number.Length];
            double ost = 0;

            for (int i = 0; i < number.Length; i++)
            {
                double a = number[i] + (ost * _base);

                result[i] =  Convert.ToInt32(a / divisor);
                ost = a % divisor;
            }

            return new CBigNum(result);
        }

        private int[] TrimLeadingZeros(int[] arr)
        {
            if (arr == null || arr.Length == 0) return new int[] { 0 };

            int firstNonZero = 0;
            while (firstNonZero < arr.Length && arr[firstNonZero] == 0) { firstNonZero++; }

            if (firstNonZero == arr.Length) return new int[] { 0 };

            int[] result = new int[arr.Length - firstNonZero];
            Array.Copy(arr, firstNonZero, result, 0, result.Length);
            return result;
        }

        private int CompareTo(CBigNum bnum)
        {
            if (number.Length != bnum.number.Length)
                return number.Length.CompareTo(bnum.number.Length);

            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] != bnum.number[i])
                    return number[i].CompareTo(bnum.number[i]);
            }

            return 0;
        }

        public static CBigNum operator +(CBigNum a, CBigNum b)
        {
            return a.Add(b);
        }

        public static CBigNum operator -(CBigNum a, CBigNum b)
        {
            return a.Subtract(b);
        }

        public static CBigNum operator *(CBigNum a, double b)
        {
            return a.Multiply(b);
        }

        public static CBigNum operator /(CBigNum a, double b)
        {
            return a.Divide(b);
        }

        public static bool operator >(CBigNum a, CBigNum b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(CBigNum a, CBigNum b)
        {
            return a.CompareTo(b) < 0;
        }
        public static bool operator ==(CBigNum a, CBigNum b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(CBigNum a, CBigNum b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (obj is CBigNum bnum)
                return CompareTo(bnum) == 0;

            return false;
        }
        public override int GetHashCode()
        {
            return number.GetHashCode();
        }

        public int[] Digits => number.Clone() as int[];
        public static int NumberBase => _base;
    }
}
