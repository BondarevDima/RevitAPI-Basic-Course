using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0505_Location_Move : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            Wall wall = new FilteredElementCollector(doc).OfClass(typeof(Wall))
                                                         .First() as Wall;

            LocationCurve wallLocation = wall.Location as LocationCurve;

            Curve locationCurve = wallLocation.Curve;

            double wallLength = UnitUtils.ConvertFromInternalUnits(locationCurve.Length, UnitTypeId.Meters);

            TaskDialog.Show("Результаты анализа", $"Длина стены: {wallLength:f2} м");

            XYZ wallTranslation = new XYZ(0, -3, 0);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Move wall");

                wallLocation.Move(wallTranslation);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0505_Location_Rotation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance couch = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                    .OfCategory(BuiltInCategory.OST_Furniture)
                                                                    .First() as FamilyInstance;

            LocationPoint couchLocation = couch.Location as LocationPoint;

            XYZ couchLocationPoint = couchLocation.Point;

            TaskDialog.Show("Результаты анализа", $"Координаты дивана (футы): {couchLocationPoint}");

            Line rotationAxis = Line.CreateBound(XYZ.Zero, XYZ.Zero + new XYZ(0, 0, 1));

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Rotate couch");

                couchLocation.Rotate(rotationAxis, System.Math.PI / 4);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
