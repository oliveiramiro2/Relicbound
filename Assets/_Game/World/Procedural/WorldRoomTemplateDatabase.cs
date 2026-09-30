using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Room Template Database",
    fileName = "WorldRoomTemplateDatabase"
)]
public class WorldRoomTemplateDatabase : ScriptableObject
{
  [SerializeField]
  private List<WorldRoomTemplate> templates = new();

  public IReadOnlyList<WorldRoomTemplate> Templates =>
      templates;

  public WorldRoomTemplate GetRandomTemplate(
      WorldRoomType type,
      System.Random random
  )
  {
    List<WorldRoomTemplate> matchingTemplates =
        new List<WorldRoomTemplate>();

    foreach (
        WorldRoomTemplate template
        in templates
    )
    {
      if (template == null)
        continue;

      if (template.RoomType != type)
        continue;

      matchingTemplates.Add(template);
    }

    if (matchingTemplates.Count == 0)
      return null;

    int index =
        random.Next(
            matchingTemplates.Count
        );

    return matchingTemplates[index];
  }
}