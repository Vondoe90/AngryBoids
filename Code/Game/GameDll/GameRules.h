#pragma once

#include <IGameRulesSystem.h>

struct IActor;
struct IMonoClass;

class CGameRules : public CGameObjectExtensionHelper<CGameRules, IGameRules, 64 /* max RMI's */>
{
public:
	CGameRules();
	~CGameRules();

	// IGameRules
	virtual bool ShouldKeepClient(int channelId, EDisconnectionCause cause, const char *desc) const { return (!strcmp("timeout", desc) || cause==eDC_Timeout); }
	virtual void PrecacheLevel();
	virtual void PrecacheLevelResource(const char* resourceName, EGameResourceType resourceType) {}
	virtual XmlNodeRef FindPrecachedXmlFile( const char *sFilename ) { return 0; }
	virtual void OnConnect(struct INetChannel *pNetChannel);
	virtual void OnDisconnect(EDisconnectionCause cause, const char *desc);
	virtual bool OnClientConnect(int channelId, bool isReset);
	virtual void OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc, bool keepClient);
	virtual bool OnClientEnteredGame(int channelId, bool isReset);
	virtual void OnEntitySpawn(IEntity *pEntity) {}
	virtual void OnEntityRemoved(IEntity *pEntity) {}
	virtual void OnEntityReused(IEntity *pEntity, SEntitySpawnParams &params, EntityId prevId) {}
	virtual void SendTextMessage(ETextMessageType type, const char *msg, uint32 to=eRMI_ToAllClients, int channelId=-1, const char *p0=0, const char *p1=0, const char *p2=0, const char *p3=0) {}
	virtual void SendChatMessage(EChatMessageType type, EntityId sourceId, EntityId targetId, const char *msg) {}
	virtual void ClientSimpleHit(const SimpleHitInfo &simpleHitInfo) {}
	virtual void ServerSimpleHit(const SimpleHitInfo &simpleHitInfo) {}
	virtual void ClientHit(const HitInfo &hitInfo) {}
	virtual void ServerHit(const HitInfo &hitInfo) {}
	virtual int GetHitTypeId(const char *type) const { return 0; }
	virtual const char *GetHitType(int id) const { return ""; }
	virtual void OnVehicleDestroyed(EntityId id);
	virtual void OnVehicleSubmerged(EntityId id, float ratio);
	virtual void CreateEntityRespawnData(EntityId entityId) {}
	virtual bool HasEntityRespawnData(EntityId entityId) const { return false; }
	virtual void ScheduleEntityRespawn(EntityId entityId, bool unique, float timer) {}
	virtual void AbortEntityRespawn(EntityId entityId, bool destroyData) {}
	virtual void ScheduleEntityRemoval(EntityId entityId, float timer, bool visibility) {}
	virtual void AbortEntityRemoval(EntityId entityId) {}
	virtual void AddHitListener(IHitListener* pHitListener) {}
	virtual void RemoveHitListener(IHitListener* pHitListener) {}
	virtual bool IsFrozen(EntityId entityId) const { return false; }
	virtual bool OnCollision(const SGameCollision& event) { return true; }
	virtual void OnCollision_NotifyAI( const EventPhys * pEvent ) {}
	virtual void ShowStatus() {}
	virtual bool IsTimeLimited() const { return false; }
	virtual float GetRemainingGameTime() const { return 1.0f; }
	virtual void SetRemainingGameTime(float seconds) {}
	virtual void ClearAllMigratingPlayers(void) {}
	virtual EntityId SetChannelForMigratingPlayer(const char* name, uint16 channelID) { return 0; }
	virtual void StoreMigratingPlayer(IActor* pActor) {}
	virtual bool IsClientFriendlyProjectile(const EntityId projectileId, const EntityId targetEntityId) { return false; }
	// ~IGameRules

	// IGameObjectExtension
	virtual void GetMemoryUsage(ICrySizer *pSizer) const { pSizer->Add(*this); }

	virtual bool Init( IGameObject * pGameObject );
	virtual void PostInit( IGameObject * pGameObject );
	virtual void InitClient(int channelId) {}
	virtual void PostInitClient(int channelId) {}
	virtual bool ReloadExtension( IGameObject * pGameObject, const SEntitySpawnParams &params ) { return true; }
	virtual void PostReloadExtension( IGameObject * pGameObject, const SEntitySpawnParams &params ) {}
	virtual bool GetEntityPoolSignature( TSerialize signature ) { return false; }
	virtual void Release() { delete this; }
	virtual void FullSerialize( TSerialize ser ) {}
	virtual bool NetSerialize( TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags ) { return true; }
	virtual void PostSerialize() {}
	virtual void SerializeSpawnInfo( TSerialize ser ) {}
	virtual ISerializableInfoPtr GetSpawnInfo() { return NULL; }
	virtual void Update( SEntityUpdateContext& ctx, int updateSlot );
	virtual void HandleEvent( const SGameObjectEvent& event ) {}
	virtual void ProcessEvent( SEntityEvent& event ) {}	
	virtual void SetChannelId(uint16 id) {}
	virtual void SetAuthority( bool auth ) {}
	virtual void PostUpdate( float frameTime ) {}
	virtual void PostRemoteSpawn() {}
	// ~IGameObjectExtension

	void OnGamemodeChanged(const char *newMode);

	virtual void OnRevive(IActor *pActor, const Vec3 &pos, const Quat &rot, int teamId = 0);

	virtual string VerifyName(const char *name, IEntity *pEntity=0);
	virtual bool IsNameTaken(const char *name, IEntity *pEntity=0);
	string GetPlayerName(int channelId, bool bVerifyName = false);

protected:

	std::vector<int>		m_channelIds;

	IMonoClass *m_pScriptClass;
};