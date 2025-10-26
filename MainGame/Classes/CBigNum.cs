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
        public int ArrayLength;

        public CBigNum(string num) 
        {
            ArrayLength = (int)Math.Ceiling((decimal)(num.Length / 3));
            number = new int[ArrayLength];
            for (int i = 0, j = 0; i < num.Length - 2; i += 3, j++) {
                number[j] = Convert.ToInt32($"{num[i]}{num[i + 1]}{num[i + 2]}");
            }
        }
        public override string ToString()
        {
            string s = "";
            foreach (int i in number) s += Convert.ToString(i);
            return s;
        }
        public CBigNum Add(CBigNum value) 
        {
            int maxAL;
            if (value.ArrayLength > this.ArrayLength) maxAL = value.ArrayLength;
            else maxAL = this.ArrayLength;
            int[] sum = new int[maxAL + 1];
            for (int i = maxAL - 1; i >= 0; i--)
            {
                sum[i] += this.number[i] + value.number[i];
            }
            string s = "";
            foreach (int i in sum) s += Convert.ToString(i);
            return new CBigNum(s);
        }

    }
}
