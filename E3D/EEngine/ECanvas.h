#pragma once
#include "EGraphics.h"

namespace E3D
{
	//��ͼ�ӿ�
	class ECanvas
	{
	public:
		~ECanvas() {}

		//����ֻ�������ģ��
		virtual void onPaint() = 0;

		//������³���
		virtual void update() = 0;
	};
}
