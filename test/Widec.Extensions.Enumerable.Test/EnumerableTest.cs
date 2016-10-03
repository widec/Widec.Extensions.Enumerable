//Copyright (c) 2014 Wim De Cleen

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Widec.Extensions.Enumerable;
using System;

namespace Widec.Extensions.Enumerable.Test
{
    public class EnumerableTest
    {
        #region Crudonize

        [Theory()]
        [InlineData("A", "", "A", "", "")]
        [InlineData("A", "A", "", "A", "")]
        [InlineData("", "A", "", "", "A")]
        [InlineData("A,B", "B", "A", "B", "")]
        [InlineData("A,B", "B,C", "A", "B", "C")]
        [InlineData("A,B", "A,B", "", "A,B", "")]
        [InlineData("", "A,B", "", "", "A,B")]
        public void Crudonize(string master, string slave, string expectedCreates, string expectedUpdates, string expectedDeletes)
        {
            List<string> creates = new List<string>();
            List<string> updates = new List<string>();
            List<string> deletes = new List<string>();

            master.Split(',').Crudonize(
                slave.Split(','),
                (m, s) => m == s,
                (m) => creates.Add(m),
                (m, s) => updates.Add(m),
                (s) => deletes.Add(s));

            Assert.Equal(expectedCreates, creates.OrderBy(s => s).UnSplit(","));
            Assert.Equal(expectedUpdates, updates.OrderBy(s => s).UnSplit(","));
            Assert.Equal(expectedDeletes, deletes.OrderBy(s => s).UnSplit(","));
        }

        [Theory()]
        [InlineData("A,B")]
        [InlineData("A")]
        [InlineData("")]
        [InlineData("A,B,C")]
        public void UnSplit(string expected)
        {
            Assert.Equal(expected, expected.Split(',').UnSplit(","));
        }

        [Theory()]
        [InlineData("A,B,C,D", 1, "A1,B2,C3,D4")]
        [InlineData("A", 1, "A1")]
        [InlineData("A,B,C,D", 3, "A3,B4,C5,D6")]
        public void Sequence(string template, int startSequence, string expected)
        {
            Assert.Equal(expected, template.Split(',').Sequence(startSequence).Select(si => string.Format("{0}{1}", si.Item, si.Sequence)).UnSplit(","));
        }

        #endregion

        #region Median

        [Fact()]
        public void MedianTest_EvenNumberOfItemsSelector()
        {
            var list = new string[] { "1", "4", "5", "8" };
            Assert.Equal(4, list.Median(n => int.Parse(n)));
        }

        [Fact()]
        public void MedianTest_EvenNumberOfItems()
        {
            var list = new int[] { 1, 4, 5, 8 };
            Assert.Equal(4, list.Median(n=>n));
        }

        [Fact()]
        public void MedianTest_OddNumberOfItems()
        {
            var list = new int[] { 1, 4, 5, 8, 9 };
            Assert.Equal(5, list.Median(n => n));
        }

        [Fact()]
        public void MedianTest_EvenNumberOfItems_Order()
        {
            var list = new int[] { 1, 8, 5, 4 };
            Assert.Equal(4, list.Median(n => n));
        }

        [Fact()]
        public void MedianTest_OddNumberOfItems_Order()
        {
            var list = new int[] { 4, 9, 8, 5, 1 };
            Assert.Equal(5, list.Median(n => n));
        }

        [Fact()]
        public void MedianTest_SingleItem()
        {
            var list = new int[] { 1 };
            Assert.Equal(1, list.Median(n => n));
        }

        [Fact()]
        public void MedianTest_EmptyList()
        {
            var list = new int[] {};
            Assert.Equal(0, list.Median(n => n));
        }

        [Fact()]
        public void MedianTest_TwoItemList()
        {
            var list = new int[] { 1, 3 };
            Assert.Equal(2, list.Median(n => n));
        }

        #endregion

        #region Padding

        [Fact()]
        public void PadRight_EmptyItems()
        {
            var list = new int[] { 0 };
            var padList = list.PadRight(3, i => i).ToArray();
            Assert.Equal(0, padList[0]);
            Assert.Equal(1, padList[1]);
            Assert.Equal(2, padList[2]);
        }

        [Fact()]
        public void PadRight_EmptySource()
        {
            var list = new int[] { };
            var padList = list.PadRight(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadRight_LengthGreaterThanNumber()
        {
            var list = new int[] { 0 };
            var padList = list.PadRight(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadRight_LengthEqualToNumber()
        {
            var list = new int[] { 0, 1, 2 };
            var padList = list.PadRight(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadRight_LengthSmallerThanNumber()
        {
            var list = new int[] { 0, 1, 2 };
            var padList = list.PadRight(2, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadLeft_EmptyItems()
        {
            var list = new int[] { 2 };
            var padList = list.PadLeft(3, i => i).ToArray();
            Assert.Equal(0, padList[0]);
            Assert.Equal(1, padList[1]);
            Assert.Equal(2, padList[2]);
        }

        [Fact()]
        public void PadLeft_EmptySource()
        {
            var list = new int[] { };
            var padList = list.PadLeft(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadLeft_LengthGreaterThanNumber()
        {
            var list = new int[] { 0 };
            var padList = list.PadLeft(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadLeft_LengthEqualToNumber()
        {
            var list = new int[] { 0, 1, 2 };
            var padList = list.PadLeft(3, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        [Fact()]
        public void PadLeft_LengthSmallerThanNumber()
        {
            var list = new int[] { 0, 1, 2 };
            var padList = list.PadLeft(2, i => i).ToArray();
            Assert.Equal(3, padList.Length);
        }

        #endregion

        #region ExceptWith
        [Theory()]
        [InlineData("A,B,C,D", "B", "A,C,D")]
        [InlineData("A,B,C,D", "A,B,C,D", "")]
        [InlineData("A,B,C,D", "C", "A,B,D")]
        [InlineData("A,B,C,D", "D", "A,B,C")]
        [InlineData("A,B,C,D", "A", "B,C,D")]
        [InlineData("", "B", "")]
        [InlineData("A,B,C,D", "X", "A,B,C,D")]
        [InlineData("A,B,C,D", "A,B", "C,D")]
        [InlineData("A,B,C,D", "", "A,B,C,D")]
        public void ExceptWith(string source, string other, string expected)
        {
            var result = source.Split(',').ExceptWith(other.Split(','), a => a).OrderBy(a => a).UnSplit(",");
            Assert.Equal(expected, result);
        }

        #endregion

        #region Buffer

        [Theory()]
        [InlineData("A,B,C,D", 1, "A;B;C;D")]
        [InlineData("A,B,C,D", 2, "A,B;C,D")]
        [InlineData("A,B,C,D", 3, "A,B,C;D")]
        [InlineData("A,B,C,D", 4, "A,B,C,D")]
        [InlineData("A,B,C,D", 5, "A,B,C,D")]
        public void Buffer(string source, int size, string expected)
        {
            var result = source.Split(',').Buffer(size).Select(buffer => buffer.UnSplit(",")).UnSplit(";");
            Assert.Equal(expected, result);
        }

        [Fact()]
        public void Buffer_Size_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>("size", () => new int[] { 1, 2, 3 }.Buffer(0).ToArray());
        }

        [Fact()]
        public void Buffer_Size_Negative()
        {
            Assert.Throws<ArgumentOutOfRangeException>("size", () => new int[] { 1, 2, 3 }.Buffer(-1).ToArray());
        }

        [Fact()]
        public void Buffer_Source_Null()
        {
            Assert.Throws<ArgumentNullException>("source", () => Enumerable.Buffer((IEnumerable<int>)null, 15).ToArray());
        }

        #endregion

        #region Unsplit

        [Theory()]
        [InlineData("A,B,C","A,B,C")]
        [InlineData("A", "A")]
        [InlineData("", "")]
        public void Unsplit(string source, string expected)
        {
            var result = source.Split(',').UnSplit(",");
            Assert.Equal(expected, result);
        }

        [Fact()]
        public void Unsplit_Source_Null()
        {
            Assert.Throws<ArgumentNullException>("source", () => Enumerable.UnSplit((IEnumerable<string>)null, ",").ToArray());
        }

        [Fact()]
        public void Unsplit_Seperator_Null()
        {
            Assert.Throws<ArgumentNullException>("seperator", () => Enumerable.UnSplit(new string[] { "A", "B" }, null).ToArray());
        }

        #endregion

        #region Sequence

        [Theory()]
        [InlineData("A,B,C","0-A,1-B,2-C")]
        [InlineData("A", "0-A")]
        [InlineData("", "")]
        public void Sequence(string source, string expected)
        {
            var result = source.Split(',').Where(s => !string.IsNullOrEmpty(s)).Sequence().Select(si => string.Format("{0}-{1}", si.Sequence, si.Item)).UnSplit(",");
            Assert.Equal(expected, result);  
        }

        [Theory()]
        [InlineData("A,B,C", 0, "0-A,1-B,2-C")]
        [InlineData("A,B,C", -5, "-5-A,-4-B,-3-C")]
        [InlineData("A,B,C", 2, "2-A,3-B,4-C")]
        [InlineData("",10, "")]
        public void Sequence_Offset(string source, int startIndex, string expected)
        {
            var result = source.Split(',').Where(s => !string.IsNullOrEmpty(s)).Sequence(startIndex).Select(si => string.Format("{0}-{1}", si.Sequence, si.Item)).UnSplit(",");
            Assert.Equal(expected, result);
        }

        [Fact()]
        public void Sequence_Source_Null()
        {
            Assert.Throws<ArgumentNullException>("source", () => Enumerable.Sequence((IEnumerable<string>)null).ToArray());
        }

        #endregion
    }
}