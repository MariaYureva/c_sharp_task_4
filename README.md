# Задание 4. Рефлексия — Летательные средства

WPF-приложение, демонстрирующее использование рефлексии для динамической загрузки библиотеки классов, создания объектов и вызова методов через графический интерфейс.

## Структура решения

```
task-4.sln
├── AircraftLibrary.Contracts   — интерфейс IAircraft и FlightEventArgs
├── AircraftLibrary             — реализация классов (Aircraft, Airplane, Helicopter)
└── task-4                      — WPF-приложение с рефлексией (MVVM)
```

### AircraftLibrary.Contracts

Общий контракт, на который ссылаются и библиотека, и WPF-приложение.

- **IAircraft** — интерфейс летательного средства (свойства `Name`, `Altitude`, `IsInFlight`; методы `TakeOff()`, `Land()`; событие `FlightStatusChanged`).
- **FlightEventArgs** — аргументы события с сообщением и текущей высотой.

### AircraftLibrary

Отдельный модуль (DLL), загружаемый динамически через рефлексию.

| Класс | Описание |
|---|---|
| `Aircraft` | Абстрактный базовый класс. Реализует `IAircraft`. Содержит виртуальный метод `GetFlightInfo()`. |
| `Airplane` | Самолёт. Требует длину взлётной полосы. Дополнительный метод `SetAvailableRunwayLength(double)`. |
| `Helicopter` | Вертолёт. Взлётная полоса не нужна. Дополнительный метод `HoverAtAltitude(double)`. |

### task-4 (WPF-приложение)

Оконное приложение на основе паттерна MVVM.

| Файл | Назначение |
|---|---|
| `MainViewModel.cs` | Загрузка сборки, поиск классов через рефлексию, создание объектов, вызов методов |
| `BaseViewModel.cs` | Базовый класс с `INotifyPropertyChanged` |
| `RelayCommand.cs` | Реализация `ICommand` |
| `ParameterInputViewModel.cs` | Модель для ввода параметров конструктора/метода |
| `MethodDisplayItem.cs` | Обёртка над `MethodInfo` для отображения в UI |
| `MainWindow.xaml` | Разметка главного окна |

## Сборка и запуск

```bash
dotnet build task-4.sln
dotnet run --project task-4
```

## Использование

1. Нажмите **«Обзор...»** и выберите файл `AircraftLibrary.dll`:
   ```
   AircraftLibrary\bin\Debug\net8.0\AircraftLibrary.dll
   ```
2. Нажмите **«Загрузить»** — в списке появятся классы `Airplane` и `Helicopter`.
3. Выберите класс — справа отобразится полная информация (конструкторы, свойства, методы, события).
4. Заполните параметры конструктора и нажмите **«Создать объект»**.
5. Выберите метод из выпадающего списка, введите параметры (если есть) и нажмите **«Выполнить»**.
6. Результаты и события полёта отображаются в журнале внизу окна.

## Применённые механизмы рефлексии

- `Assembly.LoadFrom()` — загрузка сборки по пути
- `assembly.GetTypes()` + `IsAssignableFrom()` — поиск классов, реализующих интерфейс
- `type.GetConstructors()`, `type.GetProperties()`, `type.GetMethods()`, `type.GetEvents()` — получение метаданных
- `ConstructorInfo.Invoke()` — создание объекта
- `MethodInfo.Invoke()` — вызов метода
- `Convert.ChangeType()` — преобразование строковых параметров в нужные типы

## Требования

- .NET 8.0 SDK
- Windows (WPF)
