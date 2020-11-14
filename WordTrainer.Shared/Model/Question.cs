using System;
using System.Collections.Generic;
using System.Text;

namespace WordTrainer
{
    public class Question
    {
        public string Word { get; set; }
        public string Answer { get; set; }

        public List<string> Options { get; set; }
    }
}
