using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using static Configurator.Data;
using System.Linq.Expressions;
using System;
using System.Data;
using System.Reflection;
using System.IO;
using System.Text;
using System.Globalization;
using System.Net.Sockets;
using System.Windows.Media.Media3D;


namespace Configurator
{
    public partial class MainWindow : Window
    {
        public ComputerComponent[] computerComponent = new ComputerComponent[8];
        int currentComponent;
        public static MainWindow main { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            main = this;
        }

        private void CurrentComponent(object sender)
        {
            if (sender == AddProcessorName)
                currentComponent = 0;
            else if (sender == AddGPUName)
                currentComponent = 1;
            else if (sender == AddRAMName)
                currentComponent = 2;
            else if (sender == AddMotherboardName)
                currentComponent = 3;
            else if (sender == AddStorageDrivesName)
                currentComponent = 4;
            else if (sender == AddPowerSupplyUnitName)
                currentComponent = 5;
            else if (sender == AddCaseName)
                currentComponent = 6;
            else if (sender == AddCoolingSystemName)
                currentComponent = 7;
        }//
        private void AddComponentNameAndFilters_Click(object sender, RoutedEventArgs e)
        {
            Exit.Visibility = Visibility.Visible;
            CurrentComponent(sender);
            CreatComponentName(sender);
            CreateComboBoxFilters(sender);
        }
        private void AddComponent_Click(object sender, RoutedEventArgs e)
        {
            PanelChoiceElement.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;

            int index = listBox.Items.IndexOf((sender as Button).DataContext);
            if (currentComponent == 0)
            {
                Processor p = (Processor)listBox.Items[index];
                ProcessorName.Text = p.FullName;
                ProcessorPrice.Text = p.Price.ToString();
                computerComponent[0] = p;
            }
            else if (currentComponent == 1)
            {
                GPU p = (GPU)listBox.Items[index];
                GPUName.Text = p.FullName;
                GPUPrice.Text = p.Price.ToString();
                computerComponent[1] = p;
            }
            else if (currentComponent == 2)
            {
                RAM p = (RAM)listBox.Items[index];
                RAMName.Text = p.FullName;
                RAMPrice.Text = p.Price.ToString();
                computerComponent[2] = p;
            }
            else if (currentComponent == 3)
            {
                Motherboard p = (Motherboard)listBox.Items[index];
                MotherboardName.Text = p.FullName;
                MotherboardPrice.Text = p.Price.ToString();
                computerComponent[3] = p;
            }
            else if (currentComponent == 4)
            {
                StorageDrives p = (StorageDrives)listBox.Items[index];
                StorageDrivesName.Text = p.FullName;
                StorageDrivesPrice.Text = p.Price.ToString();
                computerComponent[4] = p;
            }
            else if (currentComponent == 5)
            {
                PowerSupplyUnit p = (PowerSupplyUnit)listBox.Items[index];
                PowerSupplyUnitName.Text = p.FullName;
                PowerSupplyUnitPrice.Text = p.Price.ToString();
                computerComponent[5] = p;
                UpdatePower();
            }
            else if (currentComponent == 6)
            {
                Case p = (Case)listBox.Items[index];
                CaseName.Text = p.FullName;
                CasePrice.Text = p.Price.ToString();
                computerComponent[6] = p;
            }
            else if (currentComponent == 7)
            {
                CoolingSystem p = (CoolingSystem)listBox.Items[index];
                CoolingSystemName.Text = p.FullName;
                CoolingSystemPrice.Text = p.Price.ToString();
                computerComponent[7] = p;
            }
            UpdatePrice();
            UpdateConsumption();
            Filters.Items.Clear();
            listBox.Items.Clear();
            CompatibilityChecker_Click();
        }//
        private static void CreatComponentName(object sender)
        {
            if (sender == main.AddProcessorName)
                CreatComponentName(DataProcessor);
            else if (sender == main.AddGPUName)
                CreatComponentName(DataGPU);
            else if (sender == main.AddRAMName)
                CreatComponentName(DataRAM);
            else if (sender == main.AddMotherboardName)
                CreatComponentName(DataMotherboard);
            else if (sender == main.AddStorageDrivesName)
                CreatComponentName(DataStorageDrives);
            else if (sender == main.AddPowerSupplyUnitName)
                CreatComponentName(DataPowerSupplyUnit);
            else if (sender == main.AddCaseName)
                CreatComponentName(DataCase);
            else if (sender == main.AddCoolingSystemName)
                CreatComponentName(DataCoolingSystem);
        }//
        public static void CreatComponentName<T>(T[] CurrentDataComponent)
        {
            main.PanelChoiceElement.Visibility = Visibility.Visible;
            for (int i = 0; i < CurrentDataComponent.Length; i++)
            {
                main.listBox.Items.Add(CurrentDataComponent[i]);
            }
        }
        public void CreateComboBoxFilters(object sender)
        {
            if (sender == AddProcessorName)
                CreateComboBoxFilters(sender, nameFilterProcessor);
            else if (sender == AddGPUName)
                CreateComboBoxFilters(sender, nameFilterGPU);
            else if (sender == AddRAMName)
                CreateComboBoxFilters(sender, nameFilterRAM);
            else if (sender == AddMotherboardName)
                CreateComboBoxFilters(sender, nameFilterMotherboard);
            else if (sender == AddStorageDrivesName)
                CreateComboBoxFilters(sender, nameFilterStorageDrives);
            else if (sender == AddPowerSupplyUnitName)
                CreateComboBoxFilters(sender, nameFilterPowerSupplyUnit);
            else if (sender == AddCaseName)
                CreateComboBoxFilters(sender, nameFilterCase);
            else if (sender == AddCoolingSystemName)
                CreateComboBoxFilters(sender, nameFilterCoolingSystem);
        }//
        private void CreateComboBoxFilters(object sender, string[] nameFilterComponent)
        {
            List<List<string>> temp = NumberOfFilters(sender);
            for (int i = 0; i < nameFilterComponent.Length; i++)
            {
                ComboBox comboBox = new ComboBox()
                {
                    Name = ($"NameFilters{i + 1}").ToString(),
                    Width = 286,
                    Height = 40,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness()
                };
                Filters.Items.Add(comboBox);

                TextBlock textBlock = new TextBlock()
                {
                    Text = nameFilterComponent[i],
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 270,
                    Height = 40,
                    Margin = new Thickness(0, 0, 0, 0)
                };
                comboBox.SelectedIndex = 0;
                comboBox.Items.Add(textBlock);


                for (int j = 0; j < temp[i].Count; j++)
                {
                    CheckBox checkBox = new CheckBox()
                    {
                        Content = temp[i][j],
                        Width = 270,
                        Height = 40,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    comboBox.Items.Add(checkBox);
                }
            }
        }
        private List<List<string>> NumberOfFilters(object sender)
        {
            if (sender == AddProcessorName)
                return NumberOfFilters(nameFilter_ProcessorField, Data.DataProcessor);
            else if (sender == AddGPUName)
                return NumberOfFilters(nameFilter_GPUFild, Data.DataGPU);
            else if (sender == AddRAMName)
                return NumberOfFilters(nameFilter_RAMFild, Data.DataRAM);
            else if (sender == AddMotherboardName)
                return NumberOfFilters(nameFilter_MotherboardFild, Data.DataMotherboard);
            else if (sender == AddStorageDrivesName)
                return NumberOfFilters(nameFilter_StorageDrivesFild, Data.DataStorageDrives);
            else if (sender == AddPowerSupplyUnitName)
                return NumberOfFilters(nameFilter_PowerSupplyUnitFild, Data.DataPowerSupplyUnit);
            else if (sender == AddCaseName)
                return NumberOfFilters(nameFilter_CaseFild, Data.DataCase);
            else if (sender == AddCoolingSystemName)
                return NumberOfFilters(nameFilter_CoolingSystemFild, Data.DataCoolingSystem);
            return new List<List<string>>();
        }//
        private List<List<string>> NumberOfFilters<T>(string[] nameFilterField, T[] DataComponent)
        {
            List<List<string>> results = new List<List<string>>();

            foreach (string fieldName in nameFilterField)
            {
                List<string> fieldValues = new List<string>();
                foreach (var component in DataComponent)
                {
                    Type componentType = component.GetType();
                    FieldInfo field = componentType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        object value = field.GetValue(component);
                        fieldValues.Add(value.ToString());
                    }
                    else
                    {
                        PropertyInfo property = componentType.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (property != null)
                        {
                            object value = property.GetValue(component);
                            fieldValues.Add(value.ToString());
                        }
                    }
                }

                bool isNumeric = fieldValues.All(item => int.TryParse(item, out _));

                if (isNumeric)
                {
                    List<int> intValues = fieldValues.Select(int.Parse).ToList();
                    intValues.Sort();
                    fieldValues = intValues.Select(i => i.ToString()).ToList();
                }
                else
                {
                    fieldValues.Sort();
                }
                results.Add(fieldValues.Distinct().ToList());
            }
            return results;
        }

        private void UpdatePower()
        {
            PowerSupplyUnit power = (PowerSupplyUnit)computerComponent[5];
            GeneralPower.Content = power.Wattage + "Вт";
        }
        private void UpdatePrice()
        {
            int sumPrice;
            int.TryParse(ProcessorPrice.Text, out int a0);
            int.TryParse(GPUPrice.Text, out int a1);
            int.TryParse(MotherboardPrice.Text, out int a2);
            int.TryParse(RAMPrice.Text, out int a3);
            int.TryParse(StorageDrivesPrice.Text, out int a4);
            int.TryParse(PowerSupplyUnitPrice.Text, out int a5);
            int.TryParse(CasePrice.Text, out int a6);
            int.TryParse(CoolingSystemPrice.Text, out int a7); ;
            sumPrice = a0 + a1 + a2 + a3 + a4 + a5 + a6 + a7;
            GeneralPrice.Content = sumPrice.ToString();
        }
        private void UpdateConsumption()
        {
            int sumConsumption = 0;
            int value = 0;
            for (int i = 0; i < computerComponent.Length; i++)
            {
                if (computerComponent[i] == null)
                    continue;
                string fieldName = "Consumption";

                Type type = computerComponent[i].GetType();
                PropertyInfo field = type.GetProperty(fieldName);
                if (field != null)
                    value = Convert.ToInt16(field.GetValue(computerComponent[i]));
                sumConsumption += value;
                value = 0;
            }
            GeneralConsumption.Content = sumConsumption.ToString() + " Вт";
        }

        private void ApplyFilters_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter(main);
            filter.Solve(currentComponent);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Exit.Visibility = Visibility.Hidden;
            PanelChoiceElement.Visibility = Visibility.Hidden;
            listBox.Items.Clear();
            Filters.Items.Clear();
        }
        private void CompatibilityChecker_Click()
        {
            new CompatibilityChecker(main).CheckCompatibility(computerComponent);
        }
    }

    public class CompatibilityChecker
    {
        MainWindow main;
        public CompatibilityChecker(MainWindow main)
        {
            this.main = main;
        }
        public void CheckCompatibility(ComputerComponent[] computerComponent)
        {
            ComputerComponent component1, component2;
            List<bool> component = new List<bool>() { true, true, true, true, true, true, true, true, true, true };
            ComputerComponent[] components = new ComputerComponent[]
            { computerComponent[0], computerComponent[2], computerComponent[3], computerComponent[6], computerComponent[7] };
            int a = 0;
            for (int i = 0; i < components.Length; i++)
            {
                component1 = components[i];
                for (int j = i + 1; j < components.Length; j++)
                {
                    component2 = components[j];
                    if (component1 != null && component2 != null && (component1 != component2))
                    {
                        component[a] = (AreComponentsCompatible(component1, component2));
                    }
                    a++;
                }
            }
            //0 проц оперативка  +
            //1 проц мать        +
            //2 проц - корпус    -
            //3 проц - кулер     +
            //4 оперативка мать  +
            //5 оперативка корпус-
            //6 оперативка кулер -
            //7 мать корпус      +
            //8 мать кулер       +
            //9 корпус кулер     +
            List<bool> bools = new List<bool>();
            for (int i = 0; i < component.Count; i++)
            {
                if (i != 2 && i != 5 && i != 6)
                {
                    bools.Add(component[i]);
                }
            }
            //0 проц оперативка  +
            //0 проц мать        +
            //1 проц - кулер     +
            //2 оперативак мать  +
            //3 мать корпус      +
            //4 мать кулер       +
            //5 корпус кулер     +
            if (computerComponent[0] != null && computerComponent[2] != null) //проц оперативка 
            {
                SetComponentColor(computerComponent[0], bools[0]);
                SetComponentColor(computerComponent[2], bools[0]);
            }

            if (computerComponent[0] != null && computerComponent[3] != null) //проц мать 
            {
                if (bools[0])
                {
                    SetComponentColor(computerComponent[0], bools[1]);
                }
                SetComponentColor(computerComponent[3], bools[1]);
            }

            if (computerComponent[0] != null && computerComponent[7] != null)//проц - кулер
            {
                if (bools[1])
                {
                    SetComponentColor(computerComponent[0], bools[2]);
                }
                SetComponentColor(computerComponent[7], bools[2]);
            }

            if (computerComponent[2] != null && computerComponent[3] != null)//оперативка мать
            {
                if (bools[0])
                {
                    SetComponentColor(computerComponent[2], bools[3]);//оперативка
                }
                if (bools[1])
                {
                    SetComponentColor(computerComponent[3], bools[3]);
                }
            }

            if (computerComponent[3] != null && computerComponent[6] != null)//мать корпус
            {
                if (bools[3])
                {
                    SetComponentColor(computerComponent[3], bools[4]);
                }
                SetComponentColor(computerComponent[6], bools[4]);//корпус
            }

            if (computerComponent[3] != null && computerComponent[7] != null)//мать кулер
            {
                if (bools[4])
                {
                    SetComponentColor(computerComponent[3], bools[5]);
                }
                if (bools[2])
                {
                    SetComponentColor(computerComponent[7], bools[5]);
                }
            }

            if (computerComponent[6] != null && computerComponent[7] != null)//корпус кулер
            {
                if (bools[4])
                {
                    SetComponentColor(computerComponent[6], bools[6]);
                }
                if (bools[5])
                {
                    SetComponentColor(computerComponent[7], bools[6]);
                }
            }
        }
        private bool AreComponentsCompatible(ComputerComponent component1, ComputerComponent component2)
        {

            var properties1 = component1.GetType().GetProperties();
            var properties2 = component2.GetType().GetProperties();

            var fieldsToCheck = new List<string> { "Socket", "MemoryType", "FormFactor" };

            foreach (var property1 in properties1)
            {
                if (!fieldsToCheck.Contains(property1.Name))
                    continue;

                var propValue1 = property1.GetValue(component1);

                foreach (var property2 in properties2)
                {
                    var propName2 = property2.Name;
                    if (!fieldsToCheck.Contains(propName2))
                        continue;

                    var propValue2 = property2.GetValue(component2);

                    if (property1.Name == propName2 && propValue1 != null && propValue2 != null && !propValue1.Equals(propValue2))
                        return false;
                }
            }
            return true;
        }
        private void SetComponentColor(ComputerComponent component, bool b)
        {

            Color[] colors =
            {
                new Color() { A = 255, R = 225, G = 225, B = 225 }, //true
                new Color() { A = 255, R = 192, G = 194, B = 36 }   //false
            };
            Color color;
            if (b)
                color = colors[0];
            else
                color = colors[1];

            var brush = new SolidColorBrush(color);

            if (component is Processor)
            {
                main.ProcessorName.Background = brush;
            }
            else if (component is Motherboard)
            {
                main.MotherboardName.Background = brush;
            }
            else if (component is RAM)
            {
                main.RAMName.Background = brush;
            }
            else if (component is Case)
            {
                main.CaseName.Background = brush;
            }
            else if (component is CoolingSystem)
            {
                main.CoolingSystemName.Background = brush;
            }
        }
    }
    public class Filter
    {
        MainWindow main;
        public Filter(MainWindow main)
        {
            this.main = main;
        }

        public void Solve(int currentComponent)
        {
            switch (currentComponent)
            {
                case 0:
                    ApplyFilters<Processor>();
                    break;
                case 1:
                    ApplyFilters<GPU>();
                    break;
                case 2:
                    ApplyFilters<RAM>();
                    break;
                case 3:
                    ApplyFilters<Motherboard>();
                    break;
                case 4:
                    ApplyFilters<StorageDrives>();
                    break;
                case 5:
                    ApplyFilters<PowerSupplyUnit>();
                    break;
                case 6:
                    ApplyFilters<Case>();
                    break;
                case 7:
                    ApplyFilters<CoolingSystem>();
                    break;
            }
            //return main;
        }
        private void ApplyFilters<T>()
        {
            List<List<string>> selectedCheckBoxValues;

            selectedCheckBoxValues = GetSelectedCheckBoxValues();
            List<Expression<Func<T, bool>>> filtersList = new List<Expression<Func<T, bool>>>();

            int i = 0;
            foreach (var checkBoxValues in selectedCheckBoxValues)
            {
                filtersList.Add(CreateFilter<T>(checkBoxValues, GetnameFilterComponent<T>(i)));
                i++;
            }
            selectedCheckBoxValues.Add(new List<string> { main.MinPrice.Text });
            selectedCheckBoxValues.Add(new List<string> { main.MaxPrice.Text });

            filtersList.AddRange(CreateFiltersPrice<T>("Price", int.Parse(main.MinPrice.Text), int.Parse(main.MaxPrice.Text)));

            List<Expression<Func<T, bool>>> filters = filtersList;
            Filtration<T>(filters);
        }
        private string GetnameFilterComponent<T>(int i)
        {
            if (typeof(Processor) == typeof(T))
            {
                return nameFilter_ProcessorField[i];
            }
            else if (typeof(GPU) == typeof(T))
            {
                return nameFilter_GPUFild[i];
            }
            else if (typeof(Motherboard) == typeof(T))
            {
                return nameFilter_MotherboardFild[i];
            }
            else if (typeof(RAM) == typeof(T))
            {
                return nameFilter_RAMFild[i];
            }
            else if (typeof(StorageDrives) == typeof(T))
            {
                return nameFilter_StorageDrivesFild[i];
            }
            else if (typeof(PowerSupplyUnit) == typeof(T))
            {
                return nameFilter_PowerSupplyUnitFild[i];
            }
            else if (typeof(Case) == typeof(T))
            {
                return nameFilter_CaseFild[i];
            }
            else if (typeof(CoolingSystem) == typeof(T))
            {
                return nameFilter_CoolingSystemFild[i];
            }
            return "";
        }//
        private void Filtration<T>(List<Expression<Func<T, bool>>> filters)
        {
            if (typeof(Processor) == typeof(T))
            {
                CurrentDataProcessor = FilterItems<Processor>(Data.DataProcessor.ToList(), filters.Cast<Expression<Func<Processor, bool>>>().ToList()).Cast<Processor>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(Data.CurrentDataProcessor);
            }
            else if (typeof(GPU) == typeof(T))
            {
                CurrentDataGPU = FilterItems<GPU>(Data.DataGPU.ToList(), filters.Cast<Expression<Func<GPU, bool>>>().ToList()).Cast<GPU>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(Data.CurrentDataGPU);
            }
            else if (typeof(Motherboard) == typeof(T))
            {
                CurrentDataMotherboard = FilterItems<Motherboard>(DataMotherboard.ToList(), filters.Cast<Expression<Func<Motherboard, bool>>>().ToList()).Cast<Motherboard>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataMotherboard);
            }
            else if (typeof(RAM) == typeof(T))
            {
                CurrentDataRAM = FilterItems<RAM>(DataRAM.ToList(), filters.Cast<Expression<Func<RAM, bool>>>().ToList()).Cast<RAM>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataRAM);
            }
            else if (typeof(StorageDrives) == typeof(T))
            {
                CurrentDataStorageDrives = FilterItems<StorageDrives>(DataStorageDrives.ToList(), filters.Cast<Expression<Func<StorageDrives, bool>>>().ToList()).Cast<StorageDrives>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataStorageDrives);
            }
            else if (typeof(PowerSupplyUnit) == typeof(T))
            {
                CurrentDataPowerSupplyUnit = FilterItems<PowerSupplyUnit>(DataPowerSupplyUnit.ToList(), filters.Cast<Expression<Func<PowerSupplyUnit, bool>>>().ToList()).Cast<PowerSupplyUnit>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataPowerSupplyUnit);
            }
            else if (typeof(Case) == typeof(T))
            {
                CurrentDataCase = FilterItems<Case>(DataCase.ToList(), filters.Cast<Expression<Func<Case, bool>>>().ToList()).Cast<Case>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataCase);
            }
            else if (typeof(CoolingSystem) == typeof(T))
            {
                CurrentDataCoolingSystem = FilterItems<CoolingSystem>(DataCoolingSystem.ToList(), filters.Cast<Expression<Func<CoolingSystem, bool>>>().ToList()).Cast<CoolingSystem>().ToArray();
                main.listBox.Items.Clear();
                MainWindow.CreatComponentName(CurrentDataCoolingSystem);
            }
        }//
        List<T> FilterItems<T>(List<T> items, List<Expression<Func<T, bool>>> filters)
        {
            IQueryable<T> query = items.AsQueryable();
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }
        private List<List<string>> GetSelectedCheckBoxValues()
        {
            List<List<string>> selectedValues = new List<List<string>>();
            foreach (ComboBox comboBox in MainWindow.main.Filters.Items)
            {
                List<string> comboBoxSelectedValues = new List<string>();

                foreach (var item in comboBox.Items)
                {
                    if (item is CheckBox checkBox && checkBox.IsChecked == true)
                    {
                        comboBoxSelectedValues.Add(checkBox.Content.ToString());
                    }
                }
                selectedValues.Add(comboBoxSelectedValues);
            }
            return selectedValues;
        }
        private static Expression<Func<T, bool>> CreateFilter<T>(List<string> checkBoxValues, string parametr)
        {
            if (checkBoxValues == null || checkBoxValues.Count == 0)
            {
                return p => true;
            }
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), typeof(T).ToString());
            System.Linq.Expressions.Expression body = null;
            foreach (var value in checkBoxValues)
            {
                System.Linq.Expressions.Expression condition;
                if (int.TryParse(value, out int intNumber))
                {
                    // Якщо рядок може бути конвертований в int, використовуємо int
                    condition = System.Linq.Expressions.Expression.Equal(
                        System.Linq.Expressions.Expression.Property(param, parametr),
                        System.Linq.Expressions.Expression.Constant(intNumber, typeof(int))
                    );
                }
                else if (double.TryParse(value, out double doubleNumber))
                {
                    // Якщо рядок може бути конвертований в double, використовуємо double
                    condition = System.Linq.Expressions.Expression.Equal(
                        System.Linq.Expressions.Expression.Property(param, parametr),
                        System.Linq.Expressions.Expression.Constant(doubleNumber, typeof(double))
                    );
                }
                else
                {
                    // Якщо рядок не може бути конвертований ні в double, ні в int, використовуємо рядок
                    condition = System.Linq.Expressions.Expression.Equal(
                        System.Linq.Expressions.Expression.Property(param, parametr),
                        System.Linq.Expressions.Expression.Constant(value, typeof(string))
                    );
                }

                body = body == null ? condition : System.Linq.Expressions.Expression.Or(body, condition);
            }
            if (body == null)
            {
                return p => true;
            }
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);
            return lambda;
        }
        private static Expression<Func<T, bool>>[] CreateFiltersPrice<T>(string propertyName, int minValue, int maxValue)
        {
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), "processor");
            MemberExpression propertyExpression = System.Linq.Expressions.Expression.Property(param, propertyName);

            BinaryExpression minConditionExpression = System.Linq.Expressions.Expression.GreaterThan(propertyExpression, System.Linq.Expressions.Expression.Constant(minValue));
            BinaryExpression maxConditionExpression = System.Linq.Expressions.Expression.LessThan(propertyExpression, System.Linq.Expressions.Expression.Constant(maxValue));

            Expression<Func<T, bool>> minLambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(minConditionExpression, param);
            Expression<Func<T, bool>> maxLambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(maxConditionExpression, param);

            return new Expression<Func<T, bool>>[] { minLambda, maxLambda };
        }
    }
    public static class Data
    {
        static public Processor[] DataProcessor;
        static public Processor[] CurrentDataProcessor;
        public static string[] nameFilterProcessor = new string[] { "Виробник", "Споживання", "Сокет", "Тип пам'яті", "Призначення", "Частота (номінальна)", "Частота (максимальна)" };
        public static string[] nameFilter_ProcessorField = new string[] { "Brand", "Consumption", "Socket", "MemoryType", "Purpose", "ClockSpeedNominal", "ClockSpeedMax", "MinPrice", "MaxPrice" };//костиль

        static public GPU[] DataGPU;
        static public GPU[] CurrentDataGPU;
        public static string[] nameFilterGPU = new string[] { "Виробник", "Призначення", "Тип пам'яті", "Обьєм памяті", "Частота" };
        public static string[] nameFilter_GPUFild = new string[] { "Brand", "Purpose", "MemoryType", "SizeGB", "SpeedMHz", "MinPrice", "MaxPrice" };

        static public RAM[] DataRAM;
        static public RAM[] CurrentDataRAM;
        public static string[] nameFilterRAM = new string[] { "Виробник", "Тип пам'яті", "Обьєм пам'яті", "Частота" };
        public static string[] nameFilter_RAMFild = new string[] { "Brand", "MemoryType", "SizeGB", "SpeedMHz", "MinPrice", "MaxPrice" };

        static public Motherboard[] DataMotherboard;
        static public Motherboard[] CurrentDataMotherboard;
        public static string[] nameFilterMotherboard = new string[] { "Виробник", "Призначення", "Сокет", "Чіпсет", "Форм фактор", "Тип пам'яті" };
        public static string[] nameFilter_MotherboardFild = new string[] { "Brand", "Purpose", "Socket", "Сhipset", "FormFactor", "MemoryType", "MinPrice", "MaxPrice" };

        static public StorageDrives[] DataStorageDrives;
        static public StorageDrives[] CurrentDataStorageDrives;
        public static string[] nameFilterStorageDrives = new string[] { "Виробник", "Тип накопичувача", "Тип пам'яті" };
        public static string[] nameFilter_StorageDrivesFild = new string[] { "Brand", "Type", "MemoryType", "MinPrice", "MaxPrice" };

        static public PowerSupplyUnit[] DataPowerSupplyUnit;
        static public PowerSupplyUnit[] CurrentDataPowerSupplyUnit;
        public static string[] nameFilterPowerSupplyUnit = new string[] { "Виробник", "Кількість ват", "Ефективність" };
        public static string[] nameFilter_PowerSupplyUnitFild = new string[] { "Brand", "Wattage", "Productivity", "MinPrice", "MaxPrice" };

        static public Case[] DataCase;
        static public Case[] CurrentDataCase;
        public static string[] nameFilterCase = new string[] { "Виробник", "Форм фактор" };
        public static string[] nameFilter_CaseFild = new string[] { "Brand", "FormFactor", "MinPrice", "MaxPrice" };

        static public CoolingSystem[] DataCoolingSystem;
        static public CoolingSystem[] CurrentDataCoolingSystem;

        public static string[] nameFilterCoolingSystem = new string[] { "Виробник", "Сокет", "Максимальний ТДП", "Висота" };
        public static string[] nameFilter_CoolingSystemFild = new string[] { "Brand", "Socket", "MaxTDP", "Height", "MinPrice", "MaxPrice" };

        static Data()
        {
            new ReadDateBase().CreateData();
        }
        class ReadDateBase
        {
            List<string> processorData = new List<string>();
            List<string> gpuData = new List<string>();
            List<string> motherboardData = new List<string>();
            List<string> ramData = new List<string>();
            List<string> storageDriveData = new List<string>();
            List<string> powerSupplyUnitData = new List<string>();
            List<string> caseData = new List<string>();
            List<string> coolingSystemData = new List<string>();
            public void CreateData()
            {
                string data1 = ReadDBToString();
                BreakdownByType(data1);
                List<Processor> processors = processorData.Select(data => ParseProcessor(data)).ToList();
                List<GPU> gpu = gpuData.Select(data => ParseGPU(data)).ToList();
                List<Motherboard> motherboard = motherboardData.Select(data => ParseMotherboard(data)).ToList();
                List<RAM> ram = ramData.Select(data => ParseRAM(data)).ToList();
                List<StorageDrives> storageDrive = storageDriveData.Select(data => ParseStorageDrives(data)).ToList();
                List<PowerSupplyUnit> powerSupplyUnit = powerSupplyUnitData.Select(data => ParsePowerSupplyUnit(data)).ToList();
                List<Case> _case = caseData.Select(data => ParseCase(data)).ToList();
                List<CoolingSystem> coolingSystem = coolingSystemData.Select(data => ParseCoolingSystem(data)).ToList();

                DataProcessor = processors.ToArray();
                DataGPU = gpu.ToArray();
                DataMotherboard = motherboard.ToArray();
                DataRAM = ram.ToArray();
                DataStorageDrives = storageDrive.ToArray();
                DataPowerSupplyUnit = powerSupplyUnit.ToArray();
                DataCase = _case.ToArray();
                DataCoolingSystem = coolingSystem.ToArray();

                CurrentDataProcessor = DataProcessor;
                CurrentDataGPU = DataGPU;
                CurrentDataMotherboard = DataMotherboard;
                CurrentDataRAM = DataRAM;
                CurrentDataStorageDrives = DataStorageDrives;
                CurrentDataPowerSupplyUnit = DataPowerSupplyUnit;
                CurrentDataCase = DataCase;
                CurrentDataCoolingSystem = DataCoolingSystem;

            }
            private string ReadDBToString()
            {
                string path = "DataBase.txt";

                StringBuilder sb = new StringBuilder();

                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
                return sb.ToString();
            }
            private void BreakdownByType(string data)
            {
                var sections = data.Split(new[] { "Processor", "GPU", "Motherboard", "RAM", "StorageDrives", "PowerSupplyUnit", "Case", "CoolingSystem" }, StringSplitOptions.RemoveEmptyEntries);
                var type = 0;
                foreach (var section in sections)
                {
                    var lines = section.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

                    if (lines.Count == 0)
                        continue;

                    switch (type)
                    {
                        //"Processor"
                        case 0:
                            processorData.AddRange(lines);
                            break;
                        //"GPU"
                        case 1:
                            gpuData.AddRange(lines);
                            break;
                        //"Motherboard"
                        case 2:
                            motherboardData.AddRange(lines);
                            break;
                        //"RAM"
                        case 3:
                            ramData.AddRange(lines);
                            break;
                        //"StorageDrives"
                        case 4:
                            storageDriveData.AddRange(lines);
                            break;
                        //"PowerSupplyUnit"
                        case 5:
                            powerSupplyUnitData.AddRange(lines);
                            break;
                        //"Case"
                        case 6:
                            caseData.AddRange(lines);
                            break;
                        //"CoolingSystem"
                        case 7:
                            coolingSystemData.AddRange(lines);
                            break;
                    }
                    type++;
                }
            }
            private Processor ParseProcessor(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();

                return new Processor(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    parts[4],
                    double.Parse(parts[5], CultureInfo.InvariantCulture),
                    double.Parse(parts[6], CultureInfo.InvariantCulture),
                    parts[7],
                    int.Parse(parts[8]),
                    int.Parse(parts[9]),
                    parts[10]
                );
            }
            GPU ParseGPU(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new GPU(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    parts[4],
                    parts[5],
                    int.Parse(parts[6]),
                    int.Parse(parts[7])
                );
            }
            Motherboard ParseMotherboard(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new Motherboard(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7]
                );
            }
            RAM ParseRAM(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new RAM(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    int.Parse(parts[4]),
                    int.Parse(parts[5])
                );
            }
            StorageDrives ParseStorageDrives(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new StorageDrives(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    int.Parse(parts[4])
                );
            }
            PowerSupplyUnit ParsePowerSupplyUnit(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new PowerSupplyUnit(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    int.Parse(parts[4])
                );
            }
            Case ParseCase(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new Case(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    int.Parse(parts[4])
                );
            }
            CoolingSystem ParseCoolingSystem(string data)
            {
                string[] parts = data.Split(',').Select(p => p.Trim('\"')).ToArray();
                return new CoolingSystem(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    parts[4],
                    int.Parse(parts[5])
                );
            }
        }
    }

    public class Processor : ComputerComponent, IPowerConsumption
    {
        public Processor(int price, string brand, string model, int consumption, string purpose, double clockSpeedNominal,
        double clockSpeedMax, string socket, int numberOfStreams, int numberCores, string memoryType)
        {
            Price = price;
            Brand = brand;
            Model = model;
            Consumption = consumption;
            Purpose = purpose;
            ClockSpeedNominal = clockSpeedNominal;
            ClockSpeedMax = clockSpeedMax;
            Socket = socket;
            NumberOfStreams = numberOfStreams;
            NumberCores = numberCores;
            MemoryType = memoryType;
            FullName = $"Процесор {Brand} {Model} {Socket} {ClockSpeedNominal} MHz {Purpose}";

        }
        public string FullName { get; set; }
        public int Consumption { get; set; }
        public string Purpose { get; set; }
        public double ClockSpeedNominal { get; set; }
        public double ClockSpeedMax { get; set; }
        public string Socket { get; set; }
        public string MemoryType { get; set; }
        public int NumberCores { get; set; }
        public int NumberOfStreams { get; set; }
    }
    public class GPU : MemoryModule, IPowerConsumption
    {
        public GPU(int price, string brand, string model, int consumption, string purpose, 
            string memoryType, int sizeGB, int speedMHz)
        {
            Price = price;
            Brand = brand;
            Model = model;
            Consumption = consumption;
            Purpose = purpose;
            SizeGB = sizeGB;
            SpeedMHz = speedMHz;
            MemoryType = memoryType;
            FullName = $"Відеокарта {Brand} {Model} {SizeGB} {MemoryType} {Purpose}";
        }

        public string FullName { get; set; }
        public string Purpose { get; set; }
        public string MemoryType { get; set; }
        public int Consumption { get; set; }
    }
    public class Motherboard : ComputerComponent
    {
        public Motherboard(int price, string brand, string model, string socket, string chipset, 
            string formFactor, string purpose, string memoryType)
        {
            Price = price;
            Brand = brand;
            Model = model;
            Purpose = purpose;
            Socket = socket;
            Сhipset = chipset;
            FormFactor = formFactor;
            MemoryType = memoryType;
            FullName = $"Материнська плата {Brand} {Model} {Socket} {Сhipset} {FormFactor} {Purpose}";
        }
        public string FullName { get; set; }
        public string Purpose { get; set; }
        public string Socket { get; set; }
        public string Сhipset { get; set; }
        public string FormFactor { get; set; }
        public string MemoryType { get; set; }
    }
    public class RAM : MemoryModule
    {
        public RAM(int price, string brand, string model, string memoryType, int sizeGB, int speedMHz)
        {
            Price = price;
            Brand = brand;
            Model = model;
            MemoryType = memoryType;
            SizeGB = sizeGB;
            SpeedMHz = speedMHz;
            FullName = $"Оперативна память {Brand} {Model} {MemoryType} {SizeGB} GB {SpeedMHz} MHz";
        }
        public string FullName { get; set; }
        public string MemoryType { get; set; }
    }
    public class StorageDrives : StorageDevice
    {
        public StorageDrives(int price, string brand, string model, string type, int sizeGB)
        {
            Price = price;
            Brand = brand;
            Model = model;
            Type = type;
            SizeGB = sizeGB;
            FullName = $"Накопичувач {Type} {Brand} {Model} {SizeGB} GB";
        }
        public string FullName { get; set; }
        public string Type { get; set; }
    }
    public class PowerSupplyUnit : ComputerComponent
    {
        public PowerSupplyUnit(int price, string brand, string model, int wattage, int productivity)
        {
            Price = price;
            Brand = brand;
            Model = model;
            Wattage = wattage;
            Productivity = productivity;
            FullName = $"Блок живлення {Brand} {Model} {Wattage} Вт";
        }
        public string FullName { get; set; }
        public int Wattage { get; set; }
        public int Productivity { get; set; }
    }
    public class Case : ComputerComponent
    {
        public Case(int price, string brand, string model, string formFactor, int heigh)
        {
            Price = price;
            Brand = brand;
            Model = model;
            FormFactor = formFactor;
            Height = heigh;
            FullName = $"Системний блок {Brand} {Model} {FormFactor} Ширина: {Height} ";
        }

        public string FullName { get; set; }
        public string FormFactor { get; set; }
        public int Height { get; set; }
    }
    public class CoolingSystem : ComputerComponent
    {
        public CoolingSystem(int price, string brand, string model, int maxTDP, string socket, int height)
        {
            Price = price;
            Brand = brand;
            Model = model;
            MaxTDP = maxTDP;
            Socket = socket;
            Height = height;
            FullName = $"Охолоджування ЦП {Brand} {Model} {Socket} {MaxTDP} TDP  Висота:{Height}" ;
        }
        public string FullName { get; set; }
        public int MaxTDP { get; set; }
        public string Socket { get; set; }
        public int Height { get; set; }
    }
    public interface IPowerConsumption
    {
        int Consumption { get; set; }
    }
    public interface ISize
    {
        int SizeGB { get; set; }
    }
    public interface ISpeed
    {
        int SpeedMHz { get; }
    }
    public abstract class ComputerComponent
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
    }
    public abstract class StorageDevice : ComputerComponent, ISize
    {
        public int SizeGB { get; set; }
    }
    public abstract class MemoryModule : ComputerComponent, ISpeed, ISize
    {
        public int SpeedMHz { get; set; }
        public int SizeGB { get; set; }
    }
}
