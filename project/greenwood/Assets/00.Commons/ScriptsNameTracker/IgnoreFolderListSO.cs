using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptNameTracker/Ignore Folder List", fileName = "IgnoreFolderListSO")]
public class IgnoreFolderListSO : ScriptableObject
{
    public List<string> ignoredFolders = new List<string>();
}
