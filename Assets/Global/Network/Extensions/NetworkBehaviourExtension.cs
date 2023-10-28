using Mirror;

public static class NetworkBehaviourExtension
{
    public static void UpdateInjections<TypeOfNetwork>(this TypeOfNetwork networkBehaviour) where TypeOfNetwork : NetworkBehaviour
    {
        MainContainerReference.Instance.Container.Inject(networkBehaviour);
    }
}