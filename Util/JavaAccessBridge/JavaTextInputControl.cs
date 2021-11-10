using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{
    public class JavaTextInputControl : JavaControl
    {
        internal JavaTextInputControl(Int32 vmID, IntPtr ac)
            : base(vmID, ac)
        {
            // Contract.Requires(JAB.IsJABInitialized);                
        }

        public string Text 
        {       
            
            get
            {                
                AccessibleTextInfo info;                
                bool result = UnsafeNativeMethods.getAccessibleTextInfo(this.VirtualMachineId,
                    this.AccessibleContext, out info, 0, 0);

                if (!result)
                {
                    Trace.TraceWarning("Call to getAccessibleTextInfo failed.");
                    return "";
                }
 
                AccessibleTextItemsInfo textItems;
                result = UnsafeNativeMethods.getAccessibleTextItems(this.VirtualMachineId,
                    this.AccessibleContext, out textItems, info.IndexAtPoint);

                if (!result)
                {
                    Trace.TraceWarning("Call to getAccessibleTextItems failed.");
                    return "";
                }

                return textItems.Sentence;                
            }

            set
            {
                bool result = UnsafeNativeMethods.setTextContents(this.VirtualMachineId, this.AccessibleContext, value);

                if (!result)
                {
                    Trace.TraceWarning("Call to setTextContents failed.");                    
                }
            }
        }
    }
}
