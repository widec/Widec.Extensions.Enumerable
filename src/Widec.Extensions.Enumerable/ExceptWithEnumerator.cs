using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
    class ExceptWithEnumerator<TSource, TOther> : IEnumerator<TSource>
    {
        TSource m_Current;
        IEnumerable<TOther> m_Other;
        HashSet<TOther> m_OtherSet;
        IEnumerable<TSource> m_Source;
        IEnumerator<TSource> m_SourceEnumerator;
        Func<TSource, TOther> m_Convert;

        public ExceptWithEnumerator(IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther> convert)
        {
            m_Current = default(TSource);
            m_Source = source;
            m_Other = other;
            m_OtherSet = null;
            m_Convert = convert;
        }


        public TSource Current
        {
            get
            {
                return m_Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return m_Current;
            }
        }

        public void Dispose()
        {
            m_SourceEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            if (m_OtherSet == null)
            {
                m_OtherSet = new HashSet<TOther>(m_Other);
                m_SourceEnumerator = m_Source.GetEnumerator();
            }
            while(m_SourceEnumerator.MoveNext())
            {
                if (!m_OtherSet.Contains(m_Convert(m_SourceEnumerator.Current)))
                {
                    m_Current = m_SourceEnumerator.Current;
                    return true;    
                }
            }
            m_Current = default(TSource);
            return false;
        }

        public void Reset()
        {
            m_SourceEnumerator.Reset();
            m_Current = default(TSource);
        }
    }
}
