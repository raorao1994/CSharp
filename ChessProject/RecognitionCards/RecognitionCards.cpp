// RecognitionCards.cpp : �������̨Ӧ�ó������ڵ㡣
//ʶ���˿���
#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/core.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include <io.h>
#include <map>
#include <fstream>
#include <windows.h>
#include "Helper.h"

using namespace std;
using namespace cv;

/*����*/
double minTh = 0.12;//С�Ʒ�ֵ
double maxTh = 0.06;//���Ʒ�ֵ
int aroundPix = 30;//��������ֵ
//int x = 0, y = 35;//��ͼ����Ļ���ϽǾ���
int x = 256, y = 122;
//1024
//int myX = 350, myY = 278, myW = 440, myH = 60;//�ҵ��������ͼƬλ��
//int peX = 140, peY = 188, peW = 440, peH = 60;//�ϼҵ��������ͼƬλ��
//int neX = 650, neY = 188, neW = 440, neH = 60;//�¼��������ͼƬλ��
//int haX = 120, haY = 390, haW = 750, haH = 70;//�ҵ����������ͼƬλ��
//int stX = 470, stY = 230, stW = 85, stH = 117;//��ʼ���������ͼƬλ��
//850
int myX = 300, myY = 230, myW = 300, myH = 55;//�ҵ��������ͼƬλ��
int peX = 120, peY = 157, peW = 300, peH = 55;//�ϼҵ��������ͼƬλ��
int neX = 450, neY = 164, neW = 300, neH = 55;//�¼��������ͼƬλ��
int haX = 100, haY = 300, haW = 600, haH = 85;//�ҵ����������ͼƬλ��
int stX = 470, stY = 230, stW = 85, stH = 117;//��ʼ���������ͼƬλ��
//10241
//int myX = 300, myY = 230, myW = 300, myH = 120;//�ҵ��������ͼƬλ��
//int peX = 140, peY = 90, peW = 270, peH = 120;//�ϼҵ��������ͼƬλ��
//int neX = 450, neY = 90, neW = 270, neH = 120;//�¼��������ͼƬλ��
//int haX = 300, haY = 300, haW = 600, haH = 85;//�ҵ����������ͼƬλ��
//int stX = 470, stY = 230, stW = 85, stH = 117;//��ʼ���������ͼƬλ��

int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);

	//1��ʵ����������
	Helper helper = Helper(aroundPix, minTh);
	string temp = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\temp1";//�����ʶ��ģ��
	string temp2 = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\temp2";//������ģ��
	string temp3 = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\start.png";//������ģ��
	//2������ģ���ļ����ڴ�
	helper.GetAllTemp(temp);
	vector<Mat> myTemp = helper.GetAllMyTemp(temp2);
	helper.InitCards();
	//3����ͼʶ��
	Mat ScreenImg, gary;
	HBITMAP bitMap;
	Rect my = Rect(x + myX, y + myY, myW, myH);
	Rect percoius = Rect(x + peX, y + peY, peW, peH);
	Rect next = Rect(x + neX, y + neY, neW, neH);
	Rect ha = Rect(x + haX, y + haY, haW, haH);
	Rect st = Rect(x + stX, y + stY, stW, stH);
	while (true)
	{
		//��ʼ��ʱ
		QueryPerformanceCounter(&start_t);
		//��ȡ��Ļ
		bitMap = helper.CopyScreenToBitmap();
		//ת����Mat����
		helper.HBitmapToMat(bitMap, ScreenImg);
		//��ĻMat����ת�Ҷ�ͼ
		cvtColor(ScreenImg, gary, CV_BGR2GRAY);
		//�ж��Ƿ����¿�ʼ
		Mat haImg = Mat(gary, ha);
		vector<string> _lables = helper.Recognitions(haImg, myTemp, maxTh);
		if (_lables.size() == 16&& helper.PlayCardsCount==0)
		{
			helper.InitCards();
			helper.outputStr = "���е��ţ�"+helper.vectorToString(_lables);
			helper.CountCards(_lables);
		}
		if (_lables.size() == 0)
		{
			helper.InitCards();
			helper.PlayCardsCount = 0;
			helper.outputStr = "��Ϸ��ʼ������";
		}
		haImg.release();
		//ʶ��������
		helper.RecognitionCards(gary, my, percoius, next);
		helper.ShowCards();
		//��ʱ����
		QueryPerformanceCounter(&stop_t);
		exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
		cout << "��ʱ" << exe_time << "����" << endl;
		//imshow("ScteenImg", gary);
		waitKey(10);
		//�ͷŽ�ͼBITMAP�ڴ���Դ
		DeleteObject(bitMap);
	}
	waitKey(0);
	return 0;
}