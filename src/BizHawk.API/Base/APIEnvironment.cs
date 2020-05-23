using System;

namespace BizHawk.API.Base
{
	public abstract class APIEnvironment
	{
		private readonly HistoricAPIEnvironment _keep;

		public HistoricAPIEnvironment Last => _keep.Last;

		public readonly Action<string> LogCallback;

		protected APIEnvironment(Action<string> logCallback, HistoricAPIEnvironment last, out HistoricAPIEnvironment keep)
		{
			LogCallback = logCallback;
			keep = _keep = new HistoricAPIEnvironment(last);
		}
	}
}
