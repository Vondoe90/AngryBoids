/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: 
  
 -------------------------------------------------------------------------
  History:
  - 3:8:2004   11:23 : Created by Márcio Martins

*************************************************************************/
#ifndef __GAME_H__
#define __GAME_H__

#if _MSC_VER > 1000
# pragma once
#endif

#include <IGame.h>
#include <IGameFramework.h>
#include <IGameObjectSystem.h>
#include <IGameObject.h>
#include <IActorSystem.h>
#include <StlUtils.h>
#include "RayCastQueue.h"

#include "GameSettings.h"

struct ISystem;
struct IConsole;

struct IActionMap;
struct IActionFilter;
class CCameraManager;

// when you add stuff here, also update in CGame::RegisterGameObjectEvents
enum ECryGameEvent
{
	eCGE_PreFreeze = eGFE_PreFreeze,	// this is really bad and must be fixed
	eCGE_PreShatter = eGFE_PreShatter,
	eCGE_PostFreeze = 256,
	eCGE_PostShatter,
	eCGE_OnShoot,
	eCGE_Recoil, 
	eCGE_BeginReloadLoop,
	eCGE_EndReloadLoop,
	eCGE_ActorRevive,
	eCGE_VehicleDestroyed,
	eCGE_TurnRagdoll,
	eCGE_EnableFallAndPlay,
	eCGE_DisableFallAndPlay,
	eCGE_VehicleTransitionEnter,
	eCGE_VehicleTransitionExit,
	eCGE_TextArea,
	eCGE_InitiateAutoDestruction,
	eCGE_Event_Collapsing,
	eCGE_Event_Collapsed,
	eCGE_MultiplayerChatMessage,
	eCGE_ResetMovementController,
	eCGE_AnimateHands,
	eCGE_Ragdoll,
	eCGE_EnablePhysicalCollider,
	eCGE_DisablePhysicalCollider,
	eCGE_RebindAnimGraphInputs,
	eCGE_OpenParachute,
  eCGE_Turret_LockedTarget,
  eCGE_Turret_LostTarget,
};

static const int GLOBAL_SERVER_IP_KEY						=	1000;
static const int GLOBAL_SERVER_PUBLIC_PORT_KEY	= 1001;
static const int GLOBAL_SERVER_NAME_KEY					=	1002;

class CGame : public IGame
{
public:
  typedef bool (*BlockingConditionFunction)();
  typedef RayCastQueue<41> GlobalRayCaster;
public:
	CGame();
	VIRTUAL ~CGame();

	// IGame
	VIRTUAL bool  Init(IGameFramework *pFramework);
	VIRTUAL bool  CompleteInit();
	VIRTUAL void  Shutdown();
	VIRTUAL int   Update(bool haveFocus, unsigned int updateFlags);
	VIRTUAL void  ConfigureGameChannel(bool isServer, IProtocolBuilder *pBuilder) {}
	VIRTUAL void  EditorResetGame(bool bStart);
	VIRTUAL void  PlayerIdSet(EntityId playerId);
	VIRTUAL string  InitMapReloading();
	VIRTUAL bool IsReloading() { return m_bReload; }
	VIRTUAL IGameFramework *GetIGameFramework() { return m_pFramework; }

	VIRTUAL const char *GetLongName() { return GAME_TITLE; }
	VIRTUAL const char *GetName() { return GAME_TITLE; }

	VIRTUAL void GetMemoryStatistics(ICrySizer * s) const;

	VIRTUAL void OnClearPlayerIds() {}
	//auto-generated save game file name
	VIRTUAL IGame::TSaveGameName CreateSaveGameName() { return CRY_SAVEGAME_FILENAME + (TSaveGameName)CRY_SAVEGAME_FILE_EXT; }
	//level names were renamed without changing the file/directory
	VIRTUAL const char* GetMappedLevelName(const char *levelName) const;

	virtual IGameStateRecorder* CreateGameStateRecorder(IGameplayListener* pL)  { return NULL; } 
	// 
	VIRTUAL const bool DoInitialSavegame() const { return true; }

	VIRTUAL void CreateLobbySession( const SGameStartParams* pGameStartParams ) {;}
	VIRTUAL void DeleteLobbySession() {;}

	// ~IGame

  void BlockingProcess(BlockingConditionFunction f);

	VIRTUAL uint32 AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener = NULL) { return 1; }
	VIRTUAL void RenderGameWarnings() {}
	VIRTUAL void RemoveGameWarning(const char* stringId) {}
	VIRTUAL bool GameEndLevel(const char* stringId) { return false; }
	VIRTUAL void OnRenderScene() {}

	VIRTUAL const uint8* GetDRMKey();
	VIRTUAL const char* GetDRMFileList();

	// camera stuff (new tp cam)
	CCameraManager *GetCameraManager() { return m_pCameraManager; }
  ILINE GlobalRayCaster& GetRayCaster() { assert(m_pRayCaster); return *m_pRayCaster; }

	static void DumpMemInfo(const char* format, ...) PRINTF_PARAMS(1, 2);

	VIRTUAL void LoadActionMaps(const char* filename);

protected:
	VIRTUAL void CheckReloadLevel();

	VIRTUAL void RegisterGameObjectEvents();

	// marcok: this is bad and evil ... should be removed soon
	static void CmdRestartGame(IConsoleCmdArgs *pArgs);
#ifndef _RELEASE
	static void CmdDumpAmmoPoolStats(IConsoleCmdArgs *pArgs);
	static void CmdDumpSS(IConsoleCmdArgs *pArgs);
#endif

	static void CmdLastInv(IConsoleCmdArgs *pArgs);
	static void CmdName(IConsoleCmdArgs *pArgs);
	static void CmdTeam(IConsoleCmdArgs *pArgs);
	static void CmdLoadLastSave(IConsoleCmdArgs *pArgs);
	static void CmdSpectator(IConsoleCmdArgs *pArgs);
	static void CmdJoinGame(IConsoleCmdArgs *pArgs);
	static void CmdKill(IConsoleCmdArgs *pArgs);
  static void CmdVehicleKill(IConsoleCmdArgs *pArgs);
	static void CmdRestart(IConsoleCmdArgs *pArgs);
	static void CmdSay(IConsoleCmdArgs *pArgs);
	static void CmdReloadItems(IConsoleCmdArgs *pArgs);
	static void CmdLoadActionmap(IConsoleCmdArgs *pArgs);
  static void CmdReloadGameRules(IConsoleCmdArgs *pArgs);
  static void CmdReloadScripts(IConsoleCmdArgs *pArgs);
  static void CmdNextLevel(IConsoleCmdArgs* pArgs);
  static void CmdStartKickVoting(IConsoleCmdArgs* pArgs);
  static void CmdStartNextMapVoting(IConsoleCmdArgs* pArgs);
  static void CmdVote(IConsoleCmdArgs* pArgs);

  static void CmdQuickGame(IConsoleCmdArgs* pArgs);
  static void CmdQuickGameStop(IConsoleCmdArgs* pArgs);
  static void CmdBattleDustReload(IConsoleCmdArgs* pArgs);
  static void CmdLogin(IConsoleCmdArgs* pArgs);
	static void CmdLoginProfile(IConsoleCmdArgs* pArgs);
  static void CmdCryNetConnect(IConsoleCmdArgs* pArgs);
#ifndef _RELEASE
	static void CmdTestPathfinder(IConsoleCmdArgs* pArgs);
#endif
	















	IGameFramework			*m_pFramework;
	IConsole						*m_pConsole;

	bool								m_bReload;

	//menus

	IActionMap					*m_pDebugAM;
	IActionMap					*m_pDefaultAM;
	IActionMap					*m_pMultiplayerAM;
	IPlayerProfileManager* m_pPlayerProfileManager;

	bool								m_inDevMode;

	EntityId m_uiPlayerID;

	string                 m_lastSaveGame;

  GlobalRayCaster* m_pRayCaster;

	typedef std::map<string, string, stl::less_stricmp<string> > TLevelMapMap;
	TLevelMapMap m_mapNames;

	// new tp camera stuff
	CCameraManager *m_pCameraManager;
};

extern CGame *g_pGame;

#define SAFE_HARDWARE_MOUSE_FUNC(func)\
	if(gEnv->pHardwareMouse)\
		gEnv->pHardwareMouse->func

#endif //__GAME_H__