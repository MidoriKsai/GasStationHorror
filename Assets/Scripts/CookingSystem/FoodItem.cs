using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [field: SerializeField] public FoodType Type { get; private set; }
}