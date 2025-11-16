namespace ImageResizer.ViewModels;

public class DynamicListFactory
{
    public static void MakeDynamic<U, V>(Layout layout, U items, Func<V, View> mappingFunc) 
        where U : IEnumerable<V>, INotifyListItemAdded<V>, INotifyListItemRemoved, INotifyListItemReset
    {
        foreach (var item in items)
        {
            layout.Add(mappingFunc(item));
        }

        items.ItemAdded += (sender, e) =>
        {
            layout.Children.Insert(e.NewIndex, mappingFunc(e.NewItem));
        };

        items.ItemRemoved += (sender, e) =>
        {
            layout.Children.RemoveAt(e.OldIndex);
        };

        items.ListReset += (sender, e) =>
        {
            layout.Children.Clear();
            
            foreach (var item in items)
            {
                layout.Add(mappingFunc(item));
            }
        };
    }
}