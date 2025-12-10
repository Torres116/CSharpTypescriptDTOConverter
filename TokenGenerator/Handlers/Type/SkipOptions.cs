namespace TokenGenerator.Handlers.Type;

[Flags]
public enum SkipOptions
{
    Dictionary,
    List,
    Nullable,
    Optional,
    CustomType,
}