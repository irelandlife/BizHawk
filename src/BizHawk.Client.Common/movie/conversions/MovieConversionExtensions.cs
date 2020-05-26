﻿using System;
using System.IO;
using System.Linq;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.Gameboy;
using BizHawk.Emulation.Cores.Nintendo.GBHawk;
using BizHawk.Emulation.Cores.Nintendo.SubNESHawk;
using BizHawk.Emulation.Cores.Nintendo.SubGBHawk;
using BizHawk.Emulation.Cores.Sega.MasterSystem;
using BizHawk.Emulation.Cores.Consoles.Sega.gpgx;
using BizHawk.Emulation.Cores.Consoles.Sega.PicoDrive;

namespace BizHawk.Client.Common.MovieConversionExtensions
{
	public static class MovieConversionExtensions
	{
		public static ITasMovie ToTasMovie(this IMovie old)
		{
			string newFilename = $"{old.Filename}.{TasMovie.Extension}";

			if (File.Exists(newFilename))
			{
				int fileNum = 1;
				bool fileConflict = true;
				while (fileConflict)
				{
					if (File.Exists(newFilename))
					{
						newFilename = $"{old.Filename} ({fileNum}).{TasMovie.Extension}";
						fileNum++;
					}
					else
					{
						fileConflict = false;
					}
				}
			}

			var tas = (ITasMovie)MovieService.Get(newFilename, old.StartsFromSavestate);

			for (var i = 0; i < old.InputLogLength; i++)
			{
				var input = old.GetInputState(i);
				tas.AppendFrame(input);
			}

			old.Truncate(0); // Trying to minimize ram usage

			tas.HeaderEntries.Clear();
			foreach (var kvp in old.HeaderEntries)
			{
				tas.HeaderEntries[kvp.Key] = kvp.Value;
			}

			tas.SyncSettingsJson = old.SyncSettingsJson;

			tas.Comments.Clear();
			foreach (var comment in old.Comments)
			{
				tas.Comments.Add(comment);
			}

			tas.Subtitles.Clear();
			foreach (var sub in old.Subtitles)
			{
				tas.Subtitles.Add(sub);
			}

			tas.TextSavestate = old.TextSavestate;
			tas.BinarySavestate = old.BinarySavestate;
			tas.SaveRam = old.SaveRam;

			return tas;
		}

		public static IMovie ToBk2(this IMovie old)
		{
			var bk2 = MovieService.Get(old.Filename.Replace(old.PreferredExtension, Bk2Movie.Extension));

			for (var i = 0; i < old.InputLogLength; i++)
			{
				var input = old.GetInputState(i);
				bk2.AppendFrame(input);
			}

			bk2.HeaderEntries.Clear();
			foreach (var kvp in old.HeaderEntries)
			{
				bk2.HeaderEntries[kvp.Key] = kvp.Value;
			}

			bk2.SyncSettingsJson = old.SyncSettingsJson;

			bk2.Comments.Clear();
			foreach (var comment in old.Comments)
			{
				bk2.Comments.Add(comment);
			}

			bk2.Subtitles.Clear();
			foreach (var sub in old.Subtitles)
			{
				bk2.Subtitles.Add(sub);
			}

			bk2.TextSavestate = old.TextSavestate;
			bk2.BinarySavestate = old.BinarySavestate;
			bk2.SaveRam = old.SaveRam;

			return bk2;
		}

		public static ITasMovie ConvertToSavestateAnchoredMovie(this ITasMovie old, int frame, byte[] savestate)
		{
			string newFilename = old.Filename;

			if (old.Filename.Contains("tasproj"))
			{
				newFilename = newFilename.Remove(newFilename.Length - 7, 7);
				newFilename = $"{newFilename}nfn.{TasMovie.Extension}";
			}
			else
			{
				newFilename = $"{old.Filename}.{TasMovie.Extension}";
			}

			if (File.Exists(newFilename))
			{
				int fileNum = 1;
				bool fileConflict = true;
				while (fileConflict)
				{
					if (File.Exists(newFilename))
					{
						newFilename = $"{old.Filename} ({fileNum}).{TasMovie.Extension}";
						fileNum++;
					}
					else
					{
						fileConflict = false;
					}
				}
			}

			var tas = (ITasMovie)MovieService.Get(newFilename, true);
			tas.BinarySavestate = savestate;
			tas.LagLog.Clear();

			var entries = old.GetLogEntries();

			tas.CopyLog(entries.Skip(frame));
			tas.CopyVerificationLog(old.VerificationLog);
			tas.CopyVerificationLog(entries.Take(frame));

			// States can't be easily moved over, because they contain the frame number.
			// TODO? I'm not sure how this would be done.
			old.TasStateManager.Clear();

			// Lag Log
			tas.LagLog.FromLagLog(old.LagLog);
			tas.LagLog.StartFromFrame(frame);

			tas.HeaderEntries.Clear();
			foreach (var kvp in old.HeaderEntries)
			{
				tas.HeaderEntries[kvp.Key] = kvp.Value;
			}

			tas.StartsFromSavestate = true;
			tas.SyncSettingsJson = old.SyncSettingsJson;

			tas.Comments.Clear();
			foreach (string comment in old.Comments)
			{
				tas.Comments.Add(comment);
			}

			tas.Subtitles.Clear();
			foreach (Subtitle sub in old.Subtitles)
			{
				tas.Subtitles.Add(sub);
			}

			foreach (TasMovieMarker marker in old.Markers)
			{
				if (marker.Frame > frame)
				{
					tas.Markers.Add(new TasMovieMarker(marker.Frame - frame, marker.Message));
				}
			}

			tas.TasStateManager.Settings = old.TasStateManager.Settings;

			tas.Save();
			return tas;
		}

		public static ITasMovie ConvertToSaveRamAnchoredMovie(this ITasMovie old, byte[] saveRam)
		{
			string newFilename = old.Filename;

			if (old.Filename.Contains("tasproj"))
			{
				newFilename = newFilename.Remove(newFilename.Length - 7, 7);
				newFilename = $"{newFilename}nfsr.{TasMovie.Extension}";
			}
			else
			{
				newFilename = $"{old.Filename}.{TasMovie.Extension}";
			}

			if (File.Exists(newFilename))
			{
				int fileNum = 1;
				bool fileConflict = true;
				while (fileConflict)
				{
					if (File.Exists(newFilename))
					{
						newFilename = $"{old.Filename} ({fileNum}).{TasMovie.Extension}";
						fileNum++;
					}
					else
					{
						fileConflict = false;
					}
				}
			}

			var tas = (ITasMovie) MovieService.Get(newFilename, false);
			tas.SaveRam = saveRam;
			tas.TasStateManager.Clear();
			tas.LagLog.Clear();

			var entries = old.GetLogEntries();

			tas.CopyVerificationLog(old.VerificationLog);
			tas.CopyVerificationLog(entries);

			tas.HeaderEntries.Clear();
			foreach (var kvp in old.HeaderEntries)
			{
				tas.HeaderEntries[kvp.Key] = kvp.Value;
			}

			tas.StartsFromSaveRam = true;
			tas.SyncSettingsJson = old.SyncSettingsJson;

			tas.Comments.Clear();
			foreach (string comment in old.Comments)
			{
				tas.Comments.Add(comment);
			}

			tas.Subtitles.Clear();
			foreach (Subtitle sub in old.Subtitles)
			{
				tas.Subtitles.Add(sub);
			}

			tas.TasStateManager.Settings = old.TasStateManager.Settings;

			tas.Save();
			return tas;
		}

		// TODO: This doesn't really belong here, but not sure where to put it
		public static void PopulateWithDefaultHeaderValues(
			this IMovie movie,
			IEmulator emulator,
			GameInfo game,
			FirmwareManager firmwareManager,
			string author)
		{
			movie.Author = author;
			movie.EmulatorVersion = VersionInfo.GetEmuVersion();
			movie.SystemID = emulator.SystemId;

			var settable = new SettingsAdapter(emulator);
			if (settable.HasSyncSettings)
			{
				movie.SyncSettingsJson = ConfigService.SaveWithType(settable.GetSyncSettings());
			}

			if (game.IsNullInstance())
			{
				movie.GameName = "NULL";
			}
			else
			{
				movie.GameName = game.FilesystemSafeName();
				movie.Hash = game.Hash;
				if (game.FirmwareHash != null)
				{
					movie.FirmwareHash = game.FirmwareHash;
				}
			}

			if (emulator.HasBoardInfo())
			{
				movie.BoardName = emulator.AsBoardInfo().BoardName;
			}

			if (emulator.HasRegions())
			{
				var region = emulator.AsRegionable().Region;
				if (region == API.ApiHawk.DisplayType.PAL)
				{
					movie.HeaderEntries.Add(HeaderKeys.Pal, "1");
				}
			}

			if (firmwareManager.RecentlyServed.Any())
			{
				foreach (var firmware in firmwareManager.RecentlyServed)
				{
					var key = $"{firmware.SystemId}_Firmware_{firmware.FirmwareId}";

					if (!movie.HeaderEntries.ContainsKey(key))
					{
						movie.HeaderEntries.Add(key, firmware.Hash);
					}
				}
			}

			if (emulator is GBHawk gbHawk && gbHawk.IsCGBMode())
			{
				movie.HeaderEntries.Add("IsCGBMode", "1");
			}

			if (emulator is SubGBHawk subgbHawk)
			{
				if (subgbHawk._GBCore.IsCGBMode())
				{
					movie.HeaderEntries.Add("IsCGBMode", "1");
				}

				movie.HeaderEntries.Add(HeaderKeys.CycleCount, "0");
			}

			if (emulator is Gameboy gb)
			{
				if (gb.IsCGBMode())
				{
					movie.HeaderEntries.Add("IsCGBMode", "1");
				}

				movie.HeaderEntries.Add(HeaderKeys.CycleCount, "0");
			}

			if (emulator is SMS sms)
			{
				if (sms.IsSG1000)
				{
					movie.HeaderEntries.Add("IsSGMode", "1");
				}

				if (sms.IsGameGear)
				{
					movie.HeaderEntries.Add("IsGGMode", "1");
				}
			}

			if (emulator is GPGX gpgx && gpgx.IsMegaCD)
			{
				movie.HeaderEntries.Add("IsSegaCDMode", "1");
			}

			if (emulator is PicoDrive pico && pico.Is32XActive)
			{
				movie.HeaderEntries.Add("Is32X", "1");
			}

			if (emulator is SubNESHawk)
			{
				movie.HeaderEntries.Add(HeaderKeys.VBlankCount, "0");
			}

			movie.Core = ((CoreAttribute)Attribute
				.GetCustomAttribute(emulator.GetType(), typeof(CoreAttribute)))
				.CoreName;
		}
	}
}
