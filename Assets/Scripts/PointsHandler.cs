using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [field: SerializeField] public Transform CarSpawnPoint;
    [field: SerializeField] public List<Transform> GasStationPoints;
    [field: SerializeField] public Transform CashDeskPoint;
    [field: SerializeField] public Transform CarLeavePoint;
    [field: SerializeField] public Transform CoffePoint;
    [field: SerializeField] public Transform SausagePoint;
    
    [field: SerializeField] public List<Transform> GrillPoints { get; private set; }
    [field: SerializeField] public List<Transform> ProductsSpawnPoints { get; private set; }
    [field: SerializeField] public List<Transform> ScannedProductPoints { get; private set; }
}