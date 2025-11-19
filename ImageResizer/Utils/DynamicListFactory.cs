namespace ImageResizer.Utils;

public class DynamicListFactory
{
    public static void MakeDynamic<V>(Layout layout, ILiveList<V> items, Func<V, View> mappingFunc)
    {
        foreach (var item in items)
        {
            layout.Add(mappingFunc(item));
        }

        items.ItemAdded += (sender, e) =>
        {
            var newElement = mappingFunc(e.NewItem);
            layout.Children.Insert(e.NewIndex, newElement);
        };

        items.ItemRemoved += (sender, e) =>
        {
            layout.Children.RemoveAt(e.OldIndex);
        };

        items.ItemMoved += (sender, e) =>
        {
            var item = layout.Children[e.OldIndex];
            layout.Children.Remove(item);
            layout.Children.Insert(e.NewIndex, item);
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