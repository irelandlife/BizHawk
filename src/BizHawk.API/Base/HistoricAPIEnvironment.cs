namespace BizHawk.API.Base
{
	public sealed class HistoricAPIEnvironment
	{
		public readonly HistoricAPIEnvironment Last;

		public HistoricAPIEnvironment(HistoricAPIEnvironment last) => Last = last;
	}
}
