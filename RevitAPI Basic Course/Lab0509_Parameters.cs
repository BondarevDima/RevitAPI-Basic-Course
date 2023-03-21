using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0509_Parameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<CableTray> cableTrays = new FilteredElementCollector(doc).OfClass(typeof(CableTray))
                                                                          .Cast<CableTray>()
                                                                          .ToList();

            List<FamilyInstance> separators = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                               .OfCategory(BuiltInCategory.OST_CableTrayFitting)
                                                                               .Cast<FamilyInstance>()
                                                                               .Where(it => it.Symbol.FamilyName == "VC_Универсальная перегородка лотка")
                                                                               .ToList();

            List<FamilyInstance> covers = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                           .OfCategory(BuiltInCategory.OST_CableTrayFitting)
                                                                           .Cast<FamilyInstance>()
                                                                           .Where(it => it.Symbol.FamilyName == "VC_Универсальная крышка лотка")
                                                                           .ToList();

            FamilySymbol separatorSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                            .OfCategory(BuiltInCategory.OST_CableTrayFitting)
                                                                            .Cast<FamilySymbol>()
                                                                            .First(it => it.FamilyName == "VC_Универсальная перегородка лотка");

            FamilySymbol coverSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                        .OfCategory(BuiltInCategory.OST_CableTrayFitting)
                                                                        .Cast<FamilySymbol>()
                                                                        .First(it => it.FamilyName == "VC_Универсальная крышка лотка");

            Options viewSpecificOptions = new Options()
            {
                DetailLevel = ViewDetailLevel.Medium,
                ComputeReferences = true
            };

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Create cable tray separators & covers");

                separators.ForEach(it => doc.Delete(it.Id));

                covers.ForEach(it => doc.Delete(it.Id));

                if (!separatorSymbol.IsActive) separatorSymbol.Activate();

                if (!coverSymbol.IsActive) coverSymbol.Activate();

                foreach (var cableTray in cableTrays)
                {
                    bool isSeparatorNeeded = cableTray.LookupParameter("VC_Установить перегородку")?.AsInteger() == 1,
                         isCoverNeeded = cableTray.LookupParameter("VC_Установить крышку")?.AsInteger() == 1;

                    if (isSeparatorNeeded || isCoverNeeded)
                    {
                        double height = cableTray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM).AsDouble(),
                               width = cableTray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM).AsDouble();

                        Solid cableTrayGeometry = cableTray.get_Geometry(viewSpecificOptions).First(it => it is Solid) as Solid;

                        Face cableTrayTop = cableTrayGeometry.Faces.get_Item(3);

                        Curve cableTrayLine = (cableTray.Location as LocationCurve).Curve;

                        XYZ cableTrayLineStartPoint = cableTrayLine.GetEndPoint(0),
                            cableTrayLineEndPoint = cableTrayLine.GetEndPoint(1),
                            separatorStartPoint = cableTrayTop.Project(cableTrayLineStartPoint).XYZPoint,
                            separatorEndPoint = cableTrayTop.Project(cableTrayLineEndPoint).XYZPoint;

                        Line partLine = Line.CreateBound(separatorStartPoint, separatorEndPoint);

                        if (isSeparatorNeeded)
                        {
                            FamilyInstance separator = doc.Create.NewFamilyInstance(cableTrayTop, partLine, separatorSymbol);

                            separator.LookupParameter("Высота лотка")?.Set(height);
                        }

                        if (isCoverNeeded)
                        {
                            FamilyInstance cover = doc.Create.NewFamilyInstance(cableTrayTop, partLine, coverSymbol);

                            cover.LookupParameter("Ширина лотка")?.Set(width);
                        }
                    }
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
