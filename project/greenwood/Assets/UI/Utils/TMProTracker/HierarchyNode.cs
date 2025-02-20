using System.Collections.Generic;
using TMPro;

public class HierarchyNode
{
    public string Name;
    public string FullPath;
    public List<HierarchyNode> Children = new List<HierarchyNode>();
    public List<TextMeshProUGUI> TMPItems = new List<TextMeshProUGUI>();

    public HierarchyNode(string name, string fullPath)
    {
        Name = name;
        FullPath = fullPath;
    }
}
