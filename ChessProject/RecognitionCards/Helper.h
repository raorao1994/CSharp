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
	//ģ��ͼƬ�б�
	vector<Mat> TempImgs;
	//ģ��ͼƬ��Ӧlable�б�
	vector<string> Lables;
	//�˿������ּ������
	int aroundPix = 30;
	//��ȷ�ȷ�ֵ
	double minTh = 0.04;//0.025

	Helper(int Pix=50, double minTh=0.04);
	~Helper();
	LPCWSTR stringToLPCWSTR(string orig);
	HBITMAP CopyScreenToBitmap();
	BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);
	void GetAllTemp(string path);
	vector<string> Recognition(Mat img, Mat src);
	Point GetTempPoint(string imgPath, string tempPath);
	BOOL MatToHBitmap(HBITMAP& _hBmp, Mat& _mat);
	BOOL HBitmapToMat(HBITMAP& _hBmp, Mat& _mat);
};
