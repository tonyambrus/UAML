using Uaml.UX;

namespace Uaml.Core
{
    public static class ValueParser
    {
        public static bool TryParse(string value, DependencyProperty property, out IValueSetter setter)
        {
            bool isLiteral = true; // TODO: binding
            bool isBinding = false; // TODO: binding

            if (isLiteral && ValueConverter.TryConvert(value, property.PropertyType, out var result))
            {
                setter = new LiteralSetter(property, result);
                return true;
            }
            else if (isBinding)
            {
                // TODO: binding
            }

            setter = null;
            return false;
        }
    }
}