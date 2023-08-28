using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.AI
{
    public interface ITextAnalysisService
    {
        double AnalyseSentiment(string text);
    }
}
