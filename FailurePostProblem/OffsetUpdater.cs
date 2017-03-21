using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FailurePostProblem
{
    public class OffsetUpdater : IUpdater
    {
        private UpdaterId _updaterId;
        public OffsetUpdater(AddInId addinID)
        {
            _updaterId = new UpdaterId(addinID, new Guid("6C00C0B8-DB13-4B05-ADB4-1AF146CF0A64"));
        }

        public void Execute(UpdaterData data)
        {
            var doc = data.GetDocument();



            Startup.PostErrorFailure(doc, data.GetAddedElementIds());



        }
        public string GetAdditionalInformation() => "Updater to lock the offset parameter";



        public ChangePriority GetChangePriority() => ChangePriority.FreeStandingComponents;

        public UpdaterId GetUpdaterId() => _updaterId;


        public string GetUpdaterName() => "Offset locker";
    
    }
}
