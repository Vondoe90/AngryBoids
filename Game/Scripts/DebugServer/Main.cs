using System;

using System.Diagnostics;

namespace DebugServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{	
			var startInfo = new ProcessStartInfo(@"D:\Dev\INK\MiniMonoGame\Bin32\Editor.exe");
			startInfo.Arguments = "-DEBUG";
			
			var editorProcess = Process.Start(startInfo);
			
			while(true)
			{
			}
		}
	}
}
