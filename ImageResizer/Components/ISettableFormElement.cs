using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizer.Components
{
    internal interface ISettableFormElement<T>
    {
        void SetValue(T value);
    }
}
