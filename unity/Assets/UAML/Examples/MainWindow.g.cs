using UnityEngine;
using Uaml.UX;

public partial class MainWindow : Window
{
    private bool _contentLoaded;
    
    public void InitializeComponent()
    {
        if (_contentLoaded)
        {
            return;
        }
        _contentLoaded = true;
        
        Uaml.Core.Application.LoadComponent(this);
        Debug.Log("Loaded component MainWindow");
    }
}
