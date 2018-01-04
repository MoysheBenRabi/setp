using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP;

namespace DaemonLogic
{
    public class LocalProcessLogic
    {
        public static LocalProcess GetLocalProcess(Guid localProcessId)
        {
             using (DaemonEntities entities = new DaemonEntities())
             {
                 LocalProcess localProcess=QueryUtil.First<LocalProcess>((from p in entities.LocalProcess where p.LocalProcessId == localProcessId select p));
                 entities.Detach(localProcess);
                 return localProcess;
             }
        }

        public static LocalProcess ReserveLocalProcess(Participant participant, string address)
        {
             using (DaemonEntities entities = new DaemonEntities())
             {
                 entities.Attach(participant);

                 try
                 {
                     int serverPort = MxpConstants.DefaultServerPort;
                     if ((from l in entities.LocalProcess select l).Count() > 0)
                     {
                         serverPort = (from l in entities.LocalProcess select l).Max(l => l.ServerPort) + 2;
                     }
                     int hubPort = serverPort + 1;

                     LocalProcess localProcess = new LocalProcess
                     {
                         LocalProcessId = Guid.NewGuid(),
                         Participant = participant,
                         Name = "(" + serverPort + "," + hubPort + ")",
                         Address = address,
                         ServerPort = serverPort,
                         HubPort = hubPort,
                         Enabled = false
                     };

                     entities.AddToLocalProcess(localProcess);
                     entities.SaveChanges();
                     entities.Detach(localProcess);

                     return localProcess;
                 }
                 finally
                 {
                     entities.Detach(participant);
                 }
             
            }
        }
    }
}
