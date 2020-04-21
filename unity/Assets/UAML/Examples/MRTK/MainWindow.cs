using UnityEngine;
using Uaml.MRTK;
using Uaml.Events;

namespace Uaml.Examples.MRTK
{
    public partial class MainWindow : Window
    {
        protected override void Awake()
        {
            base.Awake();
            InitializeComponent();
        }
    }
}