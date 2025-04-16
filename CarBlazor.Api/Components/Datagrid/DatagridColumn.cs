namespace CarBlazor.Components.Datagrid
{
    public class DataGridColumn<TItem>
    {
        public string Title { get; set; } = string.Empty;
        public Func<TItem, object?> ValueGetter { get; set; } = _ => null;
    }
}