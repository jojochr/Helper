/// <summary>
/// This implementation is very basic and only meant for light weight use and starting off with the Result type.<br></br>
/// Please either look for a Result-implementation with more Features like Fluent.Results if needed or, if you really want to, extend this type.
/// </summary>
public readonly struct Result<T, E> {
    private readonly bool _success;
    public readonly T Value;
    public readonly E Error;

    private Result(T v, E e, bool success)
    {
        Value = v;
        Error = e;
        _success = success;
    }

    public bool IsOk => _success;

    public static Result<T, E> Ok(T v)
    {
        return new(v, default(E), true);
    }

    public static Result<T, E> Err(E e)
    {
        return new(default(T), e, false);
    }

    public static implicit operator Result<T, E>(T v) => new(v, default(E), true);
    public static implicit operator Result<T, E>(E e) => new(default(T), e, false);

    /// <summary>
    /// This can be used to handle both the success case and the error case and output a specified type
    /// </summary>
    public R Match<R>(
            Func<T, R> success,
            Func<E, R> failure) =>
        _success ? success(Value) : failure(Error);
    
    /// <summary>
    /// This can be used to transform the values inside a result without resolving it.
    /// </summary>
    /// <param name="success">This method gets run in the value case with the value as parameter</param>
    /// <param name="failure">This method gest run in the error case with the error as parameter</param>
    /// <returns>The result with its transformed values</returns>
    public Result<T, E> Transform(Action<T> success, Action<E> failure) {
        if(_success) {
            success(Value);
            return this;
        }

        failure(Error);
        return this;
    }

    /// <summary>
    /// This can be used to resolve the result.
    /// </summary>
    /// <param name="success"></param>
    /// <param name="failure"></param>
    /// <returns>This returns:<br></br>True -> If the result contains a value.<br></br>False -> If the Result contains an error.</returns>
    public bool Resolve(Action<T> success, Action<E> failure) {
        if(_success) {
            success(Value);
            return true;
        }

        failure(Error);
        return false;
    }
}