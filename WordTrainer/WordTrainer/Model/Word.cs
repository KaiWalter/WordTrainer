using System;
using System.Collections.Generic;
using System.Text;

namespace WordTrainer.Model
{
    public class Word
    {
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        public string Group { get; set; }

        public string Get(bool fromPrimary) => fromPrimary ? PrimaryLanguage : SecondaryLanguage;
    }
}
