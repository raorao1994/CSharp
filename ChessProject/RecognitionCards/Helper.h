#pragma once
#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>
#include <io.h>
#include <map>
#include <fstream>

using namespace std;
using namespace cv;

class Helper
{
public:
	//打出牌模版图片列表
	vector<Mat> TempImgs;
	//打出牌模版图片对应lable列表
	vector<string> Lables;
	//扑克牌文字间隔像素
	int aroundPix = 30;
	//精确度阀值
	double minTh = 0.04;//0.025
	//所有扑克牌
	map<string, int> Cards;
	//输出识别的文字
	string outputStr = "";
	int PlayCardsCount = 0;//打出的牌的数量
	//打出牌识别变量
	Mat myImg, previousImg, nextImg;
	string myStr="", previousStr = "", nextStr = "";
	int myCount=0, previousCount = 0, nextCount = 0,lableCount=3;//lableCount判断重复计算


	Helper(int Pix=50, double minTh=0.04);
	~Helper();
	/*字符串转LPCWSTR*/
	LPCWSTR stringToLPCWSTR(string orig);
	/*截取屏幕图像*/
	HBITMAP CopyScreenToBitmap();
	/*保存Bit屏幕图像*/
	BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);
	/*获取所有模版文件*/
	void GetAllTemp(string path);
	vector<Mat> GetAllMyTemp(string path);
	/*识别图像中的打出的牌*/
	vector<string> Recognition(Mat img, Mat src);
	vector<string> Recognitions(Mat gary, vector<Mat> tempImgs, double minThs);
	/*识别模版文件是否存在*/
	bool HaveTemp(Mat img, string tempPath, double minThs);
	/*Mat图像转HBitmap图像*/
	BOOL MatToHBitmap(HBITMAP& _hBmp, Mat& _mat);
	/*HBitmap图像转Mat图像*/
	BOOL HBitmapToMat(HBITMAP& _hBmp, Mat& _mat);
	/*切图*/
	Mat CutImg(Mat img,Rect rect);
	/*
	*统计牌
	*/
	void CountCards(vector<string> lables);
	/*
	*初始化扑克牌
	*/
	void InitCards();
	/*
	*根据三人矩形位置，分别统计出牌情况计算
	*/
	void Helper::RecognitionCards(Mat img, Rect my, Rect previous, Rect next);

	/*数组转string*/
	string vectorToString(vector<string> vec);
	/*展示牌信息*/
	void ShowCards();
	//int转string
	string int2str(const int &int_temp);
};

