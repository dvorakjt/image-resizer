using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public partial class DynamicListTest : ContentView
{
    public ILiveSortedList<string> Animals = new LiveSortedList<string>{        
        "Fish",
        "Gharial",
        "Hawk",
        "Aardvark",
        "Bear",
        "Cat",
        "Dog",
        "Elephant",
        "Ibis",
        "Jaguar",
        "Zebra",
        "Kangaroo",
        "Lemur",
        "Narwhal",
        "Monkey",
        "Pig",
        "Quail",
    };

    private IList<string> AnimalsToAdd = new List<string>
    {
        "Snake",
        "Tapir",
        "Viper",
        "Roach",
        "Whale",
    };
    
    
    public DynamicListTest()
    {
        InitializeComponent();

        var layout = new VerticalStackLayout();

        DynamicListFactory.MakeDynamic(layout, Animals, (string animal) =>
        {
            return new Label()
            {
                Text = animal,
            };
        });
        
        MainLayout.Children.Add(layout);

        var reverseButton = new Button()
        {
            Text = "Reverse",
        };

        reverseButton.Clicked += (sender, e) =>
        {
            Animals.IsReversed = !Animals.IsReversed;
        };
        
        MainLayout.Children.Add(reverseButton);

        var addAnimalButon = new Button()
        {
            Text = "Add Animal",
        };

        int i = 0;

        addAnimalButon.Clicked += (sender, e) =>
        {
            if (i < AnimalsToAdd.Count)
            {
                Animals.Add(AnimalsToAdd[i++]);
            }

            if (i >= AnimalsToAdd.Count)
            {
                addAnimalButon.IsEnabled = false;
            }
        };
        
        MainLayout.Children.Add(addAnimalButon);

        var removeAnimalButon = new Button()
        {
            Text = "Remove Animal",
        };

        removeAnimalButon.Clicked += (sender, e) =>
        {
            if (Animals.Count() > 0)
            {
                Animals.Remove(Animals.ElementAt(Animals.Count() / 2));
            }

            if (Animals.Count() == 0)
            {
                removeAnimalButon.IsEnabled = false;
            }
        };
        
        MainLayout.Children.Add(removeAnimalButon);
    }
}