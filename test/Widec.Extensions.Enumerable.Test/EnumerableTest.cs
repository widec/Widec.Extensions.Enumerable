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
    }
}