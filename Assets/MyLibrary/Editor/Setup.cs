using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;


public static class Setup
{
  [MenuItem("MyTools/Setup/Create Default Folders")]
  public static void CreateDefaultFOlders()
  {
    Folders.CreateDefault("_Project", "Animation", "Materials", "Prefabs", "ScriptableObjects", "Scripts",
      "Settings");
    Refresh();
  }

  static class Folders
  {
    public static void CreateDefault(string root, params string[] folders)
    {
      var fullpath = Combine(Application.dataPath, root);
      foreach (var folder in folders)
      {
        var path = Combine(fullpath, folder);
        if (!Exists(path))
        {
          CreateDirectory(path);
        }
      }

    }
  }
}

