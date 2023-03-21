using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0402_HelloWorld : IExternalCommand
    {
        private void HideColumn(List<string> nameRebarElement, ScheduleField field)
        {
            foreach (var name in nameRebarElement)
            {
                if (name == field.GetName())
                    return;

                field.IsHidden = true;

            }
        }


    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<Element> schedules = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Schedules)
                                                                               .Where(it => it.Name.Contains("Ведомость расхода стали на жб_Изделия арматурные"))
                                                                               .ToList();
            foreach (ViewSchedule schedule in schedules)
            {
                var scheduleDefition = schedule.Definition;
                var getField = scheduleDefition.GetFieldCount();
                List<ScheduleField> listField = new List<ScheduleField>();
                List<int> fieldIds = new List<int>();

                var nameRebarElement = new FilteredElementCollector(doc, schedule.Id).WhereElementIsNotElementType().Select(x => x.Name).ToList();

                using (Transaction transaction = new Transaction(doc))
                {
                    transaction.Start("Hide column");
                    for (int i = 0; i < getField; i++)
                    {
                        ScheduleField field = schedule.Definition.GetField(i);
                        if (field.GetName().Contains("Ø"))
                        {
                            field.IsHidden = false;
                            HideColumn(nameRebarElement, field);
                        }
                    }
                    transaction.Commit();
                }
                
            }

            return Result.Succeeded;
        }

    }

}
