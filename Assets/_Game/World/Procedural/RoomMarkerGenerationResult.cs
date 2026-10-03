public class RoomMarkerGenerationResult
{
    public GeneratedRoomMarker PlayerSpawn { get; }

    public GeneratedRoomMarker RoomEntry { get; }

    public GeneratedPlatform ExitPlatform { get; }

    public RoomMarkerGenerationResult(
        GeneratedRoomMarker playerSpawn,
        GeneratedRoomMarker roomEntry,
        GeneratedPlatform exitPlatform
    )
    {
        PlayerSpawn =
            playerSpawn;

        RoomEntry =
            roomEntry;

        ExitPlatform =
            exitPlatform;
    }
}