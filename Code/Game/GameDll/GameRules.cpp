#include "StdAfx.h"
#include "GameRules.h"

#include <IActorSystem.h>

#include <IMonoScriptSystem.h>
#include <MonoCommon.h>

//------------------------------------------------------------------------
CGameRules::CGameRules()
	: m_pScript(NULL)
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
		SAFE_RELEASE(m_pScript);
		m_pScript = pScriptSystem->InstantiateScript(newMode, eScriptFlag_GameRules);
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
	m_pScript->CallMethod("PrecacheLevel");
}

//------------------------------------------------------------------------
void CGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	m_pScript->CallMethod("OnConnect");
}

//------------------------------------------------------------------------
void CGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	m_pScript->CallMethod("OnDisconnect", cause, desc);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientConnect(int channelId, bool isReset)
{
	if(!isReset)
		m_channelIds.push_back(channelId);

	m_pScript->CallMethod("OnClientConnect", channelId, isReset, GetPlayerName(channelId, true));

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc, bool keepClient)
{
	std::vector<int>::iterator channelit=std::find(m_channelIds.begin(), m_channelIds.end(), channelId);
	if (channelit!=m_channelIds.end())
		m_channelIds.erase(channelit);

	m_pScript->CallMethod("OnClientDisconnect", channelId);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientEnteredGame(int channelId, bool isReset)
{ 
	IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->GetActorByChannelId(channelId);
	if(!pActor)
		return false;

	m_pScript->CallMethod("OnClientEnteredGame", channelId, pActor->GetEntityId(), isReset, gEnv->pGameFramework->IsLoadingSaveGame());

	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnRevive(IActor *pActor, const Vec3 &pos, const Quat &rot, int teamId)
{
	m_pScript->CallMethod("OnRevive", pActor->GetEntityId(), pos, Ang3(rot), teamId);
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleDestroyed(EntityId id)
{
	if (gEnv->bServer)
		m_pScript->CallMethod("SvOnVehicleDestroyed", id);

	if (gEnv->IsClient())
		m_pScript->CallMethod("OnVehicleDestroyed", id);
}

//------------------------------------------------------------------------
void CGameRules::OnVehicleSubmerged(EntityId id, float ratio)
{
	if (gEnv->bServer)
		m_pScript->CallMethod("SvOnVehicleSubmerged", id, ratio);

	if (gEnv->IsClient())
		m_pScript->CallMethod("OnVehicleSubmerged", id, ratio);
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