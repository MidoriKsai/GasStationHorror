using UnityEngine;
public class CustomerPath
{
    private readonly Transform[] _pathPoints;
    private int _index;
    private int _direction;
    private bool _isCompleted;

    public CustomerPath(Transform[] pathPoints)
    {
        _pathPoints = pathPoints;
        Reset();
    }

    public void Reset()
    {
        _index = -1;
        _direction = 1;
        _isCompleted = _pathPoints == null || _pathPoints.Length == 0;
    }

    public bool HasNext()
    {
        if (_isCompleted || _pathPoints == null || _pathPoints.Length == 0)
            return false;

        if (_pathPoints.Length == 1)
        {
            _isCompleted = _index >= 0;
            return !_isCompleted;
        }

        return true;
    }

    public Vector3 GetNextPosition()
    {
        if (!HasNext())
            return Vector3.zero;

        if (_index == -1)
        {
            _index = 0;
            return _pathPoints[_index].position;
        }

        int nextIndex = _index + _direction;

        if (nextIndex >= _pathPoints.Length)
        {
            _direction = -1;
            nextIndex = _index + _direction;
        }
        else if (nextIndex < 0)
        {
            _isCompleted = true;
            return Vector3.zero;
        }

        _index = nextIndex;

        if (_index == 0 && _direction == -1)
            _isCompleted = true;

        return _pathPoints[_index].position;
    }
}
