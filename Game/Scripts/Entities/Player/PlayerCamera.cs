using CryEngine;

namespace CryGameCode.AngryBoids
{
	/// <summary>
	/// HAX LIKE YOU'VE NEVER HAXED BEFORE
	/// </summary>
	public class CameraProxy : Actor
	{
		public void Init()
		{
			View.ActiveView.FieldOfView = Math.DegreesToRadians(60);
		}
	}
}