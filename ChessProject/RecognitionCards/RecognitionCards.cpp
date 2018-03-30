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

/*参数*/
double minTh = 0.12;//小牌阀值
double maxTh = 0.06;//大牌阀值
int aroundPix = 30;//单张像素值
//int x = 0, y = 35;//整图距屏幕右上角距离
int x = 256, y = 122;
//1024
//int myX = 350, myY = 278, myW = 440, myH = 60;//我的牌相对于图片位置
//int peX = 140, peY = 188, peW = 440, peH = 60;//上家的牌相对于图片位置
//int neX = 650, neY = 188, neW = 440, neH = 60;//下家牌相对于图片位置
//int haX = 120, haY = 390, haW = 750, haH = 70;//我的手牌相对于图片位置
//int stX = 470, stY = 230, stW = 85, stH = 117;//开始打牌相对于图片位置
//850
int myX = 300, myY = 230, myW = 300, myH = 55;//我的牌相对于图片位置
int peX = 120, peY = 157, peW = 300, peH = 55;//上家的牌相对于图片位置
int neX = 450, neY = 164, neW = 300, neH = 55;//下家牌相对于图片位置
int haX = 100, haY = 300, haW = 600, haH = 85;//我的手牌相对于图片位置
int stX = 470, stY = 230, stW = 85, stH = 117;//开始打牌相对于图片位置
//10241
//int myX = 300, myY = 230, myW = 300, myH = 120;//我的牌相对于图片位置
//int peX = 140, peY = 90, peW = 270, peH = 120;//上家的牌相对于图片位置
//int neX = 450, neY = 90, neW = 270, neH = 120;//下家牌相对于图片位置
//int haX = 300, haY = 300, haW = 600, haH = 85;//我的手牌相对于图片位置
//int stX = 470, stY = 230, stW = 85, stH = 117;//开始打牌相对于图片位置

int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);

	//1、实例化操作类
	Helper helper = Helper(aroundPix, minTh);
	string temp = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\temp1";//打出牌识别模版
	string temp2 = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\temp2";//手中牌模版
	string temp3 = "E:\\SVN\\CShap\\trunk\\ChessProject\\img\\850\\start.png";//手中牌模版
	//2、加载模版文件到内存
	helper.GetAllTemp(temp);
	vector<Mat> myTemp = helper.GetAllMyTemp(temp2);
	helper.InitCards();
	//3、截图识别
	Mat ScreenImg, gary;
	HBITMAP bitMap;
	Rect my = Rect(x + myX, y + myY, myW, myH);
	Rect percoius = Rect(x + peX, y + peY, peW, peH);
	Rect next = Rect(x + neX, y + neY, neW, neH);
	Rect ha = Rect(x + haX, y + haY, haW, haH);
	Rect st = Rect(x + stX, y + stY, stW, stH);
	while (true)
	{
		//开始计时
		QueryPerformanceCounter(&start_t);
		//截取屏幕
		bitMap = helper.CopyScreenToBitmap();
		//转换成Mat对象
		helper.HBitmapToMat(bitMap, ScreenImg);
		//屏幕Mat对象转灰度图
		cvtColor(ScreenImg, gary, CV_BGR2GRAY);
		//判断是否重新开始
		Mat haImg = Mat(gary, ha);
		vector<string> _lables = helper.Recognitions(haImg, myTemp, maxTh);
		if (_lables.size() == 16&& helper.PlayCardsCount==0)
		{
			helper.InitCards();
			helper.outputStr = "手中的排："+helper.vectorToString(_lables);
			helper.CountCards(_lables);
		}
		if (_lables.size() == 0)
		{
			helper.InitCards();
			helper.PlayCardsCount = 0;
			helper.outputStr = "游戏开始！！！";
		}
		haImg.release();
		//识别打出的牌
		helper.RecognitionCards(gary, my, percoius, next);
		helper.ShowCards();
		//计时结束
		QueryPerformanceCounter(&stop_t);
		exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
		cout << "耗时" << exe_time << "毫秒" << endl;
		//imshow("ScteenImg", gary);
		waitKey(10);
		//释放截图BITMAP内存资源
		DeleteObject(bitMap);
	}
	waitKey(0);
	return 0;
}