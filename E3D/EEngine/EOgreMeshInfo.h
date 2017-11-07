#pragma once

namespace E3D
{
	//Orge Mesh�ļ������Ϣ

	//Orge Mesh�������ļ��е�IDö�٣���Դ��Orge
	enum OGREMESHID
	{
		//�޷��Ŷ�����
		M_HEADER = 0x0010,
		//�汾��Ϣ
		M_MESH = 0x3000,
		//��������Ϣ
		M_SUBMESH = 0x4000,
		//M_GEROMESH�飨����useSharedVertices = falseʱ�Ż���֣�
		M_GEOMETRY = 0x5000,//������Ǳ�Ƕ�����������������
		//��������
		M_GEOMETRY_VERTEX_DECLARATION = 0x5100,
		M_GEOMETRY_VERTEX_ELEMENT = 0x5110,//�ظ�����
		M_GEOMETRY_VERTEX_BUFFER = 0x5200, //�ظ�������
		M_GEOMRTRY_VERTEX_BUFFER_DATA=0x5210,//ԭ��������

		M_SUBMESH_OPERATION = 0x4010,//��ѡ��

		M_MESH_BOUNDS = 0x9000,

		M_SUBMESH_NAME_TABLE = 0xA000,
		//�ӿ������б�ÿ���ӿ�����������ַ���
		M_SUBMESH_NAME_TABLE_ELEMENT = 0xA100,
	};

	enum OperationType
	{
		//����б�
		OT_PPOINT_LIST = 1,
		//���б�һ���߰�����������
		OT_LINT_LIST = 2,
		//��״������
		OT_LINE_STRIP = 3,
		//�������б�
		OT_TRIANGLE_LIST = 4,
		//����������
		OT_TRIANGLE_STRIP = 5,
		//��������
		OT_TRIANGLE_FAN = 6
	};
}
