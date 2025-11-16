using System.Collections.Specialized;
using ImageResizer.ViewModels;

namespace TestImageResizer.ViewModels;

public class TestLiveSortedList
{
    private static IReadOnlyList<string> _sortedList = new List<string>()
    {
        "Aardvark",
        "Jaguar",
        "Kangaroo",
        "Zebra"
    };

    private static IReadOnlyList<string> _reversedList = _sortedList.OrderByDescending(item => item).ToList();
    
    [Fact]
    public void TestAddingAnItemAddsAnItem()
    {
        var liveSortedList = new LiveSortedList<string>();
        
        liveSortedList.Add(_sortedList[3]);
        liveSortedList.Add(_sortedList[1]);
        liveSortedList.Add(_sortedList[2]);
        liveSortedList.Add(_sortedList[0]);
        
        Assert.Equal(_sortedList.Count(), liveSortedList.Count());

        for (int i = 0; i < _sortedList.Count(); i++)
        {
            Assert.Equal(_sortedList[i], liveSortedList.ElementAt(i));
        }
    }

    [Fact]
    public void TestRemovingAnItemRemovesAnItem()
    {
        var liveSortedList = new LiveSortedList<string>();
        liveSortedList.Add("Zebra");

        Assert.Contains("Zebra", liveSortedList);
        
        liveSortedList.Remove("Zebra");
        Assert.DoesNotContain("Zebra", liveSortedList);
    }

    [Fact]
    public void TestReversingTheListReversesTheList()
    {
        var liveSortedList = new LiveSortedList<string>();
        
        liveSortedList.Add(_sortedList[3]);
        liveSortedList.Add(_sortedList[1]);
        liveSortedList.Add(_sortedList[2]);
        liveSortedList.Add(_sortedList[0]);
        
        Assert.Equal(_sortedList.Count(), liveSortedList.Count());

        for (int i = 0; i < _sortedList.Count(); i++)
        {
            Assert.Equal(_sortedList[i], liveSortedList.ElementAt(i));
        }

        liveSortedList.IsReversed = true;
        
        for (int i = 0; i < _reversedList.Count(); i++)
        {
            Assert.Equal(_reversedList[i], liveSortedList.ElementAt(i));
        }
    }

    [Fact]
    public void TestTheListRemainsReversedWhenAddingANewItem()
    {
        var liveSortedList = new LiveSortedList<string>();
        liveSortedList.IsReversed = true;

        foreach (var item in _reversedList)
        {
            liveSortedList.Add(item);
            for (int i = 0; i < liveSortedList.Count() - 1; i++)
            {
               Assert.True(liveSortedList.ElementAt(i).CompareTo(liveSortedList.ElementAt(i + 1)) > 0);
            }
        }
    }

    [Fact]
    public void TestTheListRemainsReversedWhenRemovingAnItem()
    {
        var liveSortedList = new LiveSortedList<string>();
        liveSortedList.IsReversed = true;

        foreach (var item in _sortedList)
        {
            liveSortedList.Add(item);
        }

        while (liveSortedList.Count() > 0)
        {
            liveSortedList.Remove(liveSortedList.ElementAt(liveSortedList.Count() / 2));
            
            for (int i = 0; i < liveSortedList.Count() - 1; i++)
            {
                Assert.True(liveSortedList.ElementAt(i).CompareTo(liveSortedList.ElementAt(i + 1)) > 0);
            }
        }
    }

    [Fact]
    public void TestTheListReturnsToItsNormalSortOrderWhenUnReversed()
    {
        var liveSortedList = new LiveSortedList<string>();
        liveSortedList.IsReversed = true;

        foreach (var item in _sortedList)
        {
            liveSortedList.Add(item);
        }

        for (int i = 0; i < _reversedList.Count(); i++)
        {
            Assert.Equal(_reversedList[i], liveSortedList.ElementAt(i));
        }

        liveSortedList.IsReversed = false;
        
        for (int i = 0; i < _sortedList.Count(); i++)
        {
            Assert.Equal(_sortedList[i], liveSortedList.ElementAt(i));
        }
    }
}