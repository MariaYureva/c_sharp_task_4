using System.Reflection;

namespace ReflectionChessApp.ViewModels
{
    public class MethodViewModel
    {
        public MethodInfo MethodInfo { get; }

        public string DisplayName { get; }

        public MethodViewModel(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            ParameterInfo[] parameters = methodInfo.GetParameters();

            string parameterText = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));

            DisplayName = $"{methodInfo.Name}({parameterText}) : {methodInfo.ReturnType.Name}";
        }
    }
}