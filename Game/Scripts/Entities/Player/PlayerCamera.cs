using CryEngine;

namespace CryGameCode.AngryBoids
{
	/// <summary>
	/// HAX LIKE YOU'VE NEVER HAXED BEFORE
	/// </summary>
	public class CameraProxy : BasePlayer
	{
		public void Init()
		{
			ViewSystem.ActiveView.FieldOfView = Math.DegreesToRadians(60);
		}
	}
}