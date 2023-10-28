using UnityEngine;
using Mirror;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Unit : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private UnitSkin _skin;

    [Inject] private DiContainer _container;
    [Inject] private UserInputRouter _inputRouter;

    private Rigidbody _rigidBody;
    private Coroutine _exist; 
    private UnitStateMachine _machine;

    public override void OnStopAuthority()
    {
        StopPlaySystem();
    }

    public override void OnStartLocalPlayer()
    {
        LoadLocalData();
        StartPlaySystem();
    }

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody tempRigidBody))
        {
            _rigidBody = tempRigidBody;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void StopPlaySystem()
    {
        _machine.Stop();
        _skin.Deactivate();
        _rigidBody.isKinematic = true;

        if (_exist != null)
            StopCoroutine(_exist);
    }

    private void LoadLocalData()
    {
        UnitMovement movement = new UnitMovement(_rigidBody, _speed, _animator);
        UnitRelax relax = new UnitRelax(_animator, 3f, 2);
        _machine = new UnitStateMachine(relax, movement);
        _container.Inject(movement);
        _container.Inject(relax);
        _container.Inject(_machine);
    }

    private void StartPlaySystem()
    {
        _skin.Activate();
        _rigidBody.isKinematic = true;
        _inputRouter.StartSearchNewDirection();
        _machine.Start();
        _exist = StartCoroutine(_machine.Exist());
    }
}