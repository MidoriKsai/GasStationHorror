using DialogueSystem;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueParser
    {
        DialogueData Parse(string xml);
    }
}