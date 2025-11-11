namespace Web.Components.buttons;

public sealed class DropdownOption<T>(T value, int index)  
{
    public T Value { get; } = value;
    public int Index { get; } = index;
}