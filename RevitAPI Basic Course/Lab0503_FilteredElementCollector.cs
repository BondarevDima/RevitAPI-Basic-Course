using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0503_FilteredElementCollector : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            //List<CurveElement> curves = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Lines)
            //                                                             .Cast<CurveElement>()
            //                                                             .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
            //                                                             .ToList();

            //List<CurveElement> curves = new FilteredElementCollector(doc).OfClass(typeof(CurveElement))
            //                                                             .Cast<CurveElement>()
            //                                                             .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
            //                                                             .ToList();

            //List<CurveElement> curves = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(CurveElement)))
            //                                                             .Cast<CurveElement>()
            //                                                             .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
            //                                                             .ToList();

            List<CurveElement> curves = new FilteredElementCollector(doc).WherePasses(new CurveElementFilter(CurveElementType.DetailCurve))
                                                                         .Cast<CurveElement>()
                                                                         .ToList();

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Delete all detail curves");

                foreach (var curve in curves)
                {
                    doc.Delete(curve.Id);
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
