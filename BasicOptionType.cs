public struct Option<T> {
    private Option(T value) {
        this.value = value;
        isSome = this.value is { };
    }
    
    private readonly bool isSome;
    private readonly T value;
    
    public static implicit operator Option<T>(T value) => new Option<T>(value);
    
    public static Option<T> None => default;
    public static Option<T> Some(T value) => new Option<T>(value);

    public bool IsSome(out T value) {
        value = this.value;
        return isSome;
    }
}