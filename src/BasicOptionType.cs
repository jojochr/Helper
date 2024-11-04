using System;

/// <summary>
/// This implementation is very basic and only meant for light weight use and starting off with an Option type
/// </summary>
public readonly struct Option<TValue> {
    /// <summary>Happy case ONLY constructor</summary>
    private Option(TValue value) {
        this.value = value;
        IsSome = true;
    }

    public readonly bool IsSome;
    private readonly TValue value;

    public static implicit operator Option<TValue>(TValue value) => new Option<TValue>(value);

    public static Option<TValue> None => new Option<TValue> { value = default!, IsSome = false };
    public static Option<TValue> Some(TValue value) => new Option<TValue>(value);

    /// <summary>
    /// This can be used to collapse the option instance to a value
    /// </summary>
    /// <param name="fallBack">This will be the fallback value if the Option was none</param>
    /// <returns></returns>
    public TValue Reduce(TValue fallBack) {
        if(IsSome) {
            fallBack = this.value // Overwrite value if this option instance contains a value
        }

        return fallBack;
    }

    /// <summary>
    /// This can be used to map one type of Option into another type of option
    /// </summary>
    /// <typeparam name="TMapped">This is the type of the mapped value</typeparam>
    /// <param name="mappingMethod">This method is the converter that converts the "maybe"-Value of the Option</param>
    /// <returns></returns>
    public Option<TMapped> Map<TMapped>(Func<TValue, TMapped> mappingMethod) {
        if(IsSome) {
            return mappingMethod(this.value);
        }

        return Option<TMapped>.None;
    }
}
