using System.Threading;
using Cysharp.Threading.Tasks;
using DialogueSystem;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueStorage
    {
        UniTask<DialogueData> GetDialogueAsync(string id, CancellationToken ct);
    }
}