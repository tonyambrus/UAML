namespace Uaml.UX
{
    public struct DependencyPropertyChangedEventArgs
    {
        public DependencyProperty Property;
        public object OldValue;
        public object NewValue;

        public DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue)
        {
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}