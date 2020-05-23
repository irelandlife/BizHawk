using System.Collections.Generic;

namespace BizHawk.API.ApiHawk
{
	/// <remarks>
	/// Changes from 2.4.2:
	/// <list type="bullet">
	/// <item><description>TODO @yoshi: see impl</description></item>
	/// </list>
	/// </remarks>
	public interface IGameInfoLib
	{
		public string? LoadedRomMapperName { get; }

		public IReadOnlyDictionary<string, string>? LoadedRomDBOverrides { get; }

		public string? LoadedRomHash { get; }

		public string? LoadedRomName { get; }

		public RomStatus? LoadedRomStatus { get; } //TODO check nullability, impl had `(null)?.Status.ToString()`

		public bool? IsLoadedRomNotInDatabase { get; }
	}
}
