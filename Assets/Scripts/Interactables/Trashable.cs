using UnityEngine;

public class Trashable : MonoBehaviour
{
    [SerializeField] private bool canBeThrownAway = true;

    public bool CanBeThrownAway => canBeThrownAway;
}