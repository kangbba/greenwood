using System.Collections.Generic;

public enum ScriptErrorType
{
    Normal,
    NoClass,
    MultipleClasses,
    Mismatch
}

public class ScriptData
{
    public ScriptErrorType Type;
    public string FileName;
    public string AssetPath;
    public List<string> DetectedClasses;

    public ScriptData(ScriptErrorType type, string fileName, string assetPath, List<string> detectedClasses)
    {
        Type = type;
        FileName = fileName;
        AssetPath = assetPath;
        DetectedClasses = detectedClasses;
    }
}
