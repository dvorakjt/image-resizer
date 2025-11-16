using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Views;

public partial class BindableLayoutTest : ContentView
{
    public List<string> Animals
    {
        get;
    } = new()
    {
        "Aardvark",
        "Bear",
        "Cat",
        "Dog",
        "Elephant",
        "Fish",
        "Gharial",
        "Hawk",
        "Ibis",
        "Jaguar",
        "Zebra"
    };
    
    public BindableLayoutTest()
    {
        InitializeComponent();
        
        // first goal, simply show list
        // second goal, update list (re-order)
        // third goal, add item to list and re-order
        // fourth goal, remove item from list
    }
}