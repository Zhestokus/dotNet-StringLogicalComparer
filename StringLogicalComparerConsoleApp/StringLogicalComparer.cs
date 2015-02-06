using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringLogicalComparerConsoleApp
{
    public class StringLogicalComparer : IComparer, IEqualityComparer, IComparer<String>, IEqualityComparer<String>
    {
        public static readonly StringLogicalComparer Default;
        public static readonly StringLogicalComparer CaseInsensitive;
        public static readonly StringLogicalComparer DetectFloatNumber;
        public static readonly StringLogicalComparer IgnoreCaseAndDetectFloatNumber;

        static StringLogicalComparer()
        {
            Default = new StringLogicalComparer();
            CaseInsensitive = new StringLogicalComparer(true, false);
            DetectFloatNumber = new StringLogicalComparer(false, true);
            IgnoreCaseAndDetectFloatNumber = new StringLogicalComparer(true, true);
        }

        private struct PartEntry
        {
            public int length;
            public Object value;
        }

        private readonly StringComparer comparer;
        private readonly String decimalSeparator;

        public StringLogicalComparer()
            : this(false, false)
        {
        }

        public StringLogicalComparer(bool ignoreCase, bool floatNumbers)
        {
            IgnoreCase = ignoreCase;
            FloatNumbers = floatNumbers;

            decimalSeparator = NumberFormatInfo.InvariantInfo.NumberDecimalSeparator;

            comparer = (IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        public bool IgnoreCase { get; private set; }
        public bool FloatNumbers { get; private set; }

        public int Compare(Object x, Object y)
        {
            var xStr = Convert.ToString(x);
            var yStr = Convert.ToString(y);

            return Compare(xStr, yStr);
        }

        public int Compare(String x, String y)
        {
            int xIndex = 0;
            int yIndex = 0;

            x = (x ?? String.Empty);
            y = (y ?? String.Empty);

            while (xIndex < x.Length || yIndex < y.Length)
            {
                var xEntry = GetNextPart(x, xIndex);
                xIndex += xEntry.length;


                var yEntry = GetNextPart(y, yIndex);
                yIndex += xEntry.length;

                if (xEntry.value is double && yEntry.value is double)
                {
                    var xNumber = (double)xEntry.value;
                    var yNumber = (double)yEntry.value;

                    var order = xNumber.CompareTo(yNumber);
                    if (order != 0)
                    {
                        return order;
                    }
                }
                else
                {
                    var xText = Convert.ToString(xEntry.value);
                    var yText = Convert.ToString(yEntry.value);

                    var order = comparer.Compare(xText, yText);
                    if (order != 0)
                    {
                        return order;
                    }
                }
            }

            //int count = Math.Max(x.Length, y.Length);

            //for (int i = 0; i < count; i++)
            //{
            //    char xChar = (i < x.Length ? x[i] : '\0');
            //    char yChar = (i < y.Length ? y[i] : '\0');

            //    if (char.IsDigit(xChar) && char.IsDigit(yChar))
            //    {
            //        var xsDigits = GetAllDigits(x, i);
            //        var ysDigits = GetAllDigits(y, i);

            //        var xNumber = Convert.ToDouble(xsDigits, NumberFormatInfo.InvariantInfo);
            //        var yNumber = Convert.ToDouble(ysDigits, NumberFormatInfo.InvariantInfo);

            //        int result = xNumber.CompareTo(yNumber);
            //        if (result != 0)
            //        {
            //            return result;
            //        }

            //        i += Math.Max(xsDigits.Length, ysDigits.Length) - 1;
            //    }
            //    else
            //    {
            //        var xsNonDigits = GetAllNonDigits(x, i);
            //        var ysNonDigits = GetAllNonDigits(y, i);

            //        var result = comparer.Compare(xsNonDigits, ysNonDigits);
            //        if (result != 0)
            //        {
            //            return result;
            //        }

            //        i += Math.Max(xsNonDigits.Length, ysNonDigits.Length) - 1;
            //    }
            //}

            return 0;
        }

        private PartEntry GetNextPart(String text, int startIndex)
        {
            PartEntry entry;

            char @char = (startIndex < text.Length ? text[startIndex] : '\0');

            if (char.IsDigit(@char))
            {
                var digits = GetAllDigits(text, startIndex);

                entry.length = digits.Length;
                entry.value = Convert.ToDouble(digits, NumberFormatInfo.InvariantInfo);
            }
            else
            {
                var nonDigits = GetAllNonDigits(text, startIndex);

                entry.length = nonDigits.Length;
                entry.value = nonDigits;
            }

            return entry;
        }

        private String GetAllNonDigits(String text, int startIndex)
        {
            var result = String.Empty;
            for (var i = startIndex; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    break;
                }

                result += text[i];
            }

            return result;
        }

        private String GetAllDigits(String text, int startIndex)
        {
            var result = String.Empty;
            for (var i = startIndex; i < text.Length; i++)
            {
                if (!char.IsDigit(text[i]))
                {
                    var strChar = Convert.ToString(text[i]);
                    if (!FloatNumbers || strChar != decimalSeparator || result.Contains(strChar))
                    {
                        break;
                    }
                }

                result += text[i];
            }

            if (result.EndsWith(decimalSeparator))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        public bool Equals(Object x, Object y)
        {
            var xStr = Convert.ToString(x);
            var yStr = Convert.ToString(y);

            return Equals(xStr, yStr);
        }

        public bool Equals(String x, String y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(String str)
        {
            if (str == null)
            {
                return 0;
            }

            return comparer.GetHashCode(str);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(Convert.ToString(obj));
        }
    }

}
