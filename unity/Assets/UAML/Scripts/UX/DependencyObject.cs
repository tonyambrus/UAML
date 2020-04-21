using UnityEngine;

namespace Uaml.UX
{
    public class DependencyObject : MonoBehaviour
    {
        internal bool SignalPropertyChanges { get; set; } = true;

        public T GetValue<T>(DependencyProperty dp) => (T)GetValue(dp, setFromDefault: true);
        public object GetValue(DependencyProperty dp) => GetValue(dp, setFromDefault: true);

        private object GetValue(DependencyProperty dp, bool setFromDefault) => dp.GetValue(this);
        public void SetValue(DependencyProperty dp, object newValue) => dp.SetValue(this, newValue);

        internal void NotifyPropertyChanged(DependencyProperty dp, object oldValue, object newValue)
        {
            if (SignalPropertyChanges)
            {
                var e = new DependencyPropertyChangedEventArgs(dp, oldValue, newValue);
                dp.PropertyMetadata?.PropertyChangedCallback?.Invoke(this, e);
                OnPropertyChanged(e);
            }
        }

        protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        protected virtual object GetDefaultValue(DependencyProperty dp)
        {
            return dp.PropertyMetadata?.DefaultValue ?? Util.GetDefaultValue(dp.PropertyType);
        }
    }
}