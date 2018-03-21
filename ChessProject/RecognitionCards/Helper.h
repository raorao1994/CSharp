#pragma once
#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>

using namespace std;
using namespace cv;

class Helper
{
public:
	Helper();
	~Helper();
	LPCWSTR stringToLPCWSTR(string orig);
	HBITMAP CopyScreenToBitmap();
	BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);
};

