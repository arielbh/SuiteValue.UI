using SuiteValue.UI.MVVM;
using SuiteValue.UI.WP8;

namespace WindowsPhoneSample.ViewModels
{
    public class TestLongListViewModel : NavigationViewModelBase
    {
        public TestLongListViewModel()
        {
            Employees = new Employee[]
            {
                new Employee {Name = "Ariel", },
                new Employee {Name = "Efrat"},
                new Employee {Name = "Margol", IsSelected = true},
                new Employee {Name = "Raz"},
            };
            SelectedEmployee = Employees[3];
        }
        private Employee[] _employees;

        public Employee[] Employees
        {
            get { return _employees; }
            set
            {
                if (value != _employees)
                {
                    _employees = value;
                    OnPropertyChanged(() => Employees);
                }
            }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (value != _selectedEmployee)
                {
                    _selectedEmployee = value;
                    OnPropertyChanged(() => SelectedEmployee);
                    if (SelectedEmployee != null)
                    {
                        SelectedEmployee.IsSelected = true;
                    }
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }
         
    }

    public class Employee : NotifyObject
    {
        public string Name { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }
    }
}