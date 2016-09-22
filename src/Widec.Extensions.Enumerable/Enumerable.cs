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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
    public static class Enumerable
    {
        #region SequencedItem

        class SequencedItem<T> : ISequencedItem<T>
        {
            public SequencedItem(T item, int sequence)
            {
                Item = item;
                Sequence = sequence;
            }

            public T Item { get; private set; }
            public int Sequence { get; private set; }
        }

        #endregion

        #region ClosureEnumerable

        class ClosureEnumerable<T> : IEnumerable<T>
        {
            Func<IEnumerator<T>> m_GetEnumerator;

            public ClosureEnumerable(Func<IEnumerator<T>> getEnumerator)
            {
                m_GetEnumerator = getEnumerator;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return m_GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return m_GetEnumerator();
            }
        }

        #endregion

        #region SequencedEnumerator

        class SequencedEnumerator<T> : IEnumerator<ISequencedItem<T>>
        {
            IEnumerator<T> m_Original;
            ISequencedItem<T> m_Current;
            int m_Sequence;
            int m_StartSequence;

            public SequencedEnumerator(IEnumerator<T> original, int startSequence)
            {
                m_Original = original;
                m_StartSequence = startSequence;
                m_Sequence = startSequence;
                m_Current = null;
            }

            public ISequencedItem<T> Current
            {
                get
                {
                    return m_Current;
                }
            }

            public void Dispose()
            {
                m_Original.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return m_Current; }
            }

            public bool MoveNext()
            {
                var result = m_Original.MoveNext();
                if (result)
                {
                    m_Current = new SequencedItem<T>(m_Original.Current, m_Sequence);
                    m_Sequence++;
                }
                return result;
            }

            public void Reset()
            {
                m_Original.Reset();
                m_Sequence = m_StartSequence;
                m_Current = null;
            }
        }

        #endregion

        #region Support

        static IEnumerable<T> GetEnumerable<T>(Func<IEnumerator<T>> getEnumerator)
        {
            return new ClosureEnumerable<T>(getEnumerator);
        }

        #endregion

        #region Crudonize

        /// <summary>
        /// Crudonize the difference between 2 enumerables
        /// </summary>
        /// <typeparam name="TMaster">The type of the master enumerable</typeparam>
        /// <typeparam name="TSlave">The type of the slave enumerable</typeparam>
        /// <param name="masterList">The master enumerable</param>
        /// <param name="slaveList">The slave enumerable</param>
        /// <param name="compare">The compare delegate</param>
        /// <param name="create">The create delegate</param>
        /// <param name="update">The update delegate</param>
        /// <param name="delete">The delete delegate</param>
        public static void Crudonize<TMaster, TSlave>(
            this IEnumerable<TMaster> masterList,
            IEnumerable<TSlave> slaveList,
            Func<TMaster, TSlave, bool> compare,
            Action<TMaster> create,
            Action<TMaster, TSlave> update,
            Action<TSlave> delete)
        {
            var master = masterList.ToArray();
            var slave = slaveList.ToList();

            var creates = new List<TMaster>();
            var updates = new List<Tuple<TMaster, TSlave>>();

            for (int masterCounter = master.Length - 1; masterCounter >= 0; masterCounter--)
            {
                var inSlaveList = false;
                for (int slaveCounter = slave.Count - 1; slaveCounter >= 0; slaveCounter--)
                {
                    if (compare(master[masterCounter], slave[slaveCounter]))
                    {
                        // Item is in both lists, add the items to the update list.
                        updates.Add(Tuple.Create(master[masterCounter], slave[slaveCounter]));

                        // Remove the slave item because it is already found.
                        slave.RemoveAt(slaveCounter);

                        inSlaveList = true;
                        break;
                    }
                }
                if (!inSlaveList)
                {
                    // Item not in slavelist so add to Create actions
                    creates.Add(master[masterCounter]);
                }
            }

            slave.ForEach(s => delete(s));
            updates.ForEach(ms => update(ms.Item1, ms.Item2));
            creates.ForEach(m => create(m));
        }

        #endregion

        #region UnSplit

        public static string UnSplit(this IEnumerable<string> items, string seperator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in items)
            {
                if (sb.Length == 0)
                {
                    sb.Append(item);
                }
                else
                {
                    sb.AppendFormat("{0}{1}", seperator, item);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Sequence

        public static IEnumerable<ISequencedItem<T>> Sequence<T>(this IEnumerable<T> items)
        {
            return Sequence(items, 0);
        }

        public static IEnumerable<ISequencedItem<T>> Sequence<T>(this IEnumerable<T> items, int startIndex)
        {
            return GetEnumerable(() => new SequencedEnumerator<T>(items.GetEnumerator(), startIndex));
        }

        #endregion

        #region Median

        public static int Median<T>(this IEnumerable<T> list, Func<T, int> selector)
        {
            var orderedItems = list.Select(selector)
                .OrderBy(n => n)
                .ToArray();

            if (orderedItems.Length == 0)
            {
                return 0;
            }
            if (orderedItems.Length == 1)
            {
                return orderedItems[0];
            }

            int center = orderedItems.Length / 2;

            if (orderedItems.Length % 2 == 0)
            {
                return (orderedItems[center - 1] + orderedItems[center]) / 2;
            }
            else
            {
                return orderedItems[center];
            }
        }

        #endregion

        #region Padding

        public static IEnumerable<TSource> PadRight<TSource>(this IEnumerable<TSource> source, int totalWidth, Func<int, TSource> paddingItem)
        {
            int counter = 0;
            foreach (var item in source)
            {
                yield return item;
                counter++;
            }
            for (int i = counter; i < totalWidth; i++)
            {
                yield return paddingItem(i);
            }
        }

        public static IEnumerable<TSource> PadLeft<TSource>(this IEnumerable<TSource> source, int totalWidth, Func<int, TSource> paddingItem)
        {
            var list = source.ToArray();
            for (int i = 0; i < totalWidth - list.Length; i++)
            {
                yield return paddingItem(i);
            }
            foreach (var item in list)
            {
                yield return item;
            }
        }

        #endregion
    }
}


