public interface IState
{
    public void Exist(float deltaTime);
    public void Enter();
    public void Exit();
}