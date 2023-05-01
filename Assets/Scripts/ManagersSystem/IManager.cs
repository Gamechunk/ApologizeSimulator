namespace ManagersSystem
{
    public interface IManager
    {
        public bool IsActivated { get; set; }
        public void AwakeManager();
        public void StartManager();
        public void UpdateManager();
        //public void FixedUpdateManager();
        //public void LateUpdateManager();
        public void DisposeManager();
    }
}
