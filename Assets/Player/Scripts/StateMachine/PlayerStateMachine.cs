using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerStateMachine
{
    private const float _fPSLock = 0.01f;

    private readonly IState _base;
    private readonly IState _movement;
    private readonly WaitForSeconds _delay = new WaitForSeconds(_fPSLock);

    [Inject] private UserInputRouter _inputRouter;

    private IState _current;

    public PlayerStateMachine(IState baseState, IState movement)
    {
        _base = baseState;
        _current = baseState;
        _movement = movement;
    }

    public IEnumerator Exist()
    {
        yield return null;

        while (true)
        {
            _current.Exist(_fPSLock);
            yield return _delay;
        }
    }

    public void Start()
    {
        _inputRouter.StartedMoving += EnterMovement;
        _inputRouter.Stopped += EnterBaseState;
        _current.Enter();
    }

    public void Stop()
    {
        _inputRouter.StartedMoving -= EnterMovement;
        _inputRouter.Stopped -= EnterBaseState;
        EnterBaseState();
        _current.Exit();
    }

    private void EnterState(IState state)
    {
        _current.Exit();
        _current = state;
        _current.Enter();
    }

    private void EnterBaseState()
    {
        EnterState(_base);
    }

    private void EnterMovement()
    {
        EnterState(_movement);
    }
}