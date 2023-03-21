using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0602_Selection_PickObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            Selection selection = uiDoc.Selection;

            Reference elementRef = null;

            try
            {
                elementRef = selection.PickObject(ObjectType.PointOnElement, "Выберите перегородку кабельного лотка (Esc - отмена)");
            }
            catch (OperationCanceledException e)
            {
                return Result.Cancelled;
            }

            Element selectedElement = doc.GetElement(elementRef);

            TaskDialog.Show("Результаты анализа", $"Пользователь указал элемент:\n{selectedElement.Name}\n" +
                                                  $"\nКоординаты точки:\n{elementRef.GlobalPoint}");

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Delete element");

                doc.Delete(elementRef.ElementId);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0602_Selection_PickObjects : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            Selection selection = uiDoc.Selection;

            List<Reference> elementRefs = null;

            try
            {
                elementRefs = selection.PickObjects(ObjectType.Element, new SeparatorSelectionFilter(), "Выберите перегородки кабельного лотка (Esc - отмена)").ToList();
            }
            catch (OperationCanceledException e)
            {
                return Result.Cancelled;
            }

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Delete elements");

                elementRefs.ForEach(it => doc.Delete(it.ElementId));

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0602_Selection_PickElementsByRectangle : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            Selection selection = uiDoc.Selection;

            List<Element> selectedElements = null;

            try
            {
                selectedElements = selection.PickElementsByRectangle(new SeparatorSelectionFilter(),
                                   "Выберите перегородки кабельного лотка при помощи рамки (Esc - отмена)").ToList();
            }
            catch (OperationCanceledException e)
            {
                return Result.Cancelled;
            }

            selection.SetElementIds(selectedElements.Select(it => it.Id).ToList());

            return Result.Succeeded;
        }
    }

    public class SeparatorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is FamilyInstance instance && instance.Symbol.FamilyName == "VC_Универсальная перегородка лотка";
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
