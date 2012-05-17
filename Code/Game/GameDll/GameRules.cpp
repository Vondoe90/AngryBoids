#include "StdAfx.h"
#include "GameRules.h"

#include <IActorSystem.h>

#include <IMonoScriptSystem.h>
#include <MonoCommon.h>

//------------------------------------------------------------------------
CGameRules::CGameRules()
	: m_pScriptClass(NULL)
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
	{
		SAFE_RELEASE(m_pScriptClass);
		m_pScriptClass = pScriptSystem->InstantiateScript(newMode, eScriptType_GameRules);
	}
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
	CallMonoScript<void>(m_pScriptClass, "PrecacheLevel");
}

//------------------------------------------------------------------------
void CGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	CallMonoScript<void>(m_pScriptClass, "OnConnect");
}

//------------------------------------------------------------------------
void CGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	CallMonoScript<void>(m_pScriptClass, "OnDisconnect", cause, desc);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientConnect(int channelId, bool isReset)
{
	if(!isReset)
		m_channelIds.push_back(channelId);

	CallMonoScript<void>(m_pScriptClass, "OnClientConnect", channelId, isReset, GetPlayerName(channelId, true));

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc, bool keepClient)
{
	std::vector<int>::iterator channelit=std::find(m_channelIds.begin(), m_channelIds.end(), channelId);
	if (channelit!=m_channelIds.end())
		m_channelIds.erase(channelit);

	CallMonoScript<void>(m_pScriptClass, "OnClientDisconnect", channelId);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientEnteredGame(int channelId, bool isReset)
{ 
	IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->GetActorByChannelId(channelId);
	if(!pActor)
		return false;

	CallMonoScript<void>(m_pScriptClass, "OnClientEnteredGame", channelId, pActor->GetEntityId(), isReset, gEnv->pGameFramework->IsLoadingSaveGame());

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnRevive(IActor *pActor, const Vec3 &pos, const Quat &rot, int teamId)
{
	CallMonoScript<void>(m_pScriptClass, "OnRevive", pActor->GetEntityId(), pos, Ang3(rot), teamId);
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleDestroyed(EntityId id)
{
	if (gEnv->bServer)
		CallMonoScript<void>(m_pScriptClass, "SvOnVehicleDestroyed", id);

	if (gEnv->IsClient())
		CallMonoScript<void>(m_pScriptClass, "OnVehicleDestroyed", id);
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleSubmerged(EntityId id, float ratio)
{
	if (gEnv->bServer)
		CallMonoScript<void>(m_pScriptClass, "SvOnVehicleSubmerged", id, ratio);

	if (gEnv->IsClient())
		CallMonoScript<void>(m_pScriptClass, "OnVehicleSubmerged", id, ratio);
}

//------------------------------------------------------------------------
string CGameRules::VerifyName(const char *name, IEntity *pEntity)
{
	string nameFormatter(name);

	// size limit is 26
	if (nameFormatter.size()>26)
		nameFormatter.resize(26);

	// no spaces at start/end
	nameFormatter.TrimLeft(' ');
	nameFormatter.TrimRight(' ');

	// no empty names
	if (nameFormatter.empty())
		nameFormatter="empty";

	// no @ signs
	nameFormatter.replace("@", "_");

	// search for duplicates
	if (IsNameTaken(nameFormatter.c_str(), pEntity))
	{
		int n=1;
		string appendix;
		do 
		{
			appendix.Format("(%d)", n++);
		} while(IsNameTaken(nameFormatter+appendix));

		nameFormatter.append(appendix);
	}

	return nameFormatter;
}

//------------------------------------------------------------------------
bool CGameRules::IsNameTaken(const char *name, IEntity *pEntity)
{
	for (std::vector<int>::const_iterator it=m_channelIds.begin(); it!=m_channelIds.end(); ++it)
	{
		IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->GetActorByChannelId(*it);
		if (pActor && pActor->GetEntity()!=pEntity && !stricmp(name, pActor->GetEntity()->GetName()))
			return true;
	}

	return false;
}

//------------------------------------------------------------------------
string CGameRules::GetPlayerName(int channelId, bool bVerifyName)
{
	string playerName;

	if(gEnv->bMultiplayer)
	{
		if (INetChannel *pNetChannel=gEnv->pGameFramework->GetNetChannel(channelId))
		{
			playerName=pNetChannel->GetNickname();
			if (!playerName.empty() && bVerifyName)
				playerName=VerifyName(playerName);
		}
	}
	else if(bVerifyName)
		playerName = VerifyName("Dude");

	return playerName;
}