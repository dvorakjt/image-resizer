using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.UI;

public partial class LoadingSpinner : ContentView
{
    public LoadingSpinner()
    {
        InitializeComponent();
        Spin();
    }

    public async Task Spin()
    {
        while (true)
        {
            await Spinner.RotateTo(360, 1000, Easing.SinInOut);
            Spinner.Rotation = 0;
        }
    }
}