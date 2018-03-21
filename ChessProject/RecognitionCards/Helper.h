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
	//模版图片列表
	vector<Mat> TempImgs;
	//模版图片对应lable列表
	vector<string> Lables;
	//扑克牌文字间隔像素
	int aroundPix = 50;
	//精确度阀值
	double minTh = 0.04;//0.025

	Helper(int Pix=50, int minTh=0.04);
	~Helper();
	LPCWSTR stringToLPCWSTR(string orig);
	HBITMAP CopyScreenToBitmap();
	BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);
	void GetAllTemp(string path);
	vector<string> Recognition(Mat img, Mat src);
};

