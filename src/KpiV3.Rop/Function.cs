namespace KpiV3.Rop;

public static class Function
{
    /// <summary>
    /// Identity function. Returns its argument.
    /// </summary>
    /// <typeparam name="T"> Argument type. </typeparam>
    /// <param name="value"> Argument that will be returned to the caller. </param>
    /// <returns> Value that was passed as argument. </returns>
    public static T Id<T>(T value) => value;
}
