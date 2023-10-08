using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerMovement : IState
{
    private const float _rotateTime = 1f;

    private readonly Rigidbody _rigidBody;
    private readonly Animator _animator;
    private readonly int _animationHash;

    [Inject] private UserInputRouter _inputRouter;

    private float _speed;

    public PlayerMovement(Rigidbody rigidBody, float speed, Animator animator)
    {
        _rigidBody = rigidBody;
        _speed = speed;
        _animator = animator;
        _animationHash = Animator.StringToHash("Move");
    }

    public void Enter()
    {
        _animator.SetBool(_animationHash, true);
        _inputRouter.ChangedDirection += Rotate;
    }

    public void Exist(float deltaTime)
    {
        _rigidBody.DOMove(CalculateTarget(deltaTime), deltaTime);
    }

    public void Exit()
    {
        _animator.SetBool(_animationHash, false);
        _inputRouter.ChangedDirection -= Rotate;
    }

    private void Rotate()
    {
        _rigidBody.transform.DOLookAt(CalculateTarget(), _rotateTime);
    }

    private Vector3 CalculateTarget(float deltaTime = 1)
    {
        return _rigidBody.transform.position + (_inputRouter.Direction * _speed * deltaTime);
    }
}
