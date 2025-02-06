using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.Exception_
{
    public class NoTeamAddedException : Exception
    {
        public NoTeamAddedException() { }
        public NoTeamAddedException(string message) : base(message) { }
        public NoTeamAddedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
