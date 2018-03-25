// RecognitionCards.cpp : 定义控制台应用程序的入口点。
//识别扑克牌
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

/*小牌参数*/
double minTh = 0.1;//0.025阀值
int aroundPix = 30;//单张像素值
//int x = 258, y = 123;//整图距屏幕右上角距离
int x = 171, y = 50;//整图距屏幕右上角距离
int myX = 140, myY = 170, myW = 800, myH = 210;

/*大牌参数*/
//double minTh = 0.06;//0.025阀值
//int aroundPix = 30;//单张像素值
//int x = 171, y = 50;//整图距屏幕右上角距离
//int myX = 120, myY = 370, myW = 800, myH = 110;



int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);

#pragma region 第一版
	////0、获取所有模版
//GetAllTemp();
////1、读取图片文件
//QueryPerformanceCounter(&start_t);
//Mat src = imread(imgPath);
//Mat gray;
//cvtColor(src, gray, CV_BGR2GRAY);
//vector<string> str=Recognition(gray, src);
////结束，计算用时
//QueryPerformanceCounter(&stop_t);
//exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
//cout << "耗时" << exe_time << "毫秒"<<endl;
//imshow("原图", src);
//waitKey(0);
//   return 0;  
#pragma endregion


	//#pragma region 第二版
	//1、实例化操作类
	//Helper helper = Helper(aroundPix, minTh);
	////2、加载模版文件到内存
	//helper.GetAllTemp("E:\\SVN\\CShap\\trunk\\ChessProject\\img\\1024\\temp2");
	////3、截图识别
	//Mat ScreenImg,gary;
	//HBITMAP bitMap;
	//string str = "";
	//while (true)
	//{
	//	str = "";
	//	QueryPerformanceCounter(&start_t);
	//	//截取屏幕
	//	bitMap = helper.CopyScreenToBitmap();
	//	//转换成Mat对象
	//	helper.HBitmapToMat(bitMap, ScreenImg);
	//	//屏幕Mat对象转灰度图
	//	cvtColor(ScreenImg, gary, CV_BGR2GRAY);
	//	Rect rect = Rect(x+myX, y+myY, myW, myH);//254, 121, 856, 556
	//	gary = helper.CutImg(gary, rect);
	//	//识别图像
	//	vector<string> lables= helper.Recognition(gary, gary);
	//	cout << "识别出:" << lables.size() << "张牌" << endl;
	//	for (size_t i = 0; i < lables.size(); i++)
	//	{
	//		str.append(lables[i]).append(",");
	//	}
	//	cout << "牌为:" << str << endl;
	//	//计时结束
	//	QueryPerformanceCounter(&stop_t);
	//	exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
	//	cout << "耗时" << exe_time << "毫秒" << endl;
	//	imshow("ScteenImg", gary);
	//	waitKey(10);
	//	//释放内存
	//}
	//waitKey(0);
	//return 0;  
	#pragma endregion


	//1、实例化操作类
	Helper helper = Helper(aroundPix, minTh);
	//2、加载模版文件到内存
	helper.GetAllTemp("E:\\SVN\\CShap\\trunk\\ChessProject\\img\\1024\\temp1");
	//3、截图识别
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
		//截取屏幕
		bitMap = helper.CopyScreenToBitmap();
		//转换成Mat对象
		helper.HBitmapToMat(bitMap, ScreenImg);
		//屏幕Mat对象转灰度图
		cvtColor(ScreenImg, gary, CV_BGR2GRAY);
		//helper.RecognitionCards(gary, my, percoius, next);
		//计时结束
		QueryPerformanceCounter(&stop_t);
		exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
		cout << "耗时" << exe_time << "毫秒" << endl;
		imshow("ScteenImg", gary);
		waitKey(10);
		//释放截图BITMAP内存资源
		DeleteObject(bitMap);
	}
	waitKey(0);
	return 0;
}