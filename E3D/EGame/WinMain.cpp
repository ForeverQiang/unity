#include "EGraphics.h"
#include "EWindow.h"
#include "EMeshUtil.h"
#include "EUtil.h"
#include "EGame.h"
//#include "../res/resource.h"

//using namespace Eyas3D;
namespace E3D
{
	int WINAPI winMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
	{
		InitLog("Eyase3D.Log");

		Log("Init Graphics...");
		EGraphics::initGraphics(hInstance);
		Log("Grahpics Load Successed!");

		EGameWindow::GWindow = new EGameWindow("Eyas3D[3DTankWar]", hInstance);
		EGameWindow::GWindow->showWindow(true);

		//设置游戏的小图标
		HWND hwnd = EGameWindow::GWindow->getHWnd();
		LONG iconID = (LONG)LoadIcon(::GetModuleHandle(0), MAKEINTRESOURCE(IDI_ICON_TANKWAR));
		::SetClassLong(hwnd, GCL_HICON, iconID);
		EGame *game = new EGame;
		EGameWindow::GWindow->setCanvasListener(game);
		EGameWindow::GWindow->addInputListener(game);
		EGameWindow::GWindow->startLoop();

		Log("Shutdown Graphics...");
		EGraphics::shutdownGraphics();

		CloseLog();

		return 0;
	}
}
