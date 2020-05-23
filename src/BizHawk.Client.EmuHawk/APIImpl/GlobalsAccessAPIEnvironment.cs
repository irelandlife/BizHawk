#nullable enable

using System;

using BizHawk.API.Base;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk.APIImpl
{
	public class GlobalsAccessAPIEnvironment : CommonServicesAPIEnvironment
	{
		public Config GlobalConfig => Global.Config;

		public GameInfo GlobalGame => Global.Game;

		public GlobalsAccessAPIEnvironment(
			Action<string> logCallback,
			HistoricAPIEnvironment last,
			out HistoricAPIEnvironment keep
		) : base(
			logCallback,
			last,
			out keep
		) {}
	}
}
