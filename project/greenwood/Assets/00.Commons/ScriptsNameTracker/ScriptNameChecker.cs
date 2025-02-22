using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptNameChecker
{
    /// <summary>
    /// 모든 C# 스크립트를 검사하여 폴더별로 그룹화한 후, 각 스크립트의 상태를 판별합니다.
    /// ignorePaths 목록에 포함된 경로는 검사에서 제외됩니다.
    /// </summary>
    public static Dictionary<string, List<ScriptData>> CheckScriptNames(List<string> ignorePaths = null)
    {
        Dictionary<string, List<ScriptData>> scriptGroups = new Dictionary<string, List<ScriptData>>();

        string[] scriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (string scriptPath in scriptPaths)
        {
            // ignorePaths가 설정되어 있으면, 경로에 해당 단어가 포함되면 건너뜁니다.
            if (ignorePaths != null && ignorePaths.Any(ip => scriptPath.Replace("\\", "/").Contains(ip)))
                continue;

            string fileName = Path.GetFileNameWithoutExtension(scriptPath);
            string directory = Path.GetDirectoryName(scriptPath)?.Replace(Application.dataPath, "Assets").Replace("\\", "/");
            string[] scriptLines = File.ReadAllLines(scriptPath);

            List<string> foundClasses = new List<string>();

            foreach (string line in scriptLines)
            {
                string trimmedLine = line.Trim();
                // "class" 키워드가 포함된 줄이면
                if (trimmedLine.StartsWith("class ") || trimmedLine.Contains(" class "))
                {
                    string[] words = trimmedLine.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] == "class" && i < words.Length - 1)
                        {
                            string clsName = words[i + 1].Split(new char[] { '{', ':' })[0];
                            // 제네릭 타입 처리: SomeClass<T> → SomeClass
                            if (clsName.Contains("<"))
                            {
                                clsName = clsName.Substring(0, clsName.IndexOf("<"));
                            }
                            foundClasses.Add(clsName);
                        }
                    }
                }
            }

            string assetPath = "Assets" + scriptPath.Replace(Application.dataPath, "").Replace("\\", "/");
            ScriptErrorType type;
            string errorMessage;

            if (foundClasses.Count == 0)
            {
                type = ScriptErrorType.NoClass;
                errorMessage = "Missing Class";
            }
            else if (foundClasses.Count > 1)
            {
                type = ScriptErrorType.MultipleClasses;
                errorMessage = "Multiple Classes";
            }
            else if (foundClasses[0] != fileName)
            {
                type = ScriptErrorType.Mismatch;
                errorMessage = $"Mismatch (Expected: {fileName}, Found: {foundClasses[0]})";
            }
            else
            {
                type = ScriptErrorType.Normal;
                errorMessage = "Normal";
            }

            ScriptData data = new ScriptData(type, fileName, assetPath, foundClasses);

            if (!scriptGroups.ContainsKey(directory))
            {
                scriptGroups[directory] = new List<ScriptData>();
            }
            scriptGroups[directory].Add(data);
        }
        return scriptGroups;
    }
}
