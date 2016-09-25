using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
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
}
