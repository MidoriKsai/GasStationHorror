using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace TipsSystem
{
    public class TipStorage
    {
        private readonly Dictionary<string, TipData> _tips = new();

        public TipStorage(TextAsset tipsXml)
        {
            Parse(tipsXml);
        }

        private void Parse(TextAsset tipsXml)
        {
            if (tipsXml == null)
            {
                Debug.LogError("Tips XML is not assigned");
                return;
            }

            var document = XDocument.Parse(tipsXml.text);
            var root = document.Element("tips");

            if (root == null)
            {
                Debug.LogError("Root <tips> not found");
                return;
            }

            foreach (var tipElement in root.Elements("tip"))
            {
                string id = tipElement.Attribute("id")?.Value;
                string text = tipElement.Element("text")?.Value;

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(text))
                    continue;

                _tips[id] = new TipData
                {
                    Id = id,
                    Text = text.Trim()
                };
            }
        }

        public bool TryGetTip(string id, out TipData tip)
        {
            return _tips.TryGetValue(id, out tip);
        }
    }
}