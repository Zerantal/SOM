using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{
	// all of the actions associated with a component
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class AccessibleActions 
	{
		private Int32 _actionsCount;		    // number of actions

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = JABConstants.MaxActionInfo, ArraySubType = UnmanagedType.Struct)]
		private AccessibleActionInfo[] _actionInfo;	// the action information

		public AccessibleActions()
		{
			_actionInfo = new AccessibleActionInfo[JABConstants.MaxActionInfo];
		}


        public Int32 ActionsCount { get { return _actionsCount; } }

        public ICollection<AccessibleActionInfo> ActionInfo { get { return _actionInfo; } }
	}
	
}