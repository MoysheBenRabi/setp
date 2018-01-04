using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CloudDeck
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void Process()
        {
            if(synchronizationProgressBar.Maximum != DeckProgram.DeckEngine.ExpectedObjectSynchronizationCount)
            {
                synchronizationProgressBar.Maximum = DeckProgram.DeckEngine.ExpectedObjectSynchronizationCount;
            }
            if (synchronizationProgressBar.Value != DeckProgram.DeckEngine.CurrentObjectSynchronizationCount)
            {
                if (DeckProgram.DeckEngine.CurrentObjectSynchronizationCount < synchronizationProgressBar.Maximum)
                {
                    synchronizationProgressBar.Value = DeckProgram.DeckEngine.CurrentObjectSynchronizationCount;
                }
                else
                {
                    synchronizationProgressBar.Value = synchronizationProgressBar.Maximum;
                }
            }
        }
    }
}
