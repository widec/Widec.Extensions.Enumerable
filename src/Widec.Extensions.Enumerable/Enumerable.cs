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

        public static string UnSplit(this IEnumerable<string> source, string seperator)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            ExceptionHelper.CheckArgumentNotNull(seperator, "seperator");
            StringBuilder sb = new StringBuilder();

            foreach (var item in source)
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

        public static IEnumerable<ISequencedItem<T>> Sequence<T>(this IEnumerable<T> source)
        {
            return Sequence(source, 0);
        }

        public static IEnumerable<ISequencedItem<T>> Sequence<T>(this IEnumerable<T> source, int startIndex)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            return GetEnumerable(() => new SequencedEnumerator<T>(source.GetEnumerator(), startIndex));
        }

        #endregion

        #region Median

        public static int Median<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            var orderedItems = source.Select(selector)
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

        #region ExceptWith

        public static IEnumerable<TSource> ExceptWith<TSource, TOther>(
            this IEnumerable<TSource> source,
            IEnumerable<TOther> other,
            Func<TSource, TOther> convert)
        {
            return new ClosureEnumerable<TSource>(() => new ExceptWithEnumerator<TSource, TOther>(source, other, convert));
        }

        #endregion

        #region Buffer

        public static IEnumerable<IEnumerable<T>> Buffer<T>(this IEnumerable<T> source, int size)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            ExceptionHelper.CheckArgumentPositiveNotZero(size, "size");

            var enumerator = source.GetEnumerator();
            var buffer = new T[size];
            var isFinished = false;

            while (!isFinished)
            {
                for (int i = 0; i < size; i++)
                {
                    if (enumerator.MoveNext())
                    {
                        buffer[i] = enumerator.Current;
                    }
                    else
                    { 
                        if (i != 0)
                        {
                            yield return buffer.Take(i).ToArray();
                        }
                        isFinished = true;
                        break;
                    }
                }
                if (!isFinished)
                {
                    yield return buffer.ToArray();
                }
            }
        }

        #endregion

        #region ToUpper

        public static IEnumerable<string> ToUpper<T>(this IEnumerable<T> source)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            foreach (var item in source)
            {
                yield return item.ToString().ToUpper();
            }    
        }

        public static IEnumerable<string> ToUpper(this IEnumerable<string> source)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            foreach (var item in source)
            {
                yield return item.ToUpper();
            }
        }

        public static IEnumerable<string> ToUpper<T>(this IEnumerable<T> source, Func<T,string> convert)
        {
            ExceptionHelper.CheckArgumentNotNull(source, "source");
            ExceptionHelper.CheckArgumentNotNull(convert, "convert");
            foreach (var item in source)
            {
                yield return convert(item).ToUpper();
            }
        }

        #endregion

    }
}


