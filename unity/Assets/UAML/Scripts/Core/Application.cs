using System;
using System.Reflection;
using Uaml.Events;
using Uaml.Internal.Reflection;
using Uaml.UX;

namespace Uaml.Core
{
    public static class Application
    {
        public static void LoadComponent(FrameworkElement element) => LoadComponent(element, element);

        private static void LoadComponent(FrameworkElement element, FrameworkElement root)
        {
            //element.BindProperties();
            element.BindEvents(root);
        }
    }
}
