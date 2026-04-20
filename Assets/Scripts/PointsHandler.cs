using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [field: SerializeField] public Transform CarSpawnPoint;
    [field: SerializeField] public List<Transform> GasStationPoints;
    [field: SerializeField] public Transform CashDeskPoint;
    [field: SerializeField] public Transform CarLeavePoint;
    
    [field: SerializeField] public List<Transform> CustomerSpawnPoints { get; private set; }
    [field: SerializeField] public List<Transform> ScannedProductPoints { get; private set; }
}