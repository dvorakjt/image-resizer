using ImageResizer.ViewModels;

namespace TestImageResizer.ViewModels;

public class TestReorderableLiveList
{
    [Fact]
    public void TestAddingAnItemAddsAnItem()
    {
        var list = new ReorderableLiveList<string>();
        Assert.Empty(list);
        
        list.Add("Aardvark");
        Assert.Contains("Aardvark", list);
        
        list.Add("Cow");
        Assert.Contains("Cow", list);
        Assert.Equal(2, list.Count());
    }

    [Fact]
    public void TestRemovingAnItemRemovesAnItem()
    {
        var list = new ReorderableLiveList<string>();
        list.Add("Aardvark");
        list.Add("Cow");
        list.Add("Meerkat"); 
        Assert.Equal(3, list.Count());
        
        list.Remove("Cow");
        Assert.DoesNotContain("Cow", list);
        Assert.Equal(2, list.Count());
        
        list.Remove("Meerkat");
        Assert.DoesNotContain("Meerkat", list);
        Assert.Equal(1, list.Count());
        
        list.Remove("Aardvark");
        Assert.Empty(list);
    }

    [Fact]
    public void TestRemovingAnNonExistentItemPerformsNoOp()
    {
        var list = new ReorderableLiveList<string>();
        list.Add("Aardvark");
        list.Add("Cow");
        list.Add("Meerkat");
        Assert.Equal(3, list.Count());
        
        list.Remove("Zebra");
        Assert.Equal(3, list.Count());
    }

    [Fact]
    public void TestMovingAnItemMovesItToTheProvidedIndex()
    {
        var list = new ReorderableLiveList<string>();
        var itemToMove = "Aardvark";
        list.Add(itemToMove);
        
        var otherItems = new List<string> { "Cow", "Meerkat" };
        foreach (var item in otherItems)
        {
            list.Add(item);
        }
        
        Assert.Equal(3, list.Count());
        Assert.Equal(0,  list.IndexOf(itemToMove));
        Assert.True(otherItems.All(i => list.Contains(i)));

        for (var i = 0; i < list.Count(); i++)
        {
            list.Move(itemToMove, i);
            Assert.Equal(3, list.Count());
            Assert.Equal(i,  list.IndexOf(itemToMove));
            Assert.True(otherItems.All(i => list.Contains(i)));
        }

        for (var i = list.Count() - 1; i >= 0; i--)
        {
            list.Move(itemToMove, i);
            Assert.Equal(3, list.Count());
            Assert.Equal(i,  list.IndexOf(itemToMove));
            Assert.True(otherItems.All(i => list.Contains(i)));
        }
    }

    [Fact]
    public void TestMovingAnItemToANegativeIndexPerformsNoOp()
    {
        var list = new ReorderableLiveList<string>();
        list.Add("Aardvark");
        list.Add("Cow");
        list.Add("Meerkat");

        var originalList = new List<string>(list);
        
        list.Move("Aardvark", -1);
        Assert.Equal(originalList.Count(), list.Count());

        int i = 0;
        foreach (var item in list)
        {
            Assert.Equal(originalList[i++], item);
        }
    }
    
    [Fact]
    public void TestMovingAnItemToAnIndexGreaterThanTheLastPerformsNoOp()
    {
        var list = new ReorderableLiveList<string>();
        list.Add("Aardvark");
        list.Add("Cow");
        list.Add("Meerkat");

        var originalList = new List<string>(list);
        
        list.Move("Aardvark", list.Count());
        Assert.Equal(originalList.Count(), list.Count());

        int i = 0;
        foreach (var item in list)
        {
            Assert.Equal(originalList[i++], item);
        }
    }

    [Fact]
    public void TestMovingANonExistentItemPerformsNoOp()
    {
        var list = new ReorderableLiveList<string>();
        list.Add("Aardvark");
        list.Add("Cow");
        list.Add("Meerkat");
        
        var originalList = new List<string>(list);
        
        list.Move("Zebra", 0);
        Assert.Equal(originalList.Count(), list.Count());

        int i = 0;
        foreach (var item in list)
        {
            Assert.Equal(originalList[i++], item);
        }
    }

    [Fact]
    public void TestAddingAnItemEmitsAnEvent()
    {
        var list = new ReorderableLiveList<string>();
        bool addedItem = false;
        list.ItemAdded += (sender, args) => addedItem = true;
        Assert.False(addedItem);
        
        list.Add("Aardvark");
        Assert.True(addedItem);
    }
    
    [Fact]
    public void TestRemovingAnItemEmitsAnEvent()
    {
        var list = new ReorderableLiveList<string>();
        bool removedItem = false;
        list.ItemRemoved += (sender, args) => removedItem = true;
        list.Add("Aardvark");
        Assert.False(removedItem);
        
        list.Remove("Aardvark");
        Assert.True(removedItem);
    }
    
    [Fact]
    public void TestMovingAnItemEmitsAnEvent()
    {
        var list = new ReorderableLiveList<string>();
        bool movedItem = false;
        list.ItemMoved += (sender, args) => movedItem = true;
        
        list.Add("Aardvark");
        list.Add("Zebra");
        Assert.False(movedItem);

        list.Move("Zebra", 0);
        Assert.True(movedItem);
    }
}