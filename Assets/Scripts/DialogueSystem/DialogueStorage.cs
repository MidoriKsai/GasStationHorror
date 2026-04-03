using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DialogueSystem;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueStorage : IDialogueStorage
    {
        private readonly Dictionary<string, DialogueData> _cache = new();
        private readonly IDialogueParser _parser;

        public DialogueStorage(IDialogueParser parser)
        {
            this._parser = parser;
        }

        public UniTask<DialogueData> GetDialogueAsync(string id, CancellationToken ct)
        {
            if (_cache.TryGetValue(id, out var cached))
                return UniTask.FromResult(cached);

            var file = Resources.Load<TextAsset>($"Dialogues/{id}");

            if (file == null)
                throw new System.Exception($"Dialogue {id} not found");

            var parsed = _parser.Parse(file.text);

            _cache[id] = parsed;

            return UniTask.FromResult(parsed);
        }
    }
}