using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
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
}
