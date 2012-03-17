using CryEngine;

namespace CryGameCode.BoidUIEvents
{
	[UIEvent(Name = "OnBoidLaunched", Description = "Triggered when a boid is shot", Category = "BoidEvents")]
	public class OnBoidLaunched
	{
		[Port(Name = "BoidName", Description = "Name of the launched boid")]
		public OutputPort<string> boidType;

		[Port(Name = "Remaining Boids", Description = "Amount of boids left to launch")]
		public OutputPort<int> boidsLeft;
	}

	[UIEvent(Name = "SetDifficulty", Description = "Sets Angry Boids difficulty level", Category = "BoidEvents")]
	public class SetDifficulty
	{
		[Port(Name = "DifficultyLevel", Description = "The difficulty level we want to set!")]
		public void SetDifficultyLevel(string level)
		{
			// Do whatever logic is required to change the diff level.
		}
	}
}