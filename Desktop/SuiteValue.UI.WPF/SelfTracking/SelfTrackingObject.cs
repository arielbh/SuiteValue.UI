using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using SuiteValue.UI.WPF.Validation;
#if WINDOWS_PHONE
using SuiteValue.UI.WP8.Validation;
#else

#endif

#if WINDOWS_PHONE
namespace SuiteValue.UI.WP8
#else
namespace SuiteValue.UI.WPF.SelfTracking
#endif
{
    public class SelfTrackingObject : ValidatingObject
    {
        private readonly Dictionary<string, object> _shortTermMemory = new Dictionary<string, object>();
        private readonly Dictionary<string, Tuple<object, object>> _data = new Dictionary<string, Tuple<object, object>>();

        public SelfTrackingObject()
        {

            DescriptionAction = (o1, o2) => string.Format("Original value is {0}. Value {1} is currently in transaction.", o1, o2);
        }

        protected bool RaiseValidationsError { get; set; }

        protected void SetValue(object value, [CallerMemberName] string property = null)
        {

            if (!_data.ContainsKey(property))
            {
                _data[property] = new Tuple<object, object>(value, null);
                OnPropertyChanged(property);
                return;

            }
            var current = _data[property];
            if (current != null)
            {
                if (current.Item1 != null)
                {
                    if (current.Item1.Equals(value)) return;
                }
            }
            
            _data[property] = new Tuple<object, object>(current.Item1, value);

            OnPropertyChanged(property);
            OnPropertyChanged(() => IsDirty);

            if (RaiseValidationsError)
            {
                var rule = GetRulesByPropertyName(property).FirstOrDefault();
                if (rule != null)
                {
                    RemoveRule(rule);
                }
                AddRule(new SimpleRule(property, DescriptionAction(_data[property].Item1, _data[property].Item2), o =>
                {
                    var t = _data[property];
                    return t.Item2 !=
                           null;
                }));
            }
        }

        protected void SetValue<T>(object value, Expression<Func<T>> property)
        {
            SetValue(value, GetPropertyNameFromExpression(property));
        }

        protected object GetValue([CallerMemberName] string property = null)
        {
            if (!_data.ContainsKey(property))
            {
                return null;
            }
            var tuple = _data[property];
            return tuple.Item2 ?? tuple.Item1;
        }

        protected T GetValue<T>([CallerMemberName] string property = null)
        {
            return GetValue(property) is T ? (T) GetValue(property) : default(T);
        }

        protected object GetValue<T>(Expression<Func<T>> property)
        {
            return GetValue(GetPropertyNameFromExpression(property));
        }
        protected object GetPreviousValue<T>(Expression<Func<T>> property)
        {
            return GetPreviousValue(GetPropertyNameFromExpression(property));
        }

        protected object GetPreviousValue([CallerMemberName] string property = null)
        {
            if (!_data.ContainsKey(property))
            {
                return null;
            }
            var tuple = _data[property];
            if (tuple.Item2 == null)
            {
                return _shortTermMemory[property];
            }
            return tuple.Item1;
        }

        public bool HasPreviousValue<T>(Expression<Func<T>> property)
        {
            return HasPreviousValue(GetPropertyNameFromExpression(property));
        }


        public bool HasPreviousValue([CallerMemberName] string property = null)
        {
            var value = GetPreviousValue(property);
            return value != null;
        }


        public virtual void Commit()
        {
            foreach (var key in _data.Keys.ToList())
            {

                var tuple = _data[key];
                // this property is dirty
                if (tuple.Item2 != null)
                {
                    _shortTermMemory[key] = tuple.Item1;
                    OnPropertyChanging(key);
                    _data[key] = new Tuple<object, object>(tuple.Item2, null);
                    OnPropertyChanged(key);
                }
            }
            OnPropertyChanged(() => IsDirty);
        }

        public virtual void Revert()
        {
            foreach (var key in _data.Keys.ToList())
            {
                var tuple = _data[key];
                _data[key] = new Tuple<object, object>(tuple.Item1, null);
                OnPropertyChanged(key);
            }
            OnPropertyChanged(() => IsDirty);

        }

        public virtual bool IsDirty
        {
            get { return _data.Values.Any(t => t.Item2 != null); }
        }

        public Func<object, object, string> DescriptionAction { get; set; }

    }
}