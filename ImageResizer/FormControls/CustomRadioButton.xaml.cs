using System.ComponentModel;

namespace ImageResizer.FormControls;

public partial class CustomRadioButton : ContentView, INotifyPropertyChanged
{
    private static Dictionary<string, List<CustomRadioButton>> _radioButtonGroups = new Dictionary<string, List<CustomRadioButton>>();

    private static void _attachRadioButtonToGroup(CustomRadioButton radioButton, string newGroup)
    {
        var oldGroup = _radioButtonGroups.Keys.ToList().Find(k =>
             _radioButtonGroups[k].IndexOf(radioButton) != -1
        );
        
        if (newGroup == oldGroup) return;
        
        if(oldGroup != null) _radioButtonGroups[oldGroup].Remove(radioButton);
        
        if (!_radioButtonGroups.ContainsKey(newGroup))
        {
            _radioButtonGroups.Add(newGroup, new List<CustomRadioButton>());
        }
        
        _radioButtonGroups[newGroup].Add(radioButton);
    }

    private static void _uncheckAllOtherGroupMembers(CustomRadioButton radioButton, string group)
    {
        if (_radioButtonGroups.ContainsKey(group))
        {
            foreach (var r in _radioButtonGroups[group])
            {
                if (r != radioButton)
                {
                    r.IsChecked = false;
                }
            }
        }
    }
    
    public static BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomRadioButton), false);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set {
            SetValue(IsCheckedProperty, value);
            
            if (value)
            {
                _uncheckAllOtherGroupMembers(this, GroupName);
            }
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(value));
        }
    }
    
    public static BindableProperty LabelTextProperty =
        BindableProperty.Create("LabelText", typeof(string), typeof(CustomRadioButton), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public static BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(CustomRadioButton), System.Guid.NewGuid().ToString());

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set {
            SetValue(GroupNameProperty, value);
            _attachRadioButtonToGroup(this, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupName)));
        }
    }
    
    public event EventHandler<CheckedChangedEventArgs>? CheckedChanged;
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public CustomRadioButton()
    {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedEventArgs args)
    {
        IsChecked = true;
    }
}