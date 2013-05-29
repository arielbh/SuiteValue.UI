using System;
using System.Windows;
using SuiteValue.UI.MVVM;
using SuiteValue.UI.WP8;
using SuiteValue.UI.WP8.Validation;

namespace WindowsPhoneSample.ViewModels
{
    public class TestValidationViewModel : NavigationViewModelBase
    {
        public TestValidationViewModel()
        {
            CreditCard = new CreditCardWithValidation();
            CreditCard.PropertyChanged += CreditCard_PropertyChanged;
        }

        void CreditCard_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsValid")
            {
                AddCreditCardCommand.Refresh();

            }
        }

        private CreditCardWithValidation _creditCard;

        public CreditCardWithValidation CreditCard
        {
            get { return _creditCard; }
            set
            {
                if (value != _creditCard)
                {
                    _creditCard = value;
                    OnPropertyChanged(() => CreditCard);
                }
            }
        }

        private DelegateCommand _addCreditCardCommand;

        public DelegateCommand AddCreditCardCommand
        {
            get
            {
                return _addCreditCardCommand ?? (_addCreditCardCommand = new DelegateCommand(
                                                     () =>
                                                     {
                                                         if (!CreditCard.IsValid)
                                                         {
                                                             MessageBox.Show(CreditCard.Error);
                                                         }
                                                     }, () => CreditCard.IsValid));
            }
        }



         
    }

    public class CreditCardWithValidation : ValidatingObject
    {
        private string _creditCardNumber;
        private string _holderId;
        private string _CVV;
        private string _holderName;

        public CreditCardWithValidation()
        {
            //AddRule(new CreditCardValidationRule("CreditCardNumber", "אנא הזן מספר כרטיס אשראי תקין", o => CreditCardNumber, o => false));
            AddRule(new RequiredRule("HolderName", "אנא הזן שם מלא של בעל הכרטיס."));
            AddRule(new IsraelIdValidationRule("HolderId", "אנא הזן מספר תעודת זהות תקין", o => HolderId));
            AddRule(new CVVValidationRule("CVV", "אנא הזן קוד אבטחה תקין"));

        }


        public string CreditCardNumber
        {
            get { return _creditCardNumber; }
            set
            {
                if (value != _creditCardNumber)
                {
                    _creditCardNumber = value;
                    OnPropertyChanged(() => CreditCardNumber);

                }
            }
        }

        public string HolderId
        {
            get { return _holderId; }
            set
            {
                if (value != _holderId)
                {
                    _holderId = value;
                    OnPropertyChanged(() => HolderId);

                }
            }
        }

        public string CVV
        {
            get { return _CVV; }
            set
            {
                if (value != _CVV)
                {
                    _CVV = value;
                    OnPropertyChanged(() => CVV);

                }
            }
        }

        public string HolderName
        {
            get { return _holderName; }
            set
            {
                if (value != _holderName)
                {
                    _holderName = value;
                    OnPropertyChanged(() => HolderName);

                }
            }
        }

    }

    public class IsraelIdValidationRule : Rule
    {
        protected readonly Func<object, string> _getValue;
        protected int TargetLength;

        public IsraelIdValidationRule(string propertyName, string brokenDescription, Func<object, string> getValue)
            : base(propertyName, brokenDescription)
        {
            _getValue = getValue;
            TargetLength = 9;
        }

        protected override bool ValidateRule(object domainObject)
        {
            //Algirthm for Israeli Id validation
            //http://halemo.net/info/idcard/index.html
            try
            {

                var id = _getValue(domainObject);
                int sum = 0;
                if (string.IsNullOrEmpty(id)) return true;
                var fullId = id.PadLeft(TargetLength, '0');
                for (int index = 0, i = fullId.Length - 1; i >= 0; i--, index++)
                {
                    char c = fullId[i];
                    if (!char.IsDigit(c)) return true;
                    int digit = (int)char.GetNumericValue(c);
                    int weight = digit * ((index % 2 == 0) ? 1 : 2);
                    int sumOfdigit = 0;
                    while (weight != 0)
                    {
                        sumOfdigit += weight % 10;
                        weight /= 10;
                    }
                    sum += sumOfdigit;
                }
                return sum % 10 != 0;
            }
            catch (Exception e)
            {
                return true;
            }

        }
    }

    public class CreditCardValidationRule : IsraelIdValidationRule
    {
        private readonly Func<object, bool> _isIsracard;

        public CreditCardValidationRule(string propertyName, string brokenDescription, Func<object, string> getValue, Func<object, bool> isIsracard)
            : base(propertyName, brokenDescription, getValue)
        {
            _isIsracard = isIsracard;
        }

        protected override bool ValidateRule(object domainObject)
        {
            try
            {
                if (!_isIsracard(domainObject))
                {
                    // Same algohrithm as Id validation
                    var number = _getValue(domainObject);
                    if (string.IsNullOrEmpty(number)) return true;

                    TargetLength = number.Length;
                    return base.ValidateRule(domainObject);
                }
                // Algorithm for Isracard 9 digit credit card validation
                // http://halemo.net/info/isracard/index.html
                var id = _getValue(domainObject);
                var sum = 0;
                if (string.IsNullOrEmpty(id)) return true;
                var fullId = id.PadLeft(9, '0');
                for (int i = 0; i < fullId.Length; i++)
                {
                    var c = fullId[i];
                    if (!char.IsDigit(c)) return true;
                    var digit = (int)char.GetNumericValue(c);
                    sum += digit * (9 - i);

                }
                return sum % 11 != 0;
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
    public class CVVValidationRule : Rule
    {
        public CVVValidationRule(string propertyName, string brokenDescription)
            : base(propertyName, brokenDescription)
        {
        }

        protected override bool ValidateRule(object domainObject)
        {
            var domain = domainObject as CreditCardWithValidation;
            if (string.IsNullOrEmpty(domain.CVV)) return true;
            int number;
            // American Express
            return !(domain.CVV.Length == 3 && int.TryParse(domain.CVV, out number));
        }
    }
}