using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        ReferenceBinding();
        NetworkBinding();
    }

    private void ReferenceBinding()
    {
        Container.Bind<MainContainerReference>().FromNew().AsSingle().NonLazy();
    }

    private void NetworkBinding()
    {
        Container.Bind<LocalPlayerRouter>().FromNew().AsSingle().NonLazy();
        Container.Bind<MatchConnector>().FromNew().AsSingle().NonLazy();
    }
}