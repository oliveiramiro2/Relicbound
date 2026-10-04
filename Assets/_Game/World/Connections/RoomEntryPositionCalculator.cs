using UnityEngine;

public static class RoomEntryPositionCalculator
{
    public static Vector2 Calculate(
        GeneratedRoom targetRoom,
        Vector2 fromDirection)
    {
        if (targetRoom == null)
            return Vector2.zero;

        if (targetRoom.Markers == null)
            return Vector2.zero;

        GeneratedPlatform platform =
            targetRoom.Markers.StartPlatform;

        if (!PlatformSafetyValidator.IsSafeSpawnPlatform(platform))
        {
            if (targetRoom.Markers.RoomEntry != null)
                return targetRoom.Markers.RoomEntry.Position;

            return Vector2.zero;
        }

        float horizontalOffset = 0f;

        if (fromDirection.sqrMagnitude > 0.001f)
        {
            fromDirection.Normalize();

            float maximumOffset =
                platform.Bounds.x * 0.35f;

            horizontalOffset =
                Mathf.Clamp(
                    fromDirection.x * maximumOffset,
                    -maximumOffset,
                    maximumOffset);
        }

        float x =
            platform.Position.x +
            horizontalOffset;

        float minimumX =
            platform.Bounds.xMin +
            0.5f;

        float maximumX =
            platform.Bounds.xMax -
            0.5f;

        x =
            Mathf.Clamp(
                x,
                minimumX,
                maximumX);

        float y =
            platform.Bounds.yMax + 1f;

        return new Vector2(
            x,
            y);
    }
}