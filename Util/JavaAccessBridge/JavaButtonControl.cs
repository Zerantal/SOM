using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{
    public class JavaButtonControl : JavaControl
    {

        internal JavaButtonControl(Int32 vmID, IntPtr ac)
            : base(vmID, ac)
        {
            // Contract.Requires(JAB.IsJABInitialized);
        }

        public void Click()
        {            
            IntPtr failure;
            
            AccessibleActionsToDo actions = new AccessibleActionsToDo();
            // Contract.Assume(actions.ActionsCount == 0);
            actions.AddAction("click");

            bool result = UnsafeNativeMethods.doAccessibleActions(this.VirtualMachineId, this.AccessibleContext, ref actions, out failure);

            if (result == false)
                throw new JABException("Click action failed.");            
        }
    }
}
