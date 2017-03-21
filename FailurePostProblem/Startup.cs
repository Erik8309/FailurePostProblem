using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FailurePostProblem
{
    public class Startup : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RegisterFailure();
            RegisterUpdater(application.ActiveAddInId);
            return Result.Succeeded;
        }

        public static void RegisterUpdater(AddInId addinId)
        {
            var updater = new OffsetUpdater(addinId);

            UpdaterRegistry.RegisterUpdater(updater);

            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), new ElementClassFilter(typeof(FamilyInstance)), Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM)));
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), new ElementClassFilter(typeof(FamilyInstance)), Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), new ElementClassFilter(typeof(FamilyInstance)), Element.GetChangeTypeElementAddition());
        }
        public static readonly FailureDefinitionId OffsetErrorFailureDefinitionId = new FailureDefinitionId(new Guid("4240E39E-1EFD-4BD1-B66B-1948E003B341"));

        public static void RegisterFailure()
        {
            var failure = FailureDefinition.CreateFailureDefinition(OffsetErrorFailureDefinitionId, FailureSeverity.Error, "The offset parameter cannot be changed for this element. Please use W_Elevation.");


        }
        public static void PostErrorFailure(Document doc, ICollection<ElementId> failingElementIds)
        {
            var failureMessage = new FailureMessage(OffsetErrorFailureDefinitionId);

            failureMessage.SetFailingElements(failingElementIds);

            doc.PostFailure(failureMessage);

        }
    }
}
