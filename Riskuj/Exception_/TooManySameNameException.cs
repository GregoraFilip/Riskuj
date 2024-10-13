using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.Exception_
{
    public class TooManySameNameException : Exception
    {
        public TooManySameNameException() { }
        public TooManySameNameException(string message)
            : base(message)
        { }

        public TooManySameNameException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
