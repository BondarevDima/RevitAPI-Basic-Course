using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI_Basic_Course
{
    [Transaction(TransactionMode.Manual)]
    public class Lab0507_AdaptiveComponentInstanceUtils : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<FamilyInstance> supports = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                                             .OfCategory(BuiltInCategory.OST_GenericModel)
                                                                             .Cast<FamilyInstance>()
                                                                             .Where(it => it.Symbol.FamilyName == "VC_Опора")
                                                                             .ToList();

            FamilySymbol wireSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                       .OfCategory(BuiltInCategory.OST_GenericModel)
                                                                       .Cast<FamilySymbol>()
                                                                       .First(it => it.FamilyName == "VC_Провод");


            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert adaptive component");

                if (!wireSymbol.IsActive)
                {
                    wireSymbol.Activate();
                }

                for (int supportIndex = 0; supportIndex < supports.Count - 1; supportIndex++)
                {
                    FamilyInstance currentSupport = supports[supportIndex],
                                   nextSupport = supports[supportIndex + 1];

                    List<FamilyInstance> currentConnectors = currentSupport.GetSubComponentIds()
                                                                           .Select(it => doc.GetElement(it) as FamilyInstance)
                                                                           .Where(it => it.Symbol.FamilyName == "VC_Коннектор провода")
                                                                           .ToList();

                    List<FamilyInstance> nextConnectors = nextSupport.GetSubComponentIds()
                                                                     .Select(it => doc.GetElement(it) as FamilyInstance)
                                                                     .Where(it => it.Symbol.FamilyName == "VC_Коннектор провода")
                                                                     .ToList();

                    XYZ currentA = (currentConnectors.First(it => it.Symbol.Name == "A").Location as LocationPoint).Point,
                        currentB = (currentConnectors.First(it => it.Symbol.Name == "B").Location as LocationPoint).Point,
                        currentC = (currentConnectors.First(it => it.Symbol.Name == "C").Location as LocationPoint).Point,
                        nextA = (nextConnectors.First(it => it.Symbol.Name == "A").Location as LocationPoint).Point,
                        nextB = (nextConnectors.First(it => it.Symbol.Name == "B").Location as LocationPoint).Point,
                        nextC = (nextConnectors.First(it => it.Symbol.Name == "C").Location as LocationPoint).Point;

                    FamilyInstance wireA = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol),
                                   wireB = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol),
                                   wireC = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol);

                    List<ReferencePoint> wireAPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireA)
                                                                                              .Select(it => doc.GetElement(it) as ReferencePoint)
                                                                                              .ToList();

                    List<ReferencePoint> wireBPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireB)
                                                                                              .Select(it => doc.GetElement(it) as ReferencePoint)
                                                                                              .ToList();

                    List<ReferencePoint> wireCPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireC)
                                                                                              .Select(it => doc.GetElement(it) as ReferencePoint)
                                                                                              .ToList();

                    XYZ currentTranslationA = currentA - wireAPlacementPoints[0].Position,
                        currentTranslationB = currentB - wireBPlacementPoints[0].Position,
                        currentTranslationC = currentC - wireCPlacementPoints[0].Position,
                        nextTranslationA = nextA - wireAPlacementPoints[1].Position,
                        nextTranslationB = nextB - wireBPlacementPoints[1].Position,
                        nextTranslationC = nextC - wireCPlacementPoints[1].Position;

                    wireAPlacementPoints[0].Location.Move(currentTranslationA);

                    wireAPlacementPoints[1].Location.Move(nextTranslationA);

                    wireBPlacementPoints[0].Location.Move(currentTranslationB);

                    wireBPlacementPoints[1].Location.Move(nextTranslationB);

                    wireCPlacementPoints[0].Location.Move(currentTranslationC);

                    wireCPlacementPoints[1].Location.Move(nextTranslationC);
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
