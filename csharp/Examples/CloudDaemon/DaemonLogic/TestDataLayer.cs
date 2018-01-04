using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;
using System.IO;

namespace DaemonLogic
{
    [TestFixture]
    public class TestDataLayer
    {
        [Test]
        public void TestParticipantDatabase()
        {
            using (DaemonEntities daemonEntities = new DaemonEntities("metadata=res://*/Daemon.csdl|res://*/Daemon.ssdl|res://*/Daemon.msl;provider=System.Data.SqlClient;provider connection string=';Data Source=.\\SQLEXPRESS;AttachDbFilename=" + Directory.GetParent(this.GetType().Assembly.Location).Parent.Parent.Parent.FullName + "\\CloudDaemonWeb\\App_Data\\CloudDaemonWeb.mdf;Integrated Security=True;User Instance=True;MultipleActiveResultSets=True'"))
            {
                Guid participantId = Guid.NewGuid();
                string openIdUrl = "http://testid.openid.com/";

                Participant newParticipant = new Participant
                {
                    ParticipantId = participantId,
                    OpenIdUrl = openIdUrl
                };

                daemonEntities.AddToParticipant(newParticipant);
                daemonEntities.SaveChanges();

                IQueryable<Participant> participantQuery =
                    from p in daemonEntities.Participant where p.ParticipantId == participantId select p;

                Assert.AreEqual(1, participantQuery.Count<Participant>());

                IEnumerator<Participant> enumerator=participantQuery.GetEnumerator();
                enumerator.MoveNext();
                Participant loadedParticipant = enumerator.Current;
                enumerator.Dispose();

                Assert.AreEqual(participantId, loadedParticipant.ParticipantId);
                Assert.AreEqual(openIdUrl, loadedParticipant.OpenIdUrl);


                daemonEntities.DeleteObject(loadedParticipant);

                daemonEntities.SaveChanges();

                Assert.AreEqual(0, (from p in daemonEntities.Participant where p.ParticipantId == participantId select p).Count<Participant>());

            }
            
        }

        [Test]
        public void TestGuidParsing()
        {
            string str = Guid.NewGuid().ToString("N");
            str = str.Insert(8, "-");
            str = str.Insert(13, "-");
            str = str.Insert(18, "-");
            str = str.Insert(23, "-");
            Guid guid = new Guid(str);
        }

    }
}
