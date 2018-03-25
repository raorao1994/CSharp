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

/*С�Ʋ���*/
double minTh = 0.1;//0.025��ֵ
int aroundPix = 30;//��������ֵ
//int x = 258, y = 123;//��ͼ����Ļ���ϽǾ���
int x = 171, y = 50;//��ͼ����Ļ���ϽǾ���
int myX = 140, myY = 170, myW = 800, myH = 210;

/*���Ʋ���*/
//double minTh = 0.06;//0.025��ֵ
//int aroundPix = 30;//��������ֵ
//int x = 171, y = 50;//��ͼ����Ļ���ϽǾ���
//int myX = 120, myY = 370, myW = 800, myH = 110;



int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);

#pragma region ��һ��
	////0����ȡ����ģ��
//GetAllTemp();
////1����ȡͼƬ�ļ�
//QueryPerformanceCounter(&start_t);
//Mat src = imread(imgPath);
//Mat gray;
//cvtColor(src, gray, CV_BGR2GRAY);
//vector<string> str=Recognition(gray, src);
////������������ʱ
//QueryPerformanceCounter(&stop_t);
//exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
//cout << "��ʱ" << exe_time << "����"<<endl;
//imshow("ԭͼ", src);
//waitKey(0);
//   return 0;  
#pragma endregion


	//#pragma region �ڶ���
	//1��ʵ����������
	//Helper helper = Helper(aroundPix, minTh);
	////2������ģ���ļ����ڴ�
	//helper.GetAllTemp("E:\\SVN\\CShap\\trunk\\ChessProject\\img\\1024\\temp2");
	////3����ͼʶ��
	//Mat ScreenImg,gary;
	//HBITMAP bitMap;
	//string str = "";
	//while (true)
	//{
	//	str = "";
	//	QueryPerformanceCounter(&start_t);
	//	//��ȡ��Ļ
	//	bitMap = helper.CopyScreenToBitmap();
	//	//ת����Mat����
	//	helper.HBitmapToMat(bitMap, ScreenImg);
	//	//��ĻMat����ת�Ҷ�ͼ
	//	cvtColor(ScreenImg, gary, CV_BGR2GRAY);
	//	Rect rect = Rect(x+myX, y+myY, myW, myH);//254, 121, 856, 556
	//	gary = helper.CutImg(gary, rect);
	//	//ʶ��ͼ��
	//	vector<string> lables= helper.Recognition(gary, gary);
	//	cout << "ʶ���:" << lables.size() << "����" << endl;
	//	for (size_t i = 0; i < lables.size(); i++)
	//	{
	//		str.append(lables[i]).append(",");
	//	}
	//	cout << "��Ϊ:" << str << endl;
	//	//��ʱ����
	//	QueryPerformanceCounter(&stop_t);
	//	exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
	//	cout << "��ʱ" << exe_time << "����" << endl;
	//	imshow("ScteenImg", gary);
	//	waitKey(10);
	//	//�ͷ��ڴ�
	//}
	//waitKey(0);
	//return 0;  
	#pragma endregion


	//1��ʵ����������
	Helper helper = Helper(aroundPix, minTh);
	//2������ģ���ļ����ڴ�
	helper.GetAllTemp("E:\\SVN\\CShap\\trunk\\ChessProject\\img\\1024\\temp1");
	//3����ͼʶ��
	Mat ScreenImg, gary;
	HBITMAP bitMap;
	string str = "";
	Rect my = Rect(145 + 350, 50 + 278, 440, 60);
	Rect percoius = Rect(145 + 140, 50 + 188, 440, 60);
	Rect next = Rect(145 + 650, 50 + 188, 440, 60);
	while (true)
	{
		str = "";
		QueryPerformanceCounter(&start_t);
		//��ȡ��Ļ
		bitMap = helper.CopyScreenToBitmap();
		//ת����Mat����
		helper.HBitmapToMat(bitMap, ScreenImg);
		//��ĻMat����ת�Ҷ�ͼ
		cvtColor(ScreenImg, gary, CV_BGR2GRAY);
		//helper.RecognitionCards(gary, my, percoius, next);
		//��ʱ����
		QueryPerformanceCounter(&stop_t);
		exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
		cout << "��ʱ" << exe_time << "����" << endl;
		imshow("ScteenImg", gary);
		waitKey(10);
		//�ͷŽ�ͼBITMAP�ڴ���Դ
		DeleteObject(bitMap);
	}
	waitKey(0);
	return 0;
}