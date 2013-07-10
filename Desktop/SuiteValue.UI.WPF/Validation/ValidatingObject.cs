using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SuiteValue.UI.MVVM;
#if !WINDOWS_PHONE
#else
using SuiteValue.UI.MVVM;
#endif


// Borrowed from Cinch by Sasha Barber
#if WINDOWS_PHONE
namespace SuiteValue.UI.WP8.Validation
#else
namespace SuiteValue.UI.WPF.Validation
#endif
{
    public class ValidatingObject : NotifyObject, IDataErrorInfo
    {
        StringBuilder _errorsBuilder = new StringBuilder(); 
        private readonly List<Rule> _rules = new List<Rule>();


        public bool IsValid
        {
            get { return _rules.All(r => r.IsValid); }
       
        }

        private bool UpdateState()
        {
            return string.IsNullOrEmpty(Error);
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
                var valid = true;

                propertyName = this.CleanString(propertyName);
                foreach (Rule rule in this.GetBrokenRules(propertyName))
                {
                    if (propertyName == string.Empty || rule.PropertyName == propertyName)
                    {
                        _errorsBuilder.AppendLine(rule.Description);
                        valid = false;
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
                        valid = false;
                    }
                }
#endif
                string str2 = _errorsBuilder.ToString().Trim();
                if (str2.Length == 0)
                    str2 = (string) null;
                //if (valid != IsValid)
                //{
                //    UpdateState();
                //}
                return str2;
            }
        }

        protected virtual ReadOnlyCollection<Rule> GetBrokenRules()                                                        
        {
            return this.GetBrokenRules(string.Empty);
        }

        protected virtual ReadOnlyCollection<Rule> GetBrokenRules(string property)
        {
            property = this.CleanString(property);
            var list = new List<Rule>();
            foreach (Rule rule in this._rules)
            {
                if (rule.PropertyName == property || property == string.Empty)
                {
                    bool isRuleBroken = rule.Validate((object) this);
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + ": Validating the rule: '" + rule.ToString() +
                                    "' on object '" + this.ToString() + "'. Result = " + (!isRuleBroken ? "Valid" : "Broken"));
                    if (isRuleBroken)
                        list.Add(rule);
                }
            }
            
            return list.AsReadOnly();
        }

        public void AddRule(Rule newRule)
        {
            newRule.PropertyChanged += newRule_PropertyChanged;
            this._rules.Add(newRule);
        }

        void newRule_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(() => IsValid);
        }


        public void RemoveRule(Rule rule)
        {
            rule.PropertyChanged -= newRule_PropertyChanged;

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