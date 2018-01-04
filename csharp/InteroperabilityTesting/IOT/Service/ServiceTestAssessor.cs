using System;
using System.Collections.Generic;
using System.Text;
using IOT.Model;
using MXP.Cloud;
using MXP;
using MXP.Messages;

namespace IOT.Service
{
    public class ServiceTestAssessor
    {
        private TestSuiteState testSuiteState;

        public ServiceTestAssessor(TestSuiteState testSuiteState)
        {
            this.testSuiteState = testSuiteState;
        }

        public void RegisterEventHandlers(CloudBubble bubble)
        {
            bubble.ParticipantConnected += OnParticipantConnected;
            bubble.ParticipantDisconnected += OnParticipantDisconnected;
            bubble.ParticipantMessageReceived += OnParticipantMessageReceived;
        }

        public void OnParticipantConnected(Session session, JoinRequestMessage message, Guid participantId, Guid avatarId)
        {
            testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.Connection);
        }

        public void OnParticipantDisconnected(Session session)
        {
            testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.Disconnection);
        }

        public void OnParticipantMessageReceived(Session session, Message message)
        {
            if(message.GetType()==typeof(InjectRequestMessage))
            {
                testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectInjection);
            }
            if(message.GetType()==typeof(ModifyRequestMessage))
            {
                testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectModification);
            }
            if(message.GetType()==typeof(EjectRequestMessage))
            {
                testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectEjection);
            }
            if(message.GetType()==typeof(ExamineRequestMessage))
            {
                testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectExamination);
            }
            if (message.GetType() == typeof(InteractRequestMessage))
            {
                testSuiteState.MarkTestSuccess(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectInteraction);
            }
        }


    }
}
