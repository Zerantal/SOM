using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{

    // list of AccessibleActions to do 
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class AccessibleActionsToDo
    {
        private Int32 _actionsCount;				// number of actions to do

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = JABConstants.MaxActionsToDo)]
        AccessibleActionInfo[] _actions; // the accessible actions to do

        public AccessibleActionsToDo()
        {
            _actionsCount = 0;
            _actions = new AccessibleActionInfo[JABConstants.MaxActionsToDo];
        }


        public AccessibleActionsToDo(params string[] actionNames)
        {
            // Contract.Requires(actionNames != null);
            // Contract.Requires(actionNames.Count() <= JABConstants.MaxActionsToDo);
            // Contract.Requires(// Contract.ForAll(actionNames, delegate(string s) { return s != null; }));

            _actionsCount = actionNames.Count();
            _actions = new AccessibleActionInfo[JABConstants.MaxActionsToDo];

            // Contract.Assume(actionNames.Length >= _actionsCount);    // for benefit of static verifier
            for (int i = 0; i < _actionsCount; i++)
                _actions[i] = new AccessibleActionInfo(actionNames[i]);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Diagnostics.Contracts.__ContractsRuntime.Requires<System.InvalidOperationException>(System.Boolean,System.String,System.String)")]
        public void AddAction(string actionName)
        {
            // Contract.Requires(actionName != null);
            // Contract.Requires(ActionsCount < JABConstants.MaxActionsToDo,
                //"The maximum number of actions have already been added to structured.");

            _actions[_actionsCount] = new AccessibleActionInfo(actionName);
            _actionsCount++;
        }

        public Int32 ActionsCount { get { return _actionsCount; } }

        public ReadOnlyCollection<AccessibleActionInfo> Actions 
        {
            get
            {
                List<AccessibleActionInfo> actionToDoList = new List<AccessibleActionInfo>();

                for (int i = 0; i < _actionsCount; i++)
                    actionToDoList.Add(_actions[i]);

                return new ReadOnlyCollection<AccessibleActionInfo>(_actions); 
            }
        }
    }
}
