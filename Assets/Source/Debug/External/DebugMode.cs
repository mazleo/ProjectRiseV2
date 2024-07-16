namespace ProjectRise.Debug.External
{
    /// <summary>
    /// Consists of debug mode information.
    /// </summary>
    public class DebugMode
    {
        public string Id;
        public string Name;
        public bool Enabled;

        public DebugMode(string id, string name, bool enabled)
        {
            Id = id;
            Name = name;
            Enabled = enabled;
        }
    }
}
