using System;
using System.Collections;
using UnityEngine;

public class UserInputRouter : MonoBehaviour 
{
    private UserInputMap _map;
    private Coroutine _searchNewMoveDirection;
    private bool _isMoveLastTime;

    public event Action StartedMoving;
    public event Action Stopped;
    public event Action ChangedDirection;
    public Vector3 Direction { get; private set; }

    public void StartSearchNewDirection()
    {
        StopSearchNewDirection();
        _searchNewMoveDirection = StartCoroutine(SearchingNewMoveDirection());
    }

    public void StopSearchNewDirection()
    {
        if (_searchNewMoveDirection != null)
        {
            StopCoroutine(SearchingNewMoveDirection());
            _searchNewMoveDirection = null;
        }
    }

    private void Awake()
    {
        _map = new UserInputMap();
    }

    private void OnEnable()
    {
        _map.Enable();
    }

    private void OnDisable()
    {
        _map.Disable();
        StopSearchNewDirection();
    }

    private IEnumerator SearchingNewMoveDirection()
    {
        yield return null;

        while (true)
        {
            Vector2 direction = _map.Movement.XZMovement.ReadValue<Vector2>();

            ProcessInputDirection(direction);

            yield return null;
        }
    }

    private void ProcessInputDirection(Vector2 direction)
    {

        if (direction.sqrMagnitude > 0.08)
        {
            if (direction.x != Direction.x || direction.y != Direction.z)
            {
                Direction = new Vector3(direction.x, 0, direction.y).normalized;

                if (_isMoveLastTime == false)
                {
                    StartedMoving?.Invoke();
                    _isMoveLastTime = true;
                }

                ChangedDirection?.Invoke();
            }
        }
        else if(_isMoveLastTime)
        {
            Direction = Vector3.zero;
            Stopped?.Invoke();
            _isMoveLastTime = false;
        }
    }
}