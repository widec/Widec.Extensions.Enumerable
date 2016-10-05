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
