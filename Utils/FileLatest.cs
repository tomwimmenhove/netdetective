namespace netdetective.Utils;

public class FileLatest
{
    private DateTime _lastWriteTime = DateTime.MinValue;

    public string Path { get; }

    public FileLatest(string path, bool update = false)
    {
        Path = path;
        if (update)
        {
            Update();
        }
    }

    public void Update()
    {
        _lastWriteTime = File.GetLastWriteTime(Path);
    }

    public bool HasNewWrites(bool update)
    {
        var lastWriteTime = File.GetLastWriteTime(Path);
        if (lastWriteTime > _lastWriteTime)
        {
            if (update)
            {
                _lastWriteTime = lastWriteTime;
            }

            return true;
        }

        return false;
    }
}
