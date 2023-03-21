using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static System.Math;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0502_Transaction : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            CurveArray curves = new CurveArray();

            curves.Append(Arc.Create(XYZ.Zero, 10, 0, 2 * PI, XYZ.BasisX, XYZ.BasisY));

            curves.Append(Arc.Create(XYZ.Zero, 8, PI, 2 * PI, XYZ.BasisX, XYZ.BasisY));

            curves.Append(Arc.Create(new XYZ(-8, 0, 0), new XYZ(8, 0, 0), new XYZ(0, -3, 0)));

            curves.Append(Arc.Create(new XYZ(-5, 2, 0), 3, 0, PI, XYZ.BasisX, XYZ.BasisY));

            curves.Append(Arc.Create(new XYZ(5, 2, 0), 3, 0, PI, XYZ.BasisX, XYZ.BasisY));

            View activeView = doc.ActiveView;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Safety transaction");

                doc.Create.NewDetailCurveArray(activeView, curves);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
