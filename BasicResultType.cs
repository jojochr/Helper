using System;

/// <summary>
/// This implementation is very basic and only meant for light weight use and starting off with the Result type
/// </summary>
public readonly struct Result<TValue, TError> {
    private readonly bool _success;
    private readonly TValue _value;
    private readonly TError _error;

    private Result(TValue v, TError e, bool success) {
        _value = v;
        _error = e;
        _success = success;
    }

    /// <summary>
    /// Indicates if this instance contains a Value or an Error<br></br>
    /// True -> Contains Value
    /// False -> Contains Error
    /// </summary>
    public bool IsOk => _success;

    public static Result<TValue, TError> Ok(TValue v) => new(v, default(TError), true);
    public static Result<TValue, TError> Err(TError e) => new(default(TValue), e, false);

    public static implicit operator Result<TValue, TError>(TValue v) => new(v, default(TError), true);
    public static implicit operator Result<TValue, TError>(TError e) => new(default(TValue), e, false);

    /// <summary>
    /// This method is used either to handle both happy- and error case to convert it into a single Type,<br></br>
    /// Or to convert one Result, Type into another Result Type just like a "Map" or "Select" method
    /// </summary>
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure) {
        if(_success)
            return success(_value);

        return failure(_value);
    }

    /// <summary>
    /// This can be used to dump the contents of a result type<br></br>
    /// This method is dangerous and should preferably not end up in production code<br></br>
    /// Only use this if you know what you are doing
    /// </summary>
    public bool Dump(out TValue value, out TError error, out bool success) {
        value = _value;
        error = _error;
        success = _success;
        return _success;
    }
}
