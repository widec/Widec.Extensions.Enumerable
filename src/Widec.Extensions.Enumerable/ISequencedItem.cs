using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
    public interface ISequencedItem<T>
    {
        T Item { get; }
        int Sequence { get; }
    }
}
