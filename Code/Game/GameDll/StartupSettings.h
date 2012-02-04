#ifndef __STARTUP_SETTINGS_H__
#define __STARTUP_SETTINGS_H__

#include <CryLibrary.h>

#ifndef XENON
#define DLL_INITFUNC_CREATEGAMEFRAMEWORK "CreateGameFramework"
#endif

/*
 * this section makes sure that the framework dll is loaded and cleaned up
 * at the appropriate time
 */

#if defined(LINUX)
#define GAME_FRAMEWORK_FILENAME	"CryAction.so"
#else
#define GAME_FRAMEWORK_FILENAME	"cryaction.dll"
#endif 
#define GAME_WINDOW_CLASSNAME		"CryENGINE"

#if !defined(_LIB) && !defined(PS3)

static HMODULE s_frameworkDLL;

static void CleanupFrameworkDLL()
{
	assert( s_frameworkDLL );
	CryFreeLibrary( s_frameworkDLL );
	s_frameworkDLL = 0;
}

extern HMODULE GetFrameworkDLL(const char* binariesDir);
IGameFramework *GetFramework(const char* binariesDir);
#endif // !defined(_LIB) && !defined(PS3)



#endif __STARTUP_SETTINGS_H__