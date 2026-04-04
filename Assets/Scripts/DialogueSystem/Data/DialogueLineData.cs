using System.Collections.Generic;

namespace DialogueSystem
{
    public class DialogueLineData
    {
        public string Text;
        public List<DialogueAnswerData> Answers = new();
    }
}