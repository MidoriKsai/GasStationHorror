using UnityEngine;
public class CustomerPath
{
    private readonly Transform[] _playerPathPoints;

    private int direction = 1;
    private int index;

    public CustomerPath(Transform[] pathPoints)
    {
        _playerPathPoints = pathPoints;
        index = -1;
    }

    public Vector3 GetCurrentPosition()
    {
        if (_playerPathPoints.Length == 0)
            return Vector3.zero;

        return _playerPathPoints[index].position;
    }

    public Vector3 GetNextPosition()
    {
        if (_playerPathPoints.Length == 0)
            return Vector3.zero;

        index = GetNextPointIndex();
        return _playerPathPoints[index].position;
    }

    private int GetNextPointIndex()
    {
        index += direction;

        if (index >= _playerPathPoints.Length || index < 0)
        {
            direction *= -1;
            index += direction * 2;
        }

        return index;
    }
}
