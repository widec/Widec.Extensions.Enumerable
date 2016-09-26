using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widec.Extensions.Enumerable
{
    public class ExceptionHelper
    {
        public static void CheckArgumentPositiveNotZero(int argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName, argument, string.Format("Expected positive and non-zero integer, but was {0}", argument));
            }    
        }

        public static void CheckArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);        
            }
        }
    }
}
