namespace Uaml.UX
{
    public delegate void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e);
    public delegate object CoerceValueCallback(DependencyObject d, object baseValue);

    public class PropertyMetadata
    {
        public CoerceValueCallback CoerceValueCallback { get; private set; }
        public object DefaultValue { get; private set; }
        public bool IsSealed { get; private set; }
        public PropertyChangedCallback PropertyChangedCallback { get; private set; }

        public PropertyMetadata()
        {
        }

        public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback = null, CoerceValueCallback coerceValueCallback = null)
        {
            DefaultValue = defaultValue;
            PropertyChangedCallback = propertyChangedCallback;
            CoerceValueCallback = coerceValueCallback;
        }

        public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
        {
            PropertyChangedCallback = propertyChangedCallback;
        }
    }
}