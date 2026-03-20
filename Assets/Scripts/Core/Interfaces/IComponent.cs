namespace Core.Interfaces
{
    public interface IComponent<T> where T : IController
    {
        T CreateController();
    }
}