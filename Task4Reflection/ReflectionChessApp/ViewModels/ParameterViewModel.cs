using System;

namespace ReflectionChessApp.ViewModels
{
    public class ParameterViewModel : ObservableObject
    {
        private string _value = string.Empty;

        public string Name { get; }

        public Type ParameterType { get; }

        public string TypeName => ParameterType.Name;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public ParameterViewModel(string name, Type parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }
    }
}