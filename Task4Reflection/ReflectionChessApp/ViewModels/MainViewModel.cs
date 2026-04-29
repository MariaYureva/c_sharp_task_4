using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ChessLibrary;

namespace ReflectionChessApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string _libraryPath = string.Empty;
        private string _result = string.Empty;
        private ClassViewModel? _selectedClass;
        private MethodViewModel? _selectedMethod;

        public ObservableCollection<ClassViewModel> Classes { get; }

        public ObservableCollection<MethodViewModel> Methods { get; }

        public ObservableCollection<ParameterViewModel> ConstructorParameters { get; }

        public ObservableCollection<ParameterViewModel> MethodParameters { get; }

        public ICommand LoadLibraryCommand { get; }

        public ICommand ExecuteMethodCommand { get; }

        public string LibraryPath
        {
            get => _libraryPath;
            set
            {
                _libraryPath = value;
                OnPropertyChanged();
            }
        }

        public ClassViewModel? SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();

                LoadClassData();
            }
        }

        public MethodViewModel? SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                _selectedMethod = value;
                OnPropertyChanged();

                LoadMethodParameters();
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Classes = new ObservableCollection<ClassViewModel>();
            Methods = new ObservableCollection<MethodViewModel>();
            ConstructorParameters = new ObservableCollection<ParameterViewModel>();
            MethodParameters = new ObservableCollection<ParameterViewModel>();

            LoadLibraryCommand = new RelayCommand(LoadLibrary);
            ExecuteMethodCommand = new RelayCommand(ExecuteSelectedMethod);

            Result = "Введите путь к DLL-библиотеке и нажмите кнопку загрузки.";
        }

        private void LoadLibrary()
        {
            try
            {
                Classes.Clear();
                Methods.Clear();
                ConstructorParameters.Clear();
                MethodParameters.Clear();

                Assembly assembly = Assembly.LoadFrom(LibraryPath);

                Type interfaceType = typeof(IChessPiece);

                Type[] foundTypes = assembly
                    .GetTypes()
                    .Where(type =>
                        type.IsClass &&
                        !type.IsAbstract &&
                        interfaceType.IsAssignableFrom(type))
                    .ToArray();

                foreach (Type type in foundTypes)
                {
                    Classes.Add(new ClassViewModel(type));
                }

                Result = $"Библиотека загружена. Найдено классов: {Classes.Count}.";
            }
            catch (Exception ex)
            {
                Result = $"Ошибка загрузки библиотеки: {ex.Message}";
            }
        }

        private void LoadClassData()
        {
            Methods.Clear();
            ConstructorParameters.Clear();
            MethodParameters.Clear();

            if (SelectedClass == null)
            {
                return;
            }

            ConstructorInfo? constructor = GetSelectedConstructor();

            if (constructor != null)
            {
                foreach (ParameterInfo parameter in constructor.GetParameters())
                {
                    ConstructorParameters.Add(new ParameterViewModel(
                        parameter.Name ?? "parameter",
                        parameter.ParameterType));
                }
            }

            MethodInfo[] methods = SelectedClass.Type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method =>
                    !method.IsSpecialName &&
                    method.DeclaringType != typeof(object))
                .ToArray();

            foreach (MethodInfo method in methods)
            {
                Methods.Add(new MethodViewModel(method));
            }

            Result = $"Выбран класс: {SelectedClass.Name}. Методов найдено: {Methods.Count}.";
        }

        private void LoadMethodParameters()
        {
            MethodParameters.Clear();

            if (SelectedMethod == null)
            {
                return;
            }

            foreach (ParameterInfo parameter in SelectedMethod.MethodInfo.GetParameters())
            {
                MethodParameters.Add(new ParameterViewModel(
                    parameter.Name ?? "parameter",
                    parameter.ParameterType));
            }
        }

        private void ExecuteSelectedMethod()
        {
            try
            {
                if (SelectedClass == null)
                {
                    Result = "Класс не выбран.";
                    return;
                }

                if (SelectedMethod == null)
                {
                    Result = "Метод не выбран.";
                    return;
                }

                ConstructorInfo? constructor = GetSelectedConstructor();

                if (constructor == null)
                {
                    Result = "У выбранного класса нет публичного конструктора.";
                    return;
                }

                object?[] constructorValues = ConstructorParameters
                    .Select(parameter => ConvertValue(parameter.Value, parameter.ParameterType))
                    .ToArray();

                object? instance = constructor.Invoke(constructorValues);

                object?[] methodValues = MethodParameters
                    .Select(parameter => ConvertValue(parameter.Value, parameter.ParameterType))
                    .ToArray();

                object? methodResult = SelectedMethod.MethodInfo.Invoke(instance, methodValues);

                string objectInfo = instance is IChessPiece piece
                    ? piece.GetInfo()
                    : instance?.ToString() ?? string.Empty;

                Result =
                    $"Класс: {SelectedClass.Name}\n" +
                    $"Метод: {SelectedMethod.MethodInfo.Name}\n" +
                    $"Результат метода: {methodResult ?? "void"}\n" +
                    $"Состояние объекта: {objectInfo}";
            }
            catch (Exception ex)
            {
                Result = $"Ошибка выполнения метода: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        private ConstructorInfo? GetSelectedConstructor()
        {
            return SelectedClass?.Type
                .GetConstructors()
                .OrderByDescending(constructor => constructor.GetParameters().Length)
                .FirstOrDefault();
        }

        private object? ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType == typeof(int))
            {
                return int.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(double))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(bool))
            {
                return bool.Parse(value);
            }

            throw new NotSupportedException($"Тип {targetType.Name} не поддерживается.");
        }
    }
}