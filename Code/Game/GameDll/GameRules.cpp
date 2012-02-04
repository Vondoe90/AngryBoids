#include "StdAfx.h"
#include "GameRules.h"

#include <IActorSystem.h>

CGameRules::CGameRules()
{
}

CGameRules::~CGameRules()
{
	gEnv->pGameFramework->GetIGameRulesSystem()->SetCurrentGameRules(NULL);
}

bool CGameRules::OnClientConnect(int channelId, bool isReset)
{
	IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->CreateActor(channelId, "Player", "Actor", Vec3(ZERO), Quat(IDENTITY), Vec3(1,1,1));
	
	return pActor!=NULL;
}

bool CGameRules::Init(IGameObject *pGameObject) 
{ 
	SetGameObject(pGameObject);

	if(!GetGameObject()->BindToNetwork())
		return false;

	gEnv->pGameFramework->GetIGameRulesSystem()->SetCurrentGameRules(this);

	return true; 
}

void CGameRules::PostInit(IGameObject *pGameObject) 
{
	GetGameObject()->EnableUpdateSlot(this,0);
}

void CGameRules::Update(SEntityUpdateContext& ctx, int updateSlot) 
{
	if (gEnv->bServer)
		GetGameObject()->ChangedNetworkState(eEA_GameServerDynamic);
}