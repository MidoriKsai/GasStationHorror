using UnityEngine;

public class CarPath
{
    private readonly Transform[] _pathPoints;
    private int _index;

    public CarPath(Transform[] pathPoints)
    {
        _pathPoints = pathPoints;
        Reset();
    }

    public void Reset()
    {
        _index = 0;
    }

    public bool HasNext()
    {
        return _pathPoints != null && _index < _pathPoints.Length;
    }

    public Vector3 GetNextPosition()
    {
        if (!HasNext())
            return Vector3.zero;

        Vector3 position = _pathPoints[_index].position;
        _index++;
        return position;
    }
}