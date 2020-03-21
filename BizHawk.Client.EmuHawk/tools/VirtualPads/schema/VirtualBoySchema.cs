﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	[Schema("VB")]
	// ReSharper disable once UnusedMember.Global
	public class VirtualBoySchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			yield return StandardController();
			yield return ConsoleButtons();
		}

		private static PadSchema StandardController()
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(222, 103),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = "L_Up",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(14, 36)
					},
					new ButtonSchema
					{
						Name = "L_Down",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(14, 80)
					},
					new ButtonSchema
					{
						Name = "L_Left",
						Icon = Properties.Resources.Back,
						Location = new Point(2, 58)
					},
					new ButtonSchema
					{
						Name = "L_Right",
						Icon = Properties.Resources.Forward,
						Location = new Point(24, 58)
					},
					new ButtonSchema
					{
						Name = "B",
						Location = new Point(122, 58)
					},
					new ButtonSchema
					{
						Name = "A",
						Location = new Point(146, 58)
					},
					new ButtonSchema
					{
						Name = "Select",
						DisplayName = "s",
						Location = new Point(52, 58)
					},
					new ButtonSchema
					{
						Name = "Start",
						DisplayName = "S",
						Location = new Point(74, 58)
					},
					new ButtonSchema
					{
						Name = "R_Up",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(188, 36)
					},
					new ButtonSchema
					{
						Name = "R_Down",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(188, 80)
					},
					new ButtonSchema
					{
						Name = "R_Left",
						Icon = Properties.Resources.Back,
						Location = new Point(176, 58)
					},
					new ButtonSchema
					{
						Name = "R_Right",
						Icon = Properties.Resources.Forward,
						Location = new Point(198, 58)
					},
					new ButtonSchema
					{
						Name = "L",
						Location = new Point(24, 8)
					},
					new ButtonSchema
					{
						Name = "R",
						Location = new Point(176, 8)
					}
				}
			};
		}

		private static PadSchema ConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(75, 50),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = "Power",
						DisplayName = "Power",
						Location = new Point(10, 15)
					}
				}
			};
		}
	}
}
