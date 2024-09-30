public static class MathL
{
    public static float LerpOutSmooth(float time, float duration)
    {
        var apex = duration;
        var maxValue = apex * (duration * 2 - apex);

        var t = time * (duration * 2 - time);
        return t / maxValue;
    }

    public static float LerpInSmooth(float time, float duration)
    {
        var t = (time * time) / duration;
        return t / duration;
    }
}
