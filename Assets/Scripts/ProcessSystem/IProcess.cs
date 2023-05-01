namespace ProcessSystem
{
    public interface IProcess
    {
        bool Activated { get; set; }

        void AwakeMe();
        void StartMe();
        void UpdateMe();
        void DestroyMe();
    }
}
