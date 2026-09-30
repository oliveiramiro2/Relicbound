using System.IO;
using UnityEngine;

public class SaveSystem
{
  private const string FileName = "save.json";

  private string SavePath =>
      Path.Combine(
          Application.persistentDataPath,
          FileName
      );

  public void Save(RelicSaveData data)
  {
    if (data == null)
      return;

    string json = JsonUtility.ToJson(
        data,
        true
    );

    File.WriteAllText(
        SavePath,
        json
    );

    Debug.Log(
        $"Game saved to: {SavePath}"
    );
  }

  public RelicSaveData Load()
  {
    if (!File.Exists(SavePath))
    {
      Debug.Log(
          "No save file found."
      );

      return null;
    }

    string json = File.ReadAllText(
        SavePath
    );

    RelicSaveData data =
        JsonUtility.FromJson<RelicSaveData>(
            json
        );

    return data;
  }
}