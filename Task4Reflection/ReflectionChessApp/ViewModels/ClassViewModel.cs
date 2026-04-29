using System;
using System.Reflection;

namespace ReflectionChessApp.ViewModels
{
    public class ClassViewModel
    {
        public Type Type { get; }

        public string Name => Type.Name;

        public string FullName => Type.FullName ?? Type.Name;

        public ClassViewModel(Type type)
        {
            Type = type;
        }
    }
}