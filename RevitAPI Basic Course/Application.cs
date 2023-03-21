using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitAPI_Basic_Course
{
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";

            string tabName = "Мой базовый курс";

            application.CreateRibbonTab(tabName);

            #region 4. Мой первый плагин
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Первый плагин");

                panel.AddItem(new PushButtonData(nameof(Lab0402_HelloWorld), "Приветствие", assemblyLocation, typeof(Lab0402_HelloWorld).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                });
            }
            #endregion

            #region 5. Внесение изменений в модель
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Изменение модели");

                #region 5.2 Изменение модели. Класс Transaction

                panel.AddItem(new PushButtonData(nameof(Lab0502_Transaction), "Транзакция", assemblyLocation, typeof(Lab0502_Transaction).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                });

                panel.AddSeparator();

                #endregion

                #region 5.3 Поиск в модели. Класс FilteredElementCollector

                panel.AddItem(new PushButtonData(nameof(Lab0503_FilteredElementCollector), "Поиск", assemblyLocation, typeof(Lab0503_FilteredElementCollector).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                });

                panel.AddSeparator();

                #endregion

                #region 5.4 Добавление в модель семейства. Метод NewFamilyInstance()

                //panel.AddItem(new PushButtonData(nameof(Lab0504_NewFamilyInstance_Furniture), "Разместить\nдиван", assemblyLocation, typeof(Lab0504_NewFamilyInstance_Furniture).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0504_NewFamilyInstance_Doors), "Разместить\nдверь", assemblyLocation, typeof(Lab0504_NewFamilyInstance_Doors).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0504_NewFamilyInstance_CableTraySeparator), "Разместить\nразделитель", assemblyLocation, typeof(Lab0504_NewFamilyInstance_CableTraySeparator).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 5.5 Изменение положения элемента. Класс Location

                //panel.AddItem(new PushButtonData(nameof(Lab0505_Location_Move), "Location\nПеренос", assemblyLocation, typeof(Lab0505_Location_Move).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0505_Location_Rotation), "Location\nВращение", assemblyLocation, typeof(Lab0505_Location_Rotation).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 5.6 Изменение положения элемента. Класс ElementTransformUtils

                //panel.AddItem(new PushButtonData(nameof(Lab0506_ElementTransformUtils_Copy), "ETU\nКопия", assemblyLocation, typeof(Lab0506_ElementTransformUtils_Copy).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0506_ElementTransformUtils_Reflection), "ETU\nОтражение", assemblyLocation, typeof(Lab0506_ElementTransformUtils_Reflection).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0506_ElementTransformUtils_Move), "ETUs\nПеренос", assemblyLocation, typeof(Lab0506_ElementTransformUtils_Move).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0506_ElementTransformUtils_Rotation), "ETUs\nВращение", assemblyLocation, typeof(Lab0506_ElementTransformUtils_Rotation).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 5.7 Добавление в модель адаптивного семейства. Класс AdaptiveComponentInstanceUtils

                //panel.AddItem(new PushButtonData(nameof(Lab0507_AdaptiveComponentInstanceUtils), "Повесить\nпровод", assemblyLocation, typeof(Lab0507_AdaptiveComponentInstanceUtils).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 5.8 Геометрические преобразования. Класс Transform

                //panel.AddItem(new PushButtonData(nameof(Lab0508_Transform_Translation), "Перенос", assemblyLocation, typeof(Lab0508_Transform_Translation).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0508_Transform_Rotation), "Вращение", assemblyLocation, typeof(Lab0508_Transform_Rotation).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0508_Transform_Complex), "Сложное\nпреобразование", assemblyLocation, typeof(Lab0508_Transform_Complex).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 5.9 Работа с параметрами

                //panel.AddItem(new PushButtonData(nameof(Lab0509_Parameters), "Комплектующие\nлотка", assemblyLocation, typeof(Lab0509_Parameters).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                //});

                #endregion
            }
            #endregion

            #region 6. Взаимодействие с пользователем
            {
                //RibbonPanel panel = application.CreateRibbonPanel(tabName, "Взаимодействие");

                #region 6.1 Указание координат точки в модели

                //panel.AddItem(new PushButtonData(nameof(Lab0601_PickPoint), "Создать\nполилинию", assemblyLocation, typeof(Lab0601_PickPoint).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                //});

                //panel.AddSeparator();

                #endregion

                #region 6.2 Выбор элементов из модели

                //panel.AddItem(new PushButtonData(nameof(Lab0602_Selection_PickObject), "Выбрать\nодин", assemblyLocation, typeof(Lab0602_Selection_PickObject).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0602_Selection_PickObjects), "Выбрать\nмного", assemblyLocation, typeof(Lab0602_Selection_PickObjects).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                //});

                //panel.AddItem(new PushButtonData(nameof(Lab0602_Selection_PickElementsByRectangle), "Выбрать\nрамкой", assemblyLocation, typeof(Lab0602_Selection_PickElementsByRectangle).FullName)
                //{
                //    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                //});

                //panel.AddSeparator();

                #endregion
            }
            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
