namespace CoreLoader
{
    public interface IKeyCodes
    {
        uint GetKeyCode(string name);
        string GetKeyName(uint code);
    }
}