#pragma once

#include "ECommon.h"
#include "EMesh.h"
#include "ELight.h"
#include "ECore.h"
#include "ECamera.h"
#include <map>

namespace E3D
{
	class ESceneManager
	{
		typedef std::map<EString, EMesh*>::iterator MeshIter;
	public:
		ESceneManager();
		~ESceneManager();

		//��������
		void drawString(const EString& str, EInt x, EInt y, const EColor &c = EColor(255, 255, 255));
		//����һ��ģ��ʵ��
		EMesh* CreateMesh(const EString& name, const EString& meshName);
		//��ȡָ����ģ��ʵ��
		EMesh* getMesh(const EString& name);
		//�����ƶ���ģ��ʵ�� 
		void destroyMesh(const EString& name);

		void clearMesh();

		//ָ������/�رյƹ⣨ȫ�֣�
		void  enableLights(EBool enable);
		//��ǰ�Ƿ����ƹ⣨ȫ�֣�
		EBool isLightsEnable() const { return mLightEnable; }
		//����һյ��
		ELight* createLight(LIGHT_TYPE lightType);
		//��ȡһյ�ƹ⣬����ƹⲻ���ڣ��򷵻�NULL
		ELight* getLight(EInt id);

		//��ȡ�����ָ��
		ECamera* getCamera() { return mCamera; }
		//��ȡ���У��ɼ�/���ɼ����������
		EInt getTotalObjectNumber() const { return mMeshs.size(); }	
		//��ȡ��ǰ�ɼ��������
		EInt getVisibleObjectNumber() const { return mVisibleObjectNumber; }
		//��ȡ��ǰ�ɼ�����θ���
		EInt getVisiblePolyonNumber() const { return mVisiblePolyonNumber; }
		//��ȡ���У��ɼ�/���ɼ�������θ���
		EInt getTotalPolyonNumber() const { return mTotalPolyonNumber; }

		//��������
		EMesh* createTerrain(const EString& textureFileName, const EString& heightMapFileName, EFloat uTitle = 1, EFloat vTitle = 1, EFloat blockSize = 5.0f);
		EMesh* getTerrain() { return mTerrainMesh; }

		//����
		void update();


	protected:
		std::map<EString, EMesh*> mMeshs;
		std::map<EString, EMesh*> mUsedMesh;
		ERenderList4D *mRenderList;

		EMesh *mTerrainMesh;

		ECamera *mCamera;
		EBool mLightEnable;

		EInt mVisibleObjectNumber;
		EInt mVisiblePolyonNumber;
		EInt mTotalPolyonNumber;

	};
}