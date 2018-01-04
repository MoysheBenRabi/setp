using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP;

namespace DaemonLogic
{
    public class RemoteProcessLogic
    {
        public static RemoteProcess AddRemoteProcess(Participant participant,LocalProcess locaProcess)
        {
            using(DaemonEntities entities=new DaemonEntities())
            {
                try
                {
                    entities.Attach(locaProcess);
                    entities.Attach(participant);
                    RemoteProcess remoteProcess=new RemoteProcess
                    {
                        RemoteProcessId=Guid.NewGuid(),
                        LocalProcess=locaProcess,
                        Participant=participant,
                        Address="127.0.0.1",
                        HubPort=MxpConstants.DefaultHubPort,
                        Trusted=false
                    };
                    entities.AddToRemoteProcess(remoteProcess);
                    entities.SaveChanges();
                    entities.Detach(remoteProcess);
                    return remoteProcess;
                }
                finally
                {
                    entities.Detach(locaProcess);
                    entities.Detach(participant);

                }
            }
        }
    }
}
