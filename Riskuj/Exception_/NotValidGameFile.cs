using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.Exception_
{
    public class NotValidGameFile : Exception
    {
        public NotValidGameFile() { }
        public NotValidGameFile(string message)
    : base(message)
        { }
        public NotValidGameFile(string message, Exception innerException)
    : base(message, innerException)
        { }
    }
}
