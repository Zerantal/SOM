using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

namespace SOMSimulator
{
    internal class TextBoxListener : TraceListener
    {
        private TextBox m_OutputControl;

        private delegate void AppendTextDelegate(string message);

        public TextBoxListener(TextBox output)
        {
            if (output == null)
                throw new ArgumentNullException();
            m_OutputControl = output;         
        }

        private void AppendText(string message)
        {
            if (m_OutputControl == null)
                return;
            //If we are not running on the UI thread
            if (m_OutputControl.InvokeRequired)
            {
                AppendTextDelegate del = new AppendTextDelegate(AppendText);
                Form parentForm = m_OutputControl.FindForm();
                if (parentForm != null)    // check that the control is on a valid form
                    parentForm.BeginInvoke(del, new object[] { message });
            }
            else
                m_OutputControl.AppendText(message);
        }            
        
        public override void Write(object obj)
        {
            if (obj == null)
                return;            
            Write(obj.ToString());
        }
                
        public override void Write(string message)
        {
            AppendText(message);
        }
                
        public override void Write(object obj, string category)
        {
            if (obj == null)
                return;
            Write(obj.ToString(), category);
        }
        
        public override void Write(string message, string category)
        {
            AppendText(category + ": " + message);
        }
        
        public override void WriteLine(object obj)
        {
            if (obj == null)
                return;
            WriteLine(obj.ToString());
        }

        public override void WriteLine(string message)
        {                
            AppendText(message + Environment.NewLine);
        }
        public override void WriteLine(object obj, string category)
        {
            if (obj == null)
                return;
            WriteLine(obj.ToString(), category);
        }

        public override void WriteLine(string message, string category)
        {
            AppendText(category + ": " + message + Environment.NewLine);
        }
    }
}
