namespace Game.Windows
{
    public interface IWindowManager
    {
        void OpenWindow<T>() where T : WindowBase;
        void CloseWindow<T>() where T : WindowBase;
        void CloseAll();
        void CloseCurrent();
        void Initialize();
    }
}