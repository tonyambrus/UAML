using Uaml.UX;

namespace Uaml.Core
{
    public struct LiteralSetter : IValueSetter
    {
        private readonly DependencyProperty property;
        private readonly object value;

        public LiteralSetter(DependencyProperty property, object value)
        {
            this.property = property;
            this.value = value;
        }

        public void Set(DependencyObject instance)
        {
            property.SetValue(instance, value);
        }
    }
}