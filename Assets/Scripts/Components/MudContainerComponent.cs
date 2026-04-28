using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MudContainerComponent : MonoBehaviour
{
    [SerializeField] private List<Mud> mudContainer = new();
    public int mudCountInContainer => mudContainer.Count;

    public void Start()
    {
        foreach (Mud mud in mudContainer)
        {
            mud.WashedAction += MudWashedEvent;
        }
    }

    private void MudWashedEvent(Mud washedMud)
    {
        mudContainer.Remove(washedMud);
        washedMud.DestroyMud();
    }
}
