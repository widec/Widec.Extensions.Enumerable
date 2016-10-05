//Copyright (c) 2016 Wim De Cleen

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
