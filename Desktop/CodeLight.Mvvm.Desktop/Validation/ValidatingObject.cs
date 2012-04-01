using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Borrowed from Cinch by Sasha Barber

namespace CodeValue.CodeLight.Mvvm.Validation
{
    public class ValidatingObject : NotifyObject, IDataErrorInfo
    {
        StringBuilder _errorsBuilder = new StringBuilder(); 
        private readonly List<Rule> _rules = new List<Rule>();

        //public virtual bool IsValid
        //{
        //    get { return string.IsNullOrEmpty(this.Error); }
        //}

        private bool _isValid;

        public virtual bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    OnPropertyChanged(() => IsValid);
                }
            }
        }

        public virtual string Error
        {
            get
            {
                string str = this[string.Empty];
                if (str != null && str.Trim().Length == 0)
                    str = (string) null;
                return str;
            }
        }

        public virtual string this[string propertyName]
        {
            get
            {
                _errorsBuilder.Clear();

                propertyName = this.CleanString(propertyName);
                foreach (Rule rule in this.GetBrokenRules(propertyName))
                {
                    if (propertyName == string.Empty || rule.PropertyName == propertyName)
                    {
                        _errorsBuilder.AppendLine(rule.Description);
                    }
                }
#if SILVERLIGHT
#else
                // Check for DataAnnotatoin validatoin
                var info = this.GetType().GetProperty(propertyName);
                if (info != null)
                {
                    object value = info.GetValue(this, null);

                    IEnumerable<string> errorInfos =
                        (from va in info.GetCustomAttributes(true).OfType<ValidationAttribute>()
                         where !va.IsValid(value)
                         select va.FormatErrorMessage(string.Empty)).ToList();
                    foreach (var error in errorInfos)
                    {
                        _errorsBuilder.AppendLine(error);

                    }
                }
#endif
                string str2 = _errorsBuilder.ToString().Trim();
                if (str2.Length == 0)
                    str2 = (string) null;
                IsValid = str2 == null;
                return str2;
            }
        }

        public virtual ReadOnlyCollection<Rule> GetBrokenRules()
        {
            return this.GetBrokenRules(string.Empty);
        }

        public virtual ReadOnlyCollection<Rule> GetBrokenRules(string property)
        {
            property = this.CleanString(property);
            List<Rule> list = new List<Rule>();
            foreach (Rule rule in this._rules)
            {
                if (rule.PropertyName == property || property == string.Empty)
                {
                    bool flag = rule.ValidateRule((object) this);
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + ": Validating the rule: '" + rule.ToString() +
                                    "' on object '" + this.ToString() + "'. Result = " + (!flag ? "Valid" : "Broken"));
                    if (flag)
                        list.Add(rule);
                }
            }
            
            return list.AsReadOnly();
        }

        public void AddRule(Rule newRule)
        {
            this._rules.Add(newRule);
        }

        public void RemoveRule(Rule rule)
        {
            this._rules.Remove(rule);
        }

        public IEnumerable<Rule> GetRulesByPropertyName(string propertyName)
        {
            return _rules.Where(r => r.PropertyName == propertyName);
        }

        protected string CleanString(string s)
        {
            return (s ?? string.Empty).Trim();
        }
    }



}