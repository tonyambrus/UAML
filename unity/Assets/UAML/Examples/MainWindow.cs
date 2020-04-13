using UnityEngine;
using Uaml.UX;
using Uaml.Events;

public partial class MainWindow : Uaml.UX.Window
{
    protected override void Awake()
    {
        base.Awake();
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Debug.Log("Click! Handling this event so it doesn't get routed");
        e.Handled = true;
    }

    private void Button_Click_Routed(object sender, RoutedEventArgs e)
    {
        // TODO: debug why this doesn't hit
        var button = e.OriginalSource as Button;
        Debug.Log("Click routed to main window! Click from: " + button?.Text);
    }
}
