#nullable enable

using System;
using System.Collections.Generic;

using BizHawk.API.ApiHawk;
using BizHawk.API.Base;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk.APIImpl.ApiHawk
{
	internal sealed class GameInfoLibLegacyImpl : LibBase<GlobalsAccessAPIEnvironment>, IGameInfoLib, IGameInfo
	{
		public string GetRomName() => LoadedRomName ?? string.Empty;
		public string? LoadedRomName => Env.GlobalGame?.Name;

		public string GetRomHash() => LoadedRomHash ?? string.Empty;
		public string? LoadedRomHash => Env.GlobalGame?.Hash;

		public bool InDatabase() => Env.GlobalGame?.NotInDatabase == false;
		public bool? IsLoadedRomNotInDatabase => Env.GlobalGame?.NotInDatabase;

		public bool IsStatusBad() => Env.GlobalGame?.IsRomStatusBad() != false;
		public string? GetStatus() => Env.GlobalGame?.Status.ToString();
		public RomStatus? LoadedRomStatus => Env.GlobalGame?.Status;

		public string GetBoardType() => LoadedRomMapperName ?? string.Empty;
		public string? LoadedRomMapperName => Env.BoardInfo?.BoardName;

		public Dictionary<string, string> GetOptions() => OptionsImpl() ?? new Dictionary<string, string>();
		public IReadOnlyDictionary<string, string>? LoadedRomDBOverrides => OptionsImpl();

		public GameInfoLibLegacyImpl(out Action<GlobalsAccessAPIEnvironment> updateEnv) : base(out updateEnv) {}

		private Dictionary<string, string>? OptionsImpl()
		{
			if (Env.GlobalGame == null) return null;
			var options = new Dictionary<string, string>();
			foreach (var option in Env.GlobalGame.GetOptionsDict()) options[option.Key] = option.Value;
			return options;
		}
	}
}
