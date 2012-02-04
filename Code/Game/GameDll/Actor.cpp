#include "StdAfx.h"
#include "Actor.h"
#include "Game.h"

#include <IGameRulesSystem.h>

CActor::CActor()
	: m_bClient(false)
{
}


CActor::~CActor()
{
	if(g_pGame && g_pGame->GetIGameFramework() && g_pGame->GetIGameFramework()->GetIActorSystem())
		g_pGame->GetIGameFramework()->GetIActorSystem()->RemoveActor( GetEntityId() );
}

bool CActor::Init( IGameObject * pGameObject ) 
{ 
	SetGameObject(pGameObject);

	g_pGame->GetIGameFramework()->GetIActorSystem()->AddActor(GetEntityId(), this);
	GetGameObject()->BindToNetwork();
	GetEntity()->SetFlags(GetEntity()->GetFlags()|(ENTITY_FLAG_ON_RADAR|ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO));

	return true; 
}

void CActor::HandleEvent(const SGameObjectEvent &event)
{
	if (event.event == eCGE_Ragdoll)
	{
		GetGameObject()->SetAspectProfile(eEA_Physics, eAP_Ragdoll);
	}
	else if (event.event == eGFE_BecomeLocalPlayer)
	{
		IEntity *pEntity = GetEntity();
		pEntity->SetFlags(GetEntity()->GetFlags() | ENTITY_FLAG_TRIGGER_AREAS);
		// Invalidate the matrix in order to force an update through the area manager
		pEntity->InvalidateTM(ENTITY_XFORM_POS);

		m_bClient = true;
		GetGameObject()->EnablePrePhysicsUpdate( ePPU_Always );
	}
}