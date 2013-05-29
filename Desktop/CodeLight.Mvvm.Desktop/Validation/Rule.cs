// Borrowed from Cinch by Sasha Barber
#if WINDOWS_PHONE
using SuiteValue.UI.MVVM;
#endif
#if WINDOWS_PHONE
namespace SuiteValue.UI.WP8.Validation
    
#else
namespace CodeValue.CodeLight.Mvvm.Validation
#endif
{
    public abstract class Rule : NotifyObject
    {
        private string _propertyName;

        public virtual string Description { get; protected set; }

        public virtual string PropertyName
        {
            get { return (this._propertyName ?? string.Empty).Trim(); }
            protected set { this._propertyName = value; }
        }

        protected Rule(string propertyName, string brokenDescription)
        {
            Description = brokenDescription;
            PropertyName = propertyName;
        }
        
        protected abstract bool ValidateRule(object domainObject);

        public virtual bool Validate(object domainObject)
        {
            IsValid = !ValidateRule(domainObject);
            return !IsValid;
        }

        

        public override string ToString()
        {
            return this.Description;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        private bool _isValid;

        public bool IsValid
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
    }

}