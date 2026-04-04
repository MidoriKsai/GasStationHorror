using DialogueSystem;
using DialogueSystem.Interfaces;
using Services.Interfaces;
using UnityEngine;

namespace DialogueSystem
{
    public class Parser : IDialogueParser
    {
        private readonly IXMLParserService xmlParser;

        public Parser(IXMLParserService xmlParser)
        {
            this.xmlParser = xmlParser;
        }

        public DialogueData Parse(string xml)
        {
            var document = xmlParser.Parse(xml);
            var root = document.Element("dialogue");

            var dialogue = new DialogueData
            {
                DialogueId = root?.Attribute("id")?.Value
            };

            if (root == null)
                return dialogue;

            foreach (var lineElement in root.Elements("line"))
            {
                var line = new DialogueLineData
                {
                    Text = lineElement.Element("text")?.Value
                };

                var answers = lineElement.Element("answers");

                if (answers != null)
                {
                    foreach (var answer in answers.Elements("answer"))
                    {
                        line.Answers.Add(new DialogueAnswerData
                        {
                            Text = answer.Value
                        });
                    }
                }

                dialogue.Lines.Add(line);
            }
            
            Debug.Log(dialogue);
            
            if (dialogue.Lines.Count == 0)
                Debug.Log("dialogue not found");

            return dialogue;
        }
    }
}