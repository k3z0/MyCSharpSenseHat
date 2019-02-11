////////////////////////////////////////////////////////////////////////////
//
//  This file is part of Rpi.SenseHat.Tools
//
//  Copyright (c) 2017, Mattias Larsson
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in 
//  the Software without restriction, including without limitation the rights to use, 
//  copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
//  Software, and to permit persons to whom the Software is furnished to do so, 
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all 
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//  PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
//  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emmellsoft.IoT.Rpi.SenseHat.Fonts.MultiColor;
using Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor;
using Emmellsoft.IoT.Rpi.SenseHat.Tools.Font;
using Emmellsoft.IoT.Rpi.SenseHat.Tools.LedBuffer;

namespace Emmellsoft.IoT.Rpi.SenseHat.Tools
{
	internal static class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Before call");
			ISenseHat senseHat = await SenseHatFactory.GetSenseHat().ConfigureAwait(false);
			Console.WriteLine("After call");
			BinaryClock(senseHat);
		}

		 private static readonly Color _activeBitColor = Color.Red;
        private static readonly Color _inctiveBitColor = Color.DimGray;

		private static void BinaryClock(ISenseHat senseHat)
		{
			 while (true)
            {
                senseHat.Display.Clear();
                senseHat.Display.Screen[0, 0] = _activeBitColor; // Place a dot to mark the top left corner.

                DateTime now = DateTime.Now;

                DrawBinary(senseHat, 0, now.Hour);
                DrawBinary(senseHat, 3, now.Minute);
                DrawBinary(senseHat, 6, now.Second);

                senseHat.Display.Update(); // Update the physical display.

               // SetScreenText?.Invoke(now.ToString("HH':'mm':'ss")); // Update the MainPage (if it's utilized; i.e. not null).

                // Take a short nap.
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
            }
        }

        private static void DrawBinary(ISenseHat senseHat, int x, int value)
        {
            for (int y = 7; y >= 0; y--)
            {
                Color bitColor = (value % 2 == 1)
                    ? _activeBitColor
                    : _inctiveBitColor;

                senseHat.Display.Screen[x, y] = bitColor;
                senseHat.Display.Screen[x + 1, y] = bitColor;

                value = value >> 1;
            }
        }
	}
}
