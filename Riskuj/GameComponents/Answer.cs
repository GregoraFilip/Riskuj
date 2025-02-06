using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.GameComponents
{
    public class Answer
    {
        public string Text { get; }
        public bool Chosen { get; set; }
        public Answer (string text)
        {
            Text = text;
            Chosen = false;
        }
    }
}
