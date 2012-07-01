using CryEngine;

namespace CryGameCode
{
    /// <summary>
    /// Contains the default callbacks called from the GameDll (Specifically GameRules.cpp and GameRulesClientServer.cpp)
    /// </summary>
    public abstract class GameRulesBase : GameRules
    {
        #region Delegates
        public delegate void OnPrecacheLevel();

        public delegate void OnSetTeamDelegate(EntityId actorId, EntityId teamId);

        public delegate void OnClientConnectDelegate(int channelId, bool isReset = false, string playerName = "");
        public delegate void OnClientDisconnectDelegate(int channelId);

        public delegate void OnClientEnteredGameDelegate(int channelId, EntityId playerId, bool reset, bool loadingSaveGame);

        public delegate void OnItemDroppedDelegate(EntityId itemId, EntityId actorId);
        public delegate void OnItemPickedUpDelegate(EntityId itemId, EntityId actorId);

        public delegate void OnAddTaggedEntityDelegate(EntityId shooterId, EntityId targetId);

        public delegate void OnChangeSpectatorModeDelegate(EntityId actorId, byte mode, EntityId targetId, bool resetAll);
        public delegate void RequestSpectatorTargetDelegate(EntityId playerId, int change);

        public delegate void OnChangeTeamDelegate(EntityId actorId, int teamId);

        public delegate void OnRequestSpawnGroup(EntityId spawnGroupId);
        public delegate void OnSetPlayerSpawnGroup(EntityId playerId, EntityId spawnGroupId);
        public delegate void OnSpawnGroupInvalidDelegate(EntityId playerId, EntityId spawnGroupId);

        public delegate void OnConnectDelegate();
        public delegate void OnDisconnectDelegate(DisconnectionCause cause, string description);

        public delegate void OnHit();//HitInfo hitInfo);
        public delegate void OnExplosion();//ExplosionInfo explosionInfo);

        public delegate void OnReviveDelegate(EntityId actorId, Vec3 pos, Vec3 rot, int teamId);
        public delegate void OnReviveInVehicleDelegate(EntityId actorId, EntityId vehicleId, int seatId, int teamId);
        public delegate void OnKillDelegate(EntityId actorId, EntityId shooterId, string weaponClassName, int damage, int material, int hitType);

        public delegate void OnVehicleDestroyedDelegate(EntityId vehicleId);
        public delegate void OnVehicleSubmergedDelegate(EntityId vehicleId, float ratio);

        public delegate void OnShowScores(bool show);

        public delegate void OnRestartGame();
        #endregion
        
        #region Events
        public static event OnRequestSpawnGroup RequestSpawnGroup;
        public static event OnSetPlayerSpawnGroup SetSpawnGroup;

        public static event OnPrecacheLevel PrecacheLevel;

        public static event OnShowScores ShowScores;

        public static event OnRestartGame RestartGame;
        
        /// <summary>
        /// Events contained inside this class are only invoked on the server.
        /// </summary>
        public static class Server
        {
            public static event OnHit Hit;
            public static event OnExplosion Explosion;
            
            /// <summary>
            /// A client has connected to the server.
            /// </summary>
            public static event OnClientConnectDelegate Connect;
            /// <summary>
            /// A client has disconnected from the server.
            /// </summary>
            public static event OnClientDisconnectDelegate Disconnect;
            /// <summary>
            /// A player has entered the game.
            /// </summary>
            public static event OnClientEnteredGameDelegate EnteredGame;
            
            public static event OnItemDroppedDelegate ItemDropped;
            public static event OnItemPickedUpDelegate ItemPickedUp;

            public static event OnAddTaggedEntityDelegate AddTaggedEntity;
            
            public static event OnChangeSpectatorModeDelegate ChangeSpectatorMode;
            public static event RequestSpectatorTargetDelegate RequestSpectatorTarget;

            public static event OnChangeTeamDelegate ChangeTeam;
            
            public static event OnSetTeamDelegate SetTeam;

            public static event OnSpawnGroupInvalidDelegate SpawnGroupInvalid;
             
            public static event OnVehicleDestroyedDelegate VehicleDestroyed;
            public static event OnVehicleSubmergedDelegate VehicleSubmerged;
        }

        /// <summary>
        /// Events contained inside this class are only invoked on the client.
        /// </summary>
        public static class Client
        {
            public static event OnConnectDelegate Connect;
            public static event OnDisconnectDelegate Disconnect;

            public static event OnSetTeamDelegate SetTeam;

            public static event OnHit Hit;
            public static event OnExplosion Explosion;

            public static event OnReviveDelegate Revive;
            public static event OnReviveInVehicleDelegate ReviveInVehicle;
            public static event OnKillDelegate Kill;

            public static event OnVehicleDestroyedDelegate VehicleDestroyed;
            public static event OnVehicleSubmergedDelegate VehicleSubmerged;
        }
        #endregion
    }
}