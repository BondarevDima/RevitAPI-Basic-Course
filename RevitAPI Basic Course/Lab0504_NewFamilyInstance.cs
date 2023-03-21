using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0504_NewFamilyInstance_Furniture : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilySymbol couchSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                        .OfCategory(BuiltInCategory.OST_Furniture)
                                                                        .Cast<FamilySymbol>()
                                                                        .First(it => it.FamilyName == "Диван-Pensi" && it.Name == "1650 мм");
            XYZ couchLocation = XYZ.Zero;

            Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).FirstElement() as Level;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert couch");

                if (!couchSymbol.IsActive)
                {
                    couchSymbol.Activate();
                }

                doc.Create.NewFamilyInstance(couchLocation, couchSymbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0504_NewFamilyInstance_Doors : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilySymbol doorSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                       .OfCategory(BuiltInCategory.OST_Doors)
                                                                       .Cast<FamilySymbol>()
                                                                       .First(it => it.FamilyName == "Дверь-Одинарная-Панель" && it.Name == "750 x 2000 мм");

            IEnumerable<Element> walls = new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToElements();

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert doors");

                if (!doorSymbol.IsActive)
                {
                    doorSymbol.Activate();
                }

                foreach (var wall in walls)
                {
                    Curve wallCurve = (wall.Location as LocationCurve).Curve;

                    XYZ wallCenter = wallCurve.Evaluate(0.5, true);

                    doc.Create.NewFamilyInstance(wallCenter, doorSymbol, wall, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0504_NewFamilyInstance_CableTraySeparator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilySymbol separatorSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                            .OfCategory(BuiltInCategory.OST_CableTrayFitting)
                                                                            .Cast<FamilySymbol>()
                                                                            .First(it => it.FamilyName == "VC_Перегородка лотка");

            List<CableTray> cableTrays = new FilteredElementCollector(doc).OfClass(typeof(CableTray))
                                                                          .Cast<CableTray>()
                                                                          .Where(it => it.Height == UnitUtils.ConvertToInternalUnits(50, UnitTypeId.Millimeters))
                                                                          .ToList();

            Options viewSpecificOptions = new Options()
            {
                DetailLevel = ViewDetailLevel.Medium,
                ComputeReferences = true
            };

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert cable tray separators");

                if (!separatorSymbol.IsActive)
                {
                    separatorSymbol.Activate();
                }

                foreach (var cableTray in cableTrays)
                {
                    Solid cableTrayGeometry = cableTray.get_Geometry(viewSpecificOptions).First(it => it is Solid) as Solid;

                    Face cableTrayTop = cableTrayGeometry.Faces.get_Item(3);

                    Curve cableTrayLine = (cableTray.Location as LocationCurve).Curve;

                    XYZ cableTrayLineStartPoint = cableTrayLine.GetEndPoint(0),
                        cableTrayLineEndPoint = cableTrayLine.GetEndPoint(1),
                        separatorStartPoint = cableTrayTop.Project(cableTrayLineStartPoint).XYZPoint,
                        separatorEndPoint = cableTrayTop.Project(cableTrayLineEndPoint).XYZPoint;

                    Line separatorLine = Line.CreateBound(separatorStartPoint, separatorEndPoint);

                    doc.Create.NewFamilyInstance(cableTrayTop, separatorLine, separatorSymbol);
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
