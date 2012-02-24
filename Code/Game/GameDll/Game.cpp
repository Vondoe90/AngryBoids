/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  
 -------------------------------------------------------------------------
  History:
  - 3:8:2004   11:26 : Created by Márcio Martins
  - 17:8:2005        : Modified - NickH: Factory registration moved to GameFactory.cpp

*************************************************************************/
#include "StdAfx.h"
#include "Game.h"

#include "GameRules.h"
#include "ScriptBind_GameRules.h"
#include "Actor.h"

#include <ICryPak.h>
#include <CryPath.h>
#include <IActionMapManager.h>
#include <IViewSystem.h>
#include <ILevelSystem.h>
#include <IItemSystem.h>
#include <IVehicleSystem.h>
#include <IMovieSystem.h>
#include <IPlayerProfiles.h>
#include <IScaleformGFx.h>
#include <IPlatformOS.h>

#include "ISaveGame.h"
#include "ILoadGame.h"
#include "CryPath.h"
#include <IPathfinder.h>

#include "IMaterialEffects.h"

#include "ICheckPointSystem.h"

#include <sys/stat.h>
#include <ShlObj.h>
#define WIN32_LEAN_AND_MEAN // Exclude rarely-used services from Windows headers
#include <windows.h>
#undef GetUserName // GetUserName is a macro in windows:( Gives issues since we have functions that are named the same

#define GAME_DEBUG_MEM  // debug memory usage
#undef  GAME_DEBUG_MEM

//FIXME: really horrible. Remove ASAP
int OnImpulse( const EventPhys *pEvent ) 
{ 
	//return 1;
	return 0;
}

#define GAME_DEBUG_MEM  // debug memory usage
#undef  GAME_DEBUG_MEM

CGame *g_pGame = NULL;

CGame::CGame()
: m_pFramework(nullptr),
	m_pConsole(nullptr),
	m_pPlayerProfileManager(nullptr),
	m_uiPlayerID(~0),
	m_pRayCaster(nullptr)
{
	g_pGame = this;
	m_bReload = false;
	m_inDevMode = false;

	m_pDefaultAM = nullptr;
	m_pMultiplayerAM = nullptr;

	GetISystem()->SetIGame( this );
}

CGame::~CGame()
{
	SAFE_DELETE(m_pScriptBindGameRules);
	SAFE_DELETE(m_pRayCaster);
	
	if(m_pFramework)
	{
		if(m_pFramework->StartedGameContext())
		{
			if(((gEnv->IsEditor() && gEnv->IsEditorGameMode()) || !gEnv->IsEditor()) && !gEnv->IsDedicated())
				m_pFramework->EndGameContext();
		}	
	}

	m_pConsole = nullptr;   //don't use m_pConsole->Release(), it is called later in the shutdown process
	m_pFramework = nullptr; //don't use m_pFramework->Shutdown(), it is called later in the shutdown process
	
	g_pGame = nullptr;
	gEnv->pGame = nullptr;
}

//A callback used to return game specific information to the Networking system via the lobby
//These specify several essential bits of information necessary for trophies on PS3 and matchmaking
void LobbyConfigurationCallback( ECryLobbyService service, SConfigurationParams *requestedParams, uint32 paramCount )
{
	uint32 a;
	for (a=0;a<paramCount;a++)
	{
		switch (requestedParams[a].m_fourCCID)
		{
		case CLCC_LAN_USER_NAME:
			break;	











		default:
			CRY_ASSERT_MESSAGE(0,"Unknown Configuration Parameter Requested!");
			break;
		}
	}
}

void LobbyInitialiseCallback(ECryLobbyService service, ECryLobbyError error, void* arg)
{
	assert( error == eCLE_Success );
}

bool CGame::Init(IGameFramework *pFramework)
{
  LOADING_TIME_PROFILE_SECTION(GetISystem());

#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::Init start");
#endif

	m_pFramework = pFramework;
	assert(m_pFramework);

	m_pConsole = gEnv->pConsole;

	RegisterGameObjectEvents();

	LoadActionMaps( ACTIONMAP_DEFAULT_PROFILE );

	m_pScriptBindGameRules = new CScriptBind_GameRules();

	//load user levelnames for ingame text and savegames
	XmlNodeRef lnames = GetISystem()->LoadXmlFromFile(PathUtil::GetGameFolder() + "/Scripts/GameRules/LevelNames.xml");
	if(lnames)
	{
		int num = lnames->getNumAttributes();
		const char *nameA, *nameB;
		for(int n = 0; n < num; ++n)
		{
			lnames->getAttributeByIndex(n, &nameA, &nameB);
			m_mapNames[string(nameA)] = string(nameB);
		}
	}

	REGISTER_FACTORY(m_pFramework, "Player", CActor, false);
	REGISTER_FACTORY(m_pFramework, "GameRules", CGameRules, false);

	gEnv->pConsole->CreateKeyBind("f12", "r_getscreenshot 2");

	//Ivo: initialites the Crysis conversion file.
	//this is a conversion solution for the Crysis game DLL. Other projects don't need it.
	// No need anymore
	//gEnv->pCharacterManager->LoadCharacterConversionFile("Objects/CrysisCharacterConversion.ccc");

	// set game GUID
	m_pFramework->SetGameGUID(GAME_GUID);

	// TEMP
	// Load the action map beforehand (see above)
	// afterwards load the user's profile whose action maps get merged with default's action map
	m_pPlayerProfileManager = m_pFramework->GetIPlayerProfileManager();

	bool bIsFirstTime = false;
	const bool bResetProfile = gEnv->pSystem->GetICmdLine()->FindArg(eCLAT_Pre,"ResetProfile") != 0;
	if (m_pPlayerProfileManager)
	{
		const char* userName = gEnv->pSystem->GetLoggedInUserName();

		bool ok = m_pPlayerProfileManager->LoginUser(userName, bIsFirstTime);
		if (ok)
		{

			// activate the always present profile "default"
			int profileCount = m_pPlayerProfileManager->GetProfileCount(userName);
			if (profileCount > 0)
			{
				bool handled = false;
				if(gEnv->IsDedicated())
				{
					for(int i = 0; i < profileCount; ++i )
					{
						IPlayerProfileManager::SProfileDescription profDesc;
						bool ok = m_pPlayerProfileManager->GetProfileInfo(userName, i, profDesc);
						if(ok)
						{
							const IPlayerProfile *preview = m_pPlayerProfileManager->PreviewProfile(userName, profDesc.name);
							int iActive = 0;
							if(preview)
							{
								preview->GetAttribute("Activated",iActive);
							}
							if(iActive>0)
							{
								m_pPlayerProfileManager->ActivateProfile(userName,profDesc.name);
								//CryLogAlways("[GameProfiles]: Successfully activated profile '%s' for user '%s'", profDesc.name, userName);
								m_pFramework->GetILevelSystem()->LoadRotation();
								handled = true;
								break;
							}
						}
					}
					m_pPlayerProfileManager->PreviewProfile(userName,NULL);
				}

				if(!handled)
				{
					IPlayerProfileManager::SProfileDescription desc;
					ok = m_pPlayerProfileManager->GetProfileInfo(userName, 0, desc);
					if (ok)
					{
						IPlayerProfile* pProfile = m_pPlayerProfileManager->ActivateProfile(userName, desc.name);

						if (pProfile == 0)
						{
							GameWarning("[GameProfiles]: Cannot activate profile '%s' for user '%s'. Trying to re-create.", desc.name, userName);
							IPlayerProfileManager::EProfileOperationResult profileResult;
							m_pPlayerProfileManager->CreateProfile(userName, desc.name, true, profileResult); // override if present!
							pProfile = m_pPlayerProfileManager->ActivateProfile(userName, desc.name);
							if (pProfile == 0)
								GameWarning("[GameProfiles]: Cannot activate profile '%s' for user '%s'.", desc.name, userName);
							else
								GameWarning("[GameProfiles]: Successfully re-created profile '%s' for user '%s'.", desc.name, userName);
						}

						if (pProfile)
						{
							if (bResetProfile)
							{
								bIsFirstTime = true;
								pProfile->Reset();
								gEnv->pCryPak->RemoveFile("%USER%/game.cfg");
								//CryLogAlways("[GameProfiles]: Successfully reset and activated profile '%s' for user '%s'", desc.name, userName);
							}
							//CryLogAlways("[GameProfiles]: Successfully activated profile '%s' for user '%s'", desc.name, userName);
							m_pFramework->GetILevelSystem()->LoadRotation();
						}
					}
					else
					{
						GameWarning("[GameProfiles]: Cannot get profile info for user '%s'", userName);
					}
				}
			}
			else
			{
				GameWarning("[GameProfiles]: User 'dude' has no profiles");
			}
		}
		else
			GameWarning("[GameProfiles]: Cannot login user '%s'", userName);
	}
	else
		GameWarning("[GameProfiles]: PlayerProfileManager not available. Running without.");

  	//Even if there's no online multiplayer, we're still required to initialize the lobby as it hosts the PS3 trophies.
	//As info from the trophies must be considered when enumerating disk space on PS3, we can't finish the PlatformOS_PS3 init process
	//without the lobby having been created.
	gEnv->pNetwork->SetMultithreadingMode(INetwork::NETWORK_MT_PRIORITY_NORMAL);
	ECryLobbyServiceFeatures	features = ECryLobbyServiceFeatures( eCLSO_Base | eCLSO_Reward | eCLSO_LobbyUI );
	ECryLobbyError						error;
	error = gEnv->pNetwork->GetLobby()->Initialise(eCLS_Online, features, LobbyConfigurationCallback, LobbyInitialiseCallback, this);
	if( error != eCLE_Success )
	{
		CryWarning( VALIDATOR_MODULE_GAME, VALIDATOR_ERROR, "Failed to initialise CryLobby in Game. Return value is 0x%X", error );
	}
	gEnv->pNetwork->GetLobby()->SetLobbyService(eCLS_Online);

	m_pRayCaster = new GlobalRayCaster;
	m_pRayCaster->SetQuota(6);

#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::Init end");
#endif

	return true;
}


bool CGame::CompleteInit()
{
	//Perform crucial post-localization boot checks and processing as the menu screen is going to be skipped
	if( gEnv && gEnv->pSystem && gEnv->pSystem->GetPlatformOS() )
	{
		gEnv->pSystem->GetPlatformOS()->PostLocalizationBootChecks();
		gEnv->pSystem->GetPlatformOS()->PostBootCheckProcessing();
	}
	// Initialize Game02 flow nodes

#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::CompleteInit");
#endif

	m_pFramework->GetIMaterialEffects()->LoadFXLibraries();

	return true;
}

int CGame::Update(bool haveFocus, unsigned int updateFlags)
{
	bool bRun = m_pFramework->PreUpdate( true, updateFlags );

	float frameTime = gEnv->pTimer->GetFrameTime();

	m_pRayCaster->Update(frameTime);
	
	m_pFramework->PostUpdate( true, updateFlags );

	if(m_inDevMode != gEnv->pSystem->IsDevMode())
	{
		m_inDevMode = gEnv->pSystem->IsDevMode();
		m_pFramework->GetIActionMapManager()->EnableActionMap("debug", m_inDevMode);
	}
	
	CheckReloadLevel();

	return bRun ? 1 : 0;
}

void CGame::EditorResetGame(bool bStart)
{
	CRY_ASSERT(gEnv->IsEditor());

	if(bStart)
	{
		IActionMapManager* pAM = m_pFramework->GetIActionMapManager();
		if (pAM)
		{
			pAM->EnableActionMap(0, true); // enable all action maps
			pAM->EnableFilter(0, false); // disable all filters
		}		
	}	
}

void CGame::PlayerIdSet(EntityId playerId)
{
	m_uiPlayerID = playerId;	
}

string CGame::InitMapReloading()
{
	string levelFileName = GetIGameFramework()->GetLevelName();
	levelFileName = PathUtil::GetFileName(levelFileName);
	if(const char* visibleName = GetMappedLevelName(levelFileName))
		levelFileName = visibleName;
	//levelFileName.append("_levelstart.crysisjmsf"); //because of the french law we can't do this ...
	levelFileName.append("_crysis.crysisjmsf");
	if (m_pPlayerProfileManager)
	{
		const char* userName = GetISystem()->GetLoggedInUserName();
		IPlayerProfile* pProfile = m_pPlayerProfileManager->GetCurrentProfile(userName);
		if (pProfile)
		{
			const char* sharedSaveGameFolder = m_pPlayerProfileManager->GetSharedSaveGameFolder();
			if (sharedSaveGameFolder && *sharedSaveGameFolder)
			{
				string prefix = pProfile->GetName();
				prefix+="_";
				levelFileName = prefix + levelFileName;
			}
			ISaveGameEnumeratorPtr pSGE = pProfile->CreateSaveGameEnumerator();
			ISaveGameEnumerator::SGameDescription desc;	
			const int nSaveGames = pSGE->GetCount();
			for (int i=0; i<nSaveGames; ++i)
			{
				if (pSGE->GetDescription(i, desc))
				{
					if(!stricmp(desc.name,levelFileName.c_str()))
					{
						m_bReload = true;
						return levelFileName;
					}
				}
			}
		}
	}
#ifndef WIN32
	m_bReload = true; //using map command
#else
	m_bReload = false;
	levelFileName.clear();
#endif
	return levelFileName;
}

void CGame::Shutdown()
{
	if (m_pPlayerProfileManager)
	{
		m_pPlayerProfileManager->LogoutUser(m_pPlayerProfileManager->GetCurrentUser());

		if(m_pPlayerProfileManager->Shutdown())
			m_pPlayerProfileManager = NULL;
	}

	this->~CGame();
}

void CGame::BlockingProcess(BlockingConditionFunction f)
{
  INetwork* pNetwork = gEnv->pNetwork;

  bool ok = false;

  ITimer * pTimer = gEnv->pTimer;
  CTimeValue startTime = pTimer->GetAsyncTime();

  while (!ok)
  {
    pNetwork->SyncWithGame(eNGS_FrameStart);
    pNetwork->SyncWithGame(eNGS_FrameEnd);
    gEnv->pTimer->UpdateOnFrameStart();
    ok = (*f)();
  }
}







const static uint8 drmKeyData[16] = {0};
const static char* drmFiles = NULL;


const uint8* CGame::GetDRMKey()
{
	//CryLogAlways("CGame::GetDRMKey");

	return drmKeyData;
}

const char* CGame::GetDRMFileList()
{
	//CryLogAlways("CGame::GetDRMFileList");

	return drmFiles;
}

void CGame::LoadActionMaps(const char* filename)
{
	CRY_ASSERT_MESSAGE((filename || *filename != 0), "filename is empty!");
	if(g_pGame->GetIGameFramework()->IsGameStarted())
	{
		//CryLogAlways("Can't change configuration while game is running (yet)");
		return;
	}

	IActionMapManager *pActionMapMan = m_pFramework->GetIActionMapManager();

	pActionMapMan->AddInputDeviceMapping(eAID_KeyboardMouse, "keyboard");
	pActionMapMan->AddInputDeviceMapping(eAID_XboxPad, "xboxpad");
	pActionMapMan->AddInputDeviceMapping(eAID_PS3Pad, "ps3pad");

	// make sure that they are also added to the GameActions.actions file!
	XmlNodeRef rootNode = m_pFramework->GetISystem()->LoadXmlFromFile(filename);
	if(rootNode)
	{
		pActionMapMan->Clear();
		pActionMapMan->LoadFromXML(rootNode);
		m_pDefaultAM = pActionMapMan->GetActionMap("default");
		m_pDebugAM = pActionMapMan->GetActionMap("debug");
		m_pMultiplayerAM = pActionMapMan->GetActionMap("multiplayer");

		// enable defaults
		pActionMapMan->EnableActionMap("default",true);

		// enable debug
		pActionMapMan->EnableActionMap("debug",gEnv->pSystem->IsDevMode());

		// enable player action map
		pActionMapMan->EnableActionMap("player",true);
	}
	else
	{
		//CryLogAlways("[game] error: Could not open configuration file %s", filename);
		//CryLogAlways("[game] error: this will probably cause an infinite loop later while loading a map");
	}
}

void CGame::CheckReloadLevel()
{
	if(!m_bReload)
		return;

	if(gEnv->IsEditor() || gEnv->bMultiplayer)
	{
		if(m_bReload)
			m_bReload = false;
		return;
	}

#ifdef WIN32
	// Restart interrupts cutscenes
	gEnv->pMovieSystem->StopAllCutScenes();

	GetISystem()->SerializingFile(1);

	//load levelstart
	ILevelSystem* pLevelSystem = m_pFramework->GetILevelSystem();
	ILevel*			pLevel = pLevelSystem->GetCurrentLevel();
	ILevelInfo* pLevelInfo = pLevelSystem->GetLevelInfo(m_pFramework->GetLevelName());
	//**********
	EntityId playerID = GetIGameFramework()->GetClientActorId();
	pLevelSystem->OnLoadingStart(pLevelInfo);
	PlayerIdSet(playerID);
	string levelstart(GetIGameFramework()->GetLevelName());
	if(const char* visibleName = GetMappedLevelName(levelstart))
		levelstart = visibleName;
	//levelstart.append("_levelstart.crysisjmsf"); //because of the french law we can't do this ...
	levelstart.append("_crysis.crysisjmsf");
	GetIGameFramework()->LoadGame(levelstart.c_str(), true, true);
	//**********
	pLevelSystem->OnLoadingComplete(pLevel);
	m_bReload = false;	//if m_bReload is true - load at levelstart

	//if paused - start game
	m_pFramework->PauseGame(false, true);

	GetISystem()->SerializingFile(0);
#else
	string command("map ");
	command.append(m_pFramework->GetLevelName());
	gEnv->pConsole->ExecuteString(command);
#endif
}

void CGame::RegisterGameObjectEvents()
{
	IGameObjectSystem* pGOS = m_pFramework->GetIGameObjectSystem();

	pGOS->RegisterEvent(eCGE_PostFreeze, "PostFreeze");
	pGOS->RegisterEvent(eCGE_PostShatter,"PostShatter");
	pGOS->RegisterEvent(eCGE_OnShoot,"OnShoot");
	pGOS->RegisterEvent(eCGE_Recoil,"Recoil");
	pGOS->RegisterEvent(eCGE_BeginReloadLoop,"BeginReloadLoop");
	pGOS->RegisterEvent(eCGE_EndReloadLoop,"EndReloadLoop");
	pGOS->RegisterEvent(eCGE_ActorRevive,"ActorRevive");
	pGOS->RegisterEvent(eCGE_VehicleDestroyed,"VehicleDestroyed");
	pGOS->RegisterEvent(eCGE_TurnRagdoll,"TurnRagdoll");
	pGOS->RegisterEvent(eCGE_EnableFallAndPlay,"EnableFallAndPlay");
	pGOS->RegisterEvent(eCGE_DisableFallAndPlay,"DisableFallAndPlay");
	pGOS->RegisterEvent(eCGE_VehicleTransitionEnter,"VehicleTransitionEnter");
	pGOS->RegisterEvent(eCGE_VehicleTransitionExit,"VehicleTransitionExit");
	pGOS->RegisterEvent(eCGE_TextArea,"TextArea");
	pGOS->RegisterEvent(eCGE_InitiateAutoDestruction,"InitiateAutoDestruction");
	pGOS->RegisterEvent(eCGE_Event_Collapsing,"Event_Collapsing");
	pGOS->RegisterEvent(eCGE_Event_Collapsed,"Event_Collapsed");
	pGOS->RegisterEvent(eCGE_MultiplayerChatMessage,"MultiplayerChatMessage");
	pGOS->RegisterEvent(eCGE_ResetMovementController,"ResetMovementController");
	pGOS->RegisterEvent(eCGE_AnimateHands,"AnimateHands");
	pGOS->RegisterEvent(eCGE_Ragdoll,"Ragdoll");
	pGOS->RegisterEvent(eCGE_EnablePhysicalCollider,"EnablePhysicalCollider");
	pGOS->RegisterEvent(eCGE_DisablePhysicalCollider,"DisablePhysicalCollider");
	pGOS->RegisterEvent(eCGE_RebindAnimGraphInputs,"RebindAnimGraphInputs");
	pGOS->RegisterEvent(eCGE_OpenParachute, "OpenParachute");

}

void CGame::GetMemoryStatistics(ICrySizer * s) const
{
	s->Add(*this);

	if (m_pPlayerProfileManager)
	  m_pPlayerProfileManager->GetMemoryUsage(s);
}

void CGame::DumpMemInfo(const char* format, ...)
{
	CryModuleMemoryInfo memInfo;
	CryGetMemoryInfoForModule(&memInfo);

	va_list args;
	va_start(args,format);
	gEnv->pLog->LogV( ILog::eAlways,format,args );
	va_end(args);

	gEnv->pLog->LogWithType( ILog::eAlways, "Alloc=%I64d kb  String=%I64d kb  STL-alloc=%I64d kb  STL-wasted=%I64d kb", (memInfo.allocated - memInfo.freed) >> 10 , memInfo.CryString_allocated >> 10, memInfo.STL_allocated >> 10 , memInfo.STL_wasted >> 10);
	// gEnv->pLog->LogV( ILog::eAlways, "%s alloc=%llu kb  instring=%llu kb  stl-alloc=%llu kb  stl-wasted=%llu kb", text, memInfo.allocated >> 10 , memInfo.CryString_allocated >> 10, memInfo.STL_allocated >> 10 , memInfo.STL_wasted >> 10);
}

const char* CGame::GetMappedLevelName(const char *levelName) const
{ 
	TLevelMapMap::const_iterator iter = m_mapNames.find(CONST_TEMP_STRING(levelName));
	return (iter == m_mapNames.end()) ? levelName : iter->second.c_str();
}

#include UNIQUE_VIRTUAL_WRAPPER(IGame)