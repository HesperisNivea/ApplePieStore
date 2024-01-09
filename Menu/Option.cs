namespace ApplePieStore.Menu;

public class Option
{
    public string Name { get; set; }
    public Action SelectedAction { get; set; }

    public Option(string name, Action selectedAction)
    {
        Name = name;
        SelectedAction = selectedAction;
    }

    protected Option()
    {
    }

    public virtual void InvokeSelectedMethodWithKey(ConsoleKey key)
    {

    }

    public void InvokeSelectedMethod()
    {
        SelectedAction.Invoke();
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}