using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������������� ���˴�����ʱ���һЩ���� 
public class CreateEnemyModel
{
    public List<CreateEnemyProperty> list = new List<CreateEnemyProperty>();
}

//�洢����һ����������Ҫ������
public class CreateEnemyProperty
{
    public string guid;//��ʾһ�����˵�GUID
    public string prefabName;//����prefab��name�ҵ�prefab��������
    public Vector3Obj position;//��ʾ����������prefab
    public int targetRoleID;
}