/*************************************************************************
Crytek Source File.
Copyright (C), Crytek Studios, 2001-2004.
-------------------------------------------------------------------------
$Id$
$DateTime$

-------------------------------------------------------------------------
History:
- 27:10:2004   11:29 : Created by M�rcio Martins

*************************************************************************/
#include "StdAfx.h"
#include "ScriptBind_GameRules.h"
#include "GameRules.h"
#include "Actor.h"
#include "Game.h"

#include <MonoCommon.h>
#include <IMonoArray.h>

//------------------------------------------------------------------------
CScriptBind_GameRules::CScriptBind_GameRules()
{
	REGISTER_METHOD(SpawnPlayer);
	REGISTER_METHOD(RevivePlayer);
}

//------------------------------------------------------------------------
CGameRules *CScriptBind_GameRules::GetGameRules()
{
	return static_cast<CGameRules *>(gEnv->pGameFramework->GetIGameRulesSystem()->GetCurrentGameRules());
}

//------------------------------------------------------------------------
CActor *CScriptBind_GameRules::GetActor(EntityId id)
{
	return static_cast<CActor *>(gEnv->pGameFramework->GetIActorSystem()->GetActor(id));
}

//------------------------------------------------------------------------
EntityId CScriptBind_GameRules::SpawnPlayer(int channelId, mono::string name, mono::string className, Vec3 pos, Vec3 angles)
{
	return GetGameRules()->SpawnPlayer(channelId, *name, *className, pos, Ang3(angles))->GetEntityId();
}

//------------------------------------------------------------------------
void CScriptBind_GameRules::RevivePlayer(EntityId playerId, Vec3 pos, Vec3 rot, int teamId, bool clearInventory)
{
	IActor *pActor = gEnv->pGameFramework->GetIActorSystem()->GetActor(playerId);
	if(!pActor)
		return;

	GetGameRules()->RevivePlayer(pActor, pos, Ang3(rot), teamId, clearInventory);
}