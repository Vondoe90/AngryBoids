#include "StdAfx.h"
#include "GameRules.h"

#include <IActorSystem.h>

#include <IMonoScriptSystem.h>
#include <MonoCommon.h>

//------------------------------------------------------------------------
CGameRules::CGameRules()
{
}

//------------------------------------------------------------------------
CGameRules::~CGameRules()
{
	gEnv->pGameFramework->GetIGameRulesSystem()->SetCurrentGameRules(NULL);
}

//------------------------------------------------------------------------
void CGameRules::OnGamemodeChanged(const char *newMode)
{
	if(IMonoScriptSystem *pScriptSystem = gEnv->pMonoScriptSystem)
		m_scriptId = pScriptSystem->InstantiateScript(EMonoScriptType_GameRules, newMode);
}

//------------------------------------------------------------------------
bool CGameRules::Init(IGameObject *pGameObject) 
{ 
	SetGameObject(pGameObject);

	if(!GetGameObject()->BindToNetwork())
		return false;

	gEnv->pGameFramework->GetIGameRulesSystem()->SetCurrentGameRules(this);

	OnGamemodeChanged(GetEntity()->GetClass()->GetName());

	return true; 
}

//------------------------------------------------------------------------
void CGameRules::PostInit(IGameObject *pGameObject) 
{
	GetGameObject()->EnableUpdateSlot(this,0);
}

//------------------------------------------------------------------------
void CGameRules::Update(SEntityUpdateContext& ctx, int updateSlot) 
{
	if (gEnv->bServer)
		GetGameObject()->ChangedNetworkState(eEA_GameServerDynamic);
}

//------------------------------------------------------------------------
void CGameRules::PrecacheLevel()
{
	CallMonoScript<void>(m_scriptId, "PrecacheLevel");
}

//------------------------------------------------------------------------
void CGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	CallMonoScript<void>(m_scriptId, "OnConnect");
}

//------------------------------------------------------------------------
void CGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	CallMonoScript<void>(m_scriptId, "OnDisconnect", cause, desc);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientConnect(int channelId, bool isReset)
{
	if (gEnv->bServer && gEnv->bMultiplayer)
	{
		string playerName;
		if (INetChannel *pNetChannel=gEnv->pGameFramework->GetNetChannel(channelId))
		{
			playerName=pNetChannel->GetNickname();
			if (!playerName.empty())
				playerName="";//VerifyName(playerName);
		}

		if(!playerName.empty())
			CallMonoScript<void>(m_scriptId, "OnClientConnect", channelId, isReset, playerName);
		else
			CallMonoScript<void>(m_scriptId, "OnClientConnect", channelId, isReset);
	}
	else
		CallMonoScript<void>(m_scriptId, "OnClientConnect", channelId);

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc, bool keepClient)
{
	CallMonoScript<void>(m_scriptId, "OnClientDisconnect", channelId);

	return;
}

//------------------------------------------------------------------------
bool CGameRules::OnClientEnteredGame(int channelId, bool isReset)
{ 
	IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->GetActorByChannelId(channelId);
	if(!pActor)
		return false;

	CallMonoScript<void>(m_scriptId, "OnClientEnteredGame", channelId, pActor->GetEntityId(), isReset, gEnv->pGameFramework->IsLoadingSaveGame());

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleDestroyed(EntityId id)
{
	if (gEnv->bServer)
		CallMonoScript<void>(m_scriptId, "SvOnVehicleDestroyed", id);

	if (gEnv->IsClient())
		CallMonoScript<void>(m_scriptId, "OnVehicleDestroyed", id);
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleSubmerged(EntityId id, float ratio)
{
	if (gEnv->bServer)
		CallMonoScript<void>(m_scriptId, "SvOnVehicleSubmerged", id, ratio);

	if (gEnv->IsClient())
		CallMonoScript<void>(m_scriptId, "OnVehicleSubmerged", id, ratio);
}

//------------------------------------------------------------------------
IActor *CGameRules::SpawnPlayer(int channelId, const char *name, const char *className, const Vec3 &pos, const Ang3 &angles)
{ 
	if (!gEnv->bServer)
		return NULL;

	return gEnv->pGameFramework->GetIActorSystem()->CreateActor(channelId, name, className, pos, Quat(angles), Vec3(1,1,1));
}

//------------------------------------------------------------------------
void CGameRules::RevivePlayer(IActor *pIActor, const Vec3 &pos, const Ang3 &angles, int teamId, bool clearInventory)
{
	// TODO
}