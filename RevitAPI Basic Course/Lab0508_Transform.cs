using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using static System.Math;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0508_Transform_Translation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                               .Cast<FamilyInstance>()
                                                                               .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

            Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5));

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");

                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, translation, true);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0508_Transform_Rotation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                               .Cast<FamilyInstance>()
                                                                               .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

            Transform rotation = Transform.CreateRotation(XYZ.BasisZ, PI / 4);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");

                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, rotation, true);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Lab0508_Transform_Complex : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                               .Cast<FamilyInstance>()
                                                                               .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

            Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5)),
                      rotation = Transform.CreateRotation(XYZ.BasisZ, PI / 4),
                      complex = translation * rotation;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");

                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, complex, true);

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
