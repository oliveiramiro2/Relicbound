public class ExtraDashCapability : PlayerCapability
{
    public int ExtraDashes { get; }

    public ExtraDashCapability(int extraDashes = 1)
    {
        ExtraDashes = extraDashes;
    }
}