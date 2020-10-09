using UnityEngine;
using Uaml.MRTK;

namespace Uaml.Examples.MRTK
{
    public partial class MainWindow : Uaml.MRTK.Window
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
}
