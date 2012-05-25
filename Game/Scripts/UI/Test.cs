using CryEngine;

namespace CryGameCode.UI
{
	class CryMonoTestFunction : UIEventSystem
	{
		[UIFunction]
		public static void SetResolution(int x, int y, bool fullscreen)
		{
			Debug.LogAlways("SetResolution {0} {1} {2}", x, y, fullscreen);

			//OnSetResolution.Activate(x, y, fullscreen);
		}

		//[UIEvent]
		//public static UIEvent<int, int, bool> OnSetResolution { get; set; }
	}
}
