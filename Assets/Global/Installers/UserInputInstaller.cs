using UnityEngine;
using Zenject;

public class UserInputInstaller : MonoInstaller
{
    [SerializeField] private UserInputRouter _inputRouter;

    public override void InstallBindings()
    {
        var instance = Container.InstantiatePrefabForComponent<UserInputRouter>(_inputRouter);
        Container.Bind<UserInputRouter>().FromInstance(instance).AsSingle().NonLazy();
    }
}