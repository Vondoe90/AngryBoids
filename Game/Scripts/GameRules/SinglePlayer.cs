using CryEngine;
using CryGameCode.Entities;

using System.Linq;

/// <summary>
/// The campaign game mode is the base game mode
/// </summary>
namespace CryGameCode
{
    [DefaultGamemodeAttribute]
	public class SinglePlayer : BaseGameRules
	{
        public override void OnClientConnect(int channelId, bool isReset = false, string playerName = "")
        {
			GameRules.SpawnPlayer<PlayerCam>(channelId, "Player", new Vec3(0, 0, 0), new Vec3(0, 0, 0));
        }

		public override void OnRevive(EntityId actorId, Vec3 pos, Vec3 rot, int teamId)
		{
			Console.LogAlways("SinglePlayer.OnRevive");

			var player = GameRules.GetPlayer(actorId) as PlayerCam;
			if (player == null)
			{
				Console.LogAlways("[SinglePlayer.OnRevive] Failed to get player");
				return;
			}

			//player.Position = new Vec3(541, 510, 146);
			//player.Rotation = new Vec3(-90 * ((float)System.Math.PI / 180.0f), 0, 0);

			StaticEntity[] spawnPoints = EntitySystem.GetEntities("SpawnPoint");
			if (spawnPoints == null || spawnPoints.Length < 1)
				Console.LogAlways("$1warning: No spawn points; using default spawn location!");
			else
			{
				SpawnPoint spawnPoint = spawnPoints[0] as SpawnPoint;
				if (spawnPoint != null)
				{
					Console.LogAlways("Found spawnpoint {0}", spawnPoint.Name);

					player.Position = spawnPoint.Position;
					player.Rotation = spawnPoint.Rotation;
				}
			}

			//EntitySystem.EnableUpdates();
			player.OnRevive();
		}
	}
}