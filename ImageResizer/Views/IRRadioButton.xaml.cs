using System.ComponentModel;

namespace ImageResizer.Views;

public partial class IRRadioButton : ContentView, INotifyPropertyChanged
{
    private static Dictionary<string, List<IRRadioButton>> _radioButtonGroups = new Dictionary<string, List<IRRadioButton>>();

    private static void _attachRadioButtonToGroup(IRRadioButton radioButton, string newGroup)
    {
        var oldGroup = _radioButtonGroups.Keys.ToList().Find(k =>
             _radioButtonGroups[k].IndexOf(radioButton) != -1
        );
        
        if (newGroup == oldGroup) return;
        
        if(oldGroup != null) _radioButtonGroups[oldGroup].Remove(radioButton);
        
        if (!_radioButtonGroups.ContainsKey(newGroup))
        {
            _radioButtonGroups.Add(newGroup, new List<IRRadioButton>());
        }
        
        _radioButtonGroups[newGroup].Add(radioButton);
    }

    private static void _uncheckAllOtherGroupMembers(IRRadioButton radioButton, string group)
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
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(IRRadioButton), false);

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
        BindableProperty.Create("LabelText", typeof(string), typeof(IRRadioButton), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public static BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(IRRadioButton), System.Guid.NewGuid().ToString());

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set {
            SetValue(GroupNameProperty, value);
            _attachRadioButtonToGroup(this, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupName)));
        }
    }
    
    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public IRRadioButton()
    {
        InitializeComponent();
    }

    private void OnClicked(object sender, TappedEventArgs args)
    {
        IsChecked = true;
    }
}