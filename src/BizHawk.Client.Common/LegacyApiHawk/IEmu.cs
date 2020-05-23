#nullable enable

using System;
using System.Collections.Generic;

using BizHawk.API.ApiHawk;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.Common
{
	public interface IEmu : IExternalApi
	{
		public Action? FrameAdvanceCallback { get; set; }

		public Action? YieldCallback { get; set; }

		public object? Disassemble(uint pc, string name = "");

		public void DisplayVsync(bool enabled);

		public void FrameAdvance();

		public int FrameCount();

		public string GetBoardName();

		public string GetDisplayType();

		public ulong? GetRegister(string name);

		public Dictionary<string, ulong> GetRegisters();

		public object? GetSettings();

		public string? GetSystemId();

		public bool IsLagged();

		public int LagCount();

		public void LimitFramerate(bool enabled);

		public void MinimizeFrameskip(bool enabled);

		public PutSettingsDirtyBits PutSettings(object settings);

		public void SetIsLagged(bool value = true);

		public void SetLagCount(int count);

		public void SetRegister(string register, int value);

		public void SetRenderPlanes(params bool[] args);

		public long TotalExecutedCycles();

		public void Yield();
	}
}
