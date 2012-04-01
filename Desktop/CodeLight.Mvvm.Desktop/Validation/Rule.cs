// Borrowed from Cinch by Sasha Barber

namespace CodeValue.CodeLight.Mvvm.Validation
{
    public abstract class Rule
    {
        private string _propertyName;

        public virtual string Description { get; protected set; }

        public virtual string PropertyName
        {
            get { return (this._propertyName ?? string.Empty).Trim(); }
            protected set { this._propertyName = value; }
        }

        public Rule(string propertyName, string brokenDescription)
        {
            this.Description = brokenDescription;
            this.PropertyName = propertyName;
        }

        public abstract bool ValidateRule(object domainObject);

        public override string ToString()
        {
            return this.Description;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }

}