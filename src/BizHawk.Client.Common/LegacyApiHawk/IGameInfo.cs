#nullable enable

using System.Collections.Generic;

namespace BizHawk.Client.Common
{
	public interface IGameInfo : IExternalApi
	{
		public string GetBoardType();

		public Dictionary<string, string> GetOptions();

		public string GetRomHash();

		public string GetRomName();

		public string? GetStatus(); //TODO check nullability, impl had `(null)?.Status.ToString()`

		public bool InDatabase();

		public bool IsStatusBad();
	}
}
