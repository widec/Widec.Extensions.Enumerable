using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
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
}
