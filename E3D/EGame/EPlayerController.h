#pragma once
#include "ETank.h"
#include "EGameManager.h"

namespace E3D
{
	enum DIRECTION
	{
		DIR_UP = 0x000001 << 1, //上
		DIR_DOWN = 0x000001 << 2, //下
		DIR_LEFT = 0x000001 << 4, //左
		DIR_RIGHT = 0x000001 << 8,	//右
	};

	class ESceneManager;
	class EGameManager;
	class EPlayerController
	{
	public:

		~EPlayerController();

		//设置/获取当前角色坐标
		void setPosition(const EVector3D& pos);
		EVector3D getPosition() const { return mTank->getPosition(); }

		//获取控制的Tank
		ETank* getTank() { return mTank; }

		void update();

		void keyPress(EInt key);
		void keyRlease(EInt key);
		void mouseWheel(EInt delta){}
		void mouseButtonDown(EInt mouseButton){}
		void mouseButtonRelease(EInt mouseButton){}
		void mouseMove(EInt x,EInt y){}

	private:

		friend class EGameManager;
		EPlayerController(EGameManager* gameMgr);

	protected:
		ESceneManager * mScene;
		EGameManager *mGameMgr;
		ETank *mTank;
		ECamera *mCamera;
		EInt mCurDir;

		EInt mFireDir;
	};
}
