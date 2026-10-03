public class RoomMarkerGenerationResult
{
    public GeneratedRoomMarker PlayerSpawn { get; }

    public GeneratedRoomMarker RoomEntry { get; }

    public GeneratedPlatform StartPlatform { get; }

    public GeneratedPlatform ExitPlatform { get; }

    public RoomMarkerGenerationResult(
        GeneratedRoomMarker playerSpawn,
        GeneratedRoomMarker roomEntry,
        GeneratedPlatform startPlatform,
        GeneratedPlatform exitPlatform
    )
    {
        PlayerSpawn =
            playerSpawn;

        RoomEntry =
            roomEntry;

        StartPlatform =
            startPlatform;

        ExitPlatform =
            exitPlatform;
    }
}