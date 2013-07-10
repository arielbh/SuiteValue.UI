using System;

#if WINDOWS_PHONE
namespace SuiteValue.UI.WP8.Validation
#else
namespace SuiteValue.UI.WPF.Validation
#endif
{
    // Borrowed from Cinch by Sasha Barber
    public class SimpleRule : Rule
    {
        private Func<object, bool> ruleDelegate;

        protected virtual Func<object, bool> RuleDelegate
        {
            get
            {
                return this.ruleDelegate;
            }
            set
            {
                this.ruleDelegate = value;
            }
        }

        public SimpleRule(string propertyName, string brokenDescription, Func<object, bool> ruleDelegate)
            : base(propertyName, brokenDescription)
        {
            this.RuleDelegate = ruleDelegate;
        }

        protected override bool ValidateRule(object domainObject)
        {
            return this.RuleDelegate(domainObject);
        }
    }

    public class SimpleRule<T> : Rule
    {
        private Func<T, bool> ruleDelegate;

        protected virtual Func<T, bool> RuleDelegate
        {
            get
            {
                return this.ruleDelegate;
            }
            set
            {
                this.ruleDelegate = value;
            }
        }

        public SimpleRule(string propertyName, string brokenDescription, Func<T, bool> ruleDelegate)
            : base(propertyName, brokenDescription)
        {
            this.RuleDelegate = ruleDelegate;
        }

        protected override bool ValidateRule(object domainObject)
        {
            return this.RuleDelegate((T)domainObject);
        }
    }
}