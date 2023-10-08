using Zenject;

public class MainContainerReference
{
    public static MainContainerReference Instance { get; private set; }

    [Inject] public DiContainer Container;

    public MainContainerReference()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
