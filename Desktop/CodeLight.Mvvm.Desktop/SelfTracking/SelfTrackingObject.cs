using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CodeValue.CodeLight.Mvvm.Validation;

namespace CodeValue.CodeLight.Mvvm.SelfTracking
{
    public class SelfTrackingObject : ValidatingObject
    {
        private readonly Dictionary<string, Tuple<object, object>> _data = new Dictionary<string, Tuple<object, object>>();

        public SelfTrackingObject()
        {
            DescriptionAction = (o1, o2) => string.Format("Original value is {0}. Value {1} is currently in transaction.", o1, o2);
        }

        protected void SetValue(object value, string property)
        {

            if (!_data.ContainsKey(property))
            {
                _data[property] = new Tuple<object, object>(value, null);
                return;

            }
            var current = _data[property];
            _data[property] = new Tuple<object, object>(current.Item1, value);
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

        protected void SetValue<T>(object value, Expression<Func<T>> property)
        {
            SetValue(value, GetPropertyNameFromExpression(property));
        }

        protected object GetValue(string property)
        {
            if (!_data.ContainsKey(property))
            {
                return null;
            }
            var tuple = _data[property];
            return tuple.Item2 ?? tuple.Item1;
        }

        protected object GetValue<T>(Expression<Func<T>> property)
        {
            return GetValue(GetPropertyNameFromExpression(property));
        }

        public void Commit()
        {
            foreach (var key in _data.Keys.ToList())
            {
                var tuple = _data[key];
                _data[key] = new Tuple<object, object>(tuple.Item2, null);
                OnPropertyChanged(key);
            }
        }

        public void Revert()
        {
            foreach (var key in _data.Keys.ToList())
            {
                var tuple = _data[key];
                _data[key] = new Tuple<object, object>(tuple.Item1, null);
                OnPropertyChanged(key);
            }
        }

        public bool IsDirty
        {
            get { return _data.Values.Any(t => t.Item2 != null); }
        }
        public Func<object, object, string> DescriptionAction { get; set; }

    }
}