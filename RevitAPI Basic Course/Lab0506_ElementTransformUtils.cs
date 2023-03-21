using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0506_ElementTransformUtils_Copy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance couch = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                    .OfCategory(BuiltInCategory.OST_Furniture)
                                                                    .Cast<FamilyInstance>()
                                                                    .Last(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм");

            XYZ translation = new XYZ(-10, 10, 0);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Copy couch");

                ElementTransformUtils.CopyElement(doc, couch.Id, translation);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0506_ElementTransformUtils_Reflection : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance couch = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                    .OfCategory(BuiltInCategory.OST_Furniture)
                                                                    .Cast<FamilyInstance>()
                                                                    .Last(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм");

            Plane reflectionPlane = Plane.CreateByNormalAndOrigin(XYZ.BasisX, XYZ.Zero);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Mirror couch");

                ElementTransformUtils.MirrorElement(doc, couch.Id, reflectionPlane);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0506_ElementTransformUtils_Move : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<ElementId> couchIds = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                        .OfCategory(BuiltInCategory.OST_Furniture)
                                                                        .Cast<FamilyInstance>()
                                                                        .Where(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм")
                                                                        .Select(it => it.Id)
                                                                        .ToList();

            XYZ translation = new XYZ(10, 10, 0);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Move couches");

                ElementTransformUtils.MoveElements(doc, couchIds, translation);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0506_ElementTransformUtils_Rotation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<ElementId> couchIds = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                        .OfCategory(BuiltInCategory.OST_Furniture)
                                                                        .Cast<FamilyInstance>()
                                                                        .Where(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм")
                                                                        .Select(it => it.Id)
                                                                        .ToList();

            Line rotationAxis = Line.CreateBound(XYZ.Zero, XYZ.Zero + new XYZ(0, 0, 1));

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Rotate couches");

                ElementTransformUtils.RotateElements(doc, couchIds, rotationAxis, Math.PI / 4);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
