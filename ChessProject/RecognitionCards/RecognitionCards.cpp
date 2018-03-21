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

using namespace std;
using namespace cv;

string imgPath = "../img/4.jpg";
//string imgTempPath = "../img/template";
string imgTempPath = "E:/SVN/CShap/trunk/ChessProject/img/template";
double minTh = 0.04;//0.025
int aroundPix = 50;
vector<Mat> TempImgs;
vector<string> Lables;
Mat src;

void GetAllTemp();
vector<string> Recognition(Mat img, Mat src);

int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);
	//0、获取所有模版
	GetAllTemp();
	//1、读取图片文件
	QueryPerformanceCounter(&start_t);
	Mat src = imread(imgPath);
	Mat gray;
	cvtColor(src, gray, CV_BGR2GRAY);
	vector<string> str=Recognition(gray, src);
	//结束，计算用时
	QueryPerformanceCounter(&stop_t);
	exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
	cout << "耗时" << exe_time << "毫秒"<<endl;
	imshow("原图", src);
	waitKey(0);
    return 0;
}
//读取所有模版
void GetAllTemp()
{
	intptr_t hFile = 0;
	struct  _finddata_t  fileinfo;
	string p;
	long i;
	if ((hFile = _findfirst(p.assign(imgTempPath).append("\\*").c_str(), &fileinfo)) != -1)
	{
		do
		{
			if (!(fileinfo.attrib& _A_SUBDIR))
			{
				string tmp_path = p.assign(imgTempPath).append("\\").append(fileinfo.name);
				Mat temp = imread(tmp_path, CV_LOAD_IMAGE_GRAYSCALE);
				TempImgs.push_back(temp);
				Lables.push_back(fileinfo.name);
			}
		} while (_findnext(hFile, &fileinfo) == 0);
	}
}
//识别所有模版
vector<string> Recognition(Mat img,Mat src)
{
	vector<string> resultLable;
	for (size_t i = 0; i < TempImgs.size(); i++)
	{
		Mat result;
		//2、读取模版图片
		Mat temp = TempImgs[i];
		//3、匹配结果
		int result_cols = img.cols - temp.cols + 1;
		int result_rows = img.rows - temp.rows + 1;
		//4、图像匹配
		//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
		matchTemplate(img, temp, result, CV_TM_SQDIFF_NORMED);
		//5、标准归一化
		//normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
		//6、计算出匹配值
		//单目标匹配
		double minVal = -1;
		double maxVal;
		Point minLoc;
		Point maxLoc;
		minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
		if (minVal > minTh)continue;
		//7、绘制出匹配区域
		/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
			Scalar(0, 0, 0), 2, 8, 0);*/
		double matchValue = result.at<float>(minLoc.y, minLoc.x);
		//8、判断临近坐标是否存在匹配点
		for (int x = minLoc.x- aroundPix; x<minLoc.x+ aroundPix; x++)
		{
			//4.2获得resultImg中(j,x)位置的匹配值matchValue  
			double matchValue = result.at<float>(minLoc.y, x);
			//4.3给定筛选条件  
			//条件1:概率值大于0.9  
			if (matchValue < minTh)
			{
				//cout << "匹配度：" << matchValue << endl;
				//5.给筛选出的点画出边框和文字  
				rectangle(src, Point(x, minLoc.y), Point(x+ temp.cols, minLoc.y + temp.rows),
					Scalar(0, 255, 0), 2, 8, 0);
				x += 10;
				int index=Lables[0].find('.');
				string a = Lables[0].substr(0, index);
				resultLable.push_back(a);
			}
		}
		//多目标匹配
		//int tempX = 0;
		//int tempY = 0;
		//char prob[10] = { 0 };
		////4.1遍历resultImg  
		//for (int i = 0; i<result.rows; i++)
		//{
		//	for (int j = 0; j<result.cols; j++)
		//	{
		//		//4.2获得resultImg中(j,x)位置的匹配值matchValue  
		//		double matchValue = result.at<float>(i, j);
		//		sprintf(prob, "%.2f", matchValue);
		//		//4.3给定筛选条件  
		//		//条件1:概率值大于0.9  
		//		//条件2:任何选中的点在x方向和y方向上都要比上一个点大5(避免画边框重影的情况)  
		//		if (matchValue < 0.09&& abs(i - tempY)>5 && abs(j - tempX)>5)
		//		{
		//			//cout << "匹配度：" << matchValue << endl;
		//			//5.给筛选出的点画出边框和文字  
		//			rectangle(img, Point(j, i), Point(j + temp.cols, i + temp.rows),
		//				Scalar(0, 255, 0), 2, 8, 0);
		//			tempX = j;
		//			tempY = i;
		//		}
		//	}
		//}
	}
	return resultLable;
}