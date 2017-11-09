#pragma once

#include "ETank.h"


namespace E3D
{
	//ʵ�ּ�AI��Tank
	class EAITank : public ETank
	{
	public:

		virtual	~EAITank();

		virtual void onHited();
		//����
		virtual void fire();
		//����Tank
		virtual void update();

	protected:

		//����AI
		void updateAI();

	protected:
		friend class EGameManager;
		friend class EPlayerController;

		EAITank(const EString& name, const EString& meshName, EGameManager* gameMgr);

	};
}