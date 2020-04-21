using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Uaml.Core;
using Uaml.UX;

namespace Uaml.MRTK
{
    public class ScrollView : Element
    {
        private ScrollingObjectCollection _ScrollView => Instance.GetPath<ScrollingObjectCollection>("ScrollView/ScrollingObjectCollection");

        protected override void OnChildAdded(FrameworkElement child) => _ScrollView.UpdateCollection();
        protected override void OnChildRemoved(FrameworkElement child) => _ScrollView.UpdateCollection();
    }
}