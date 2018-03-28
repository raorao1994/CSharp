// 模版匹配.cpp : 定义控制台应用程序的入口点。
#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/core.hpp>

using namespace std;
using namespace cv;

int main()
{
	//1、读取模版
	Mat _temp = imread("img/07.png");
	Mat temp;
	cvtColor(_temp, temp, CV_BGR2GRAY);
	resize(temp, temp, Size(temp.cols *1.5, temp.rows * 1.5), 0, 0, INTER_LINEAR);
	//2、读取查询图像
	Mat _src = imread("2.png");
	Mat src;
	cvtColor(_src, src, CV_BGR2GRAY);
	resize(src, src, Size(src.cols * 1.5, src.rows * 1.5), 0, 0, INTER_LINEAR);
	//3、识别
	double minTh = 0.02;
	Mat result;
	int width = src.cols;
	int height = src.rows;
	//3、匹配结果
	int sum = 0;
	int result_cols = src.cols - temp.cols + 1;
	int result_rows = src.rows - temp.rows + 1;
	//4、图像匹配
	//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
	matchTemplate(src, temp, result, CV_TM_SQDIFF_NORMED);
	//5、标准归一化
	normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
	//6、计算出匹配值
	//单目标匹配
	double minVal = -1;
	double maxVal;
	Point minLoc;
	Point maxLoc;
	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
	cout << "最小值是：" << minVal << endl;
	//7、绘制出匹配区域
	/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
	Scalar(0, 0, 0), 2, 8, 0);*/
	//double matchValue = result.at<float>(minLoc.y, minLoc.x);
	//8、判断临近坐标是否存在匹配点
	int count = 0;//同一只牌不能超过四个
	for (int x = minLoc.x - 30 * 3; x<minLoc.x + 30 * 3; x++)
	{
		int y = minLoc.y;
		if (x >= result_cols || y >= result_rows || x <0 || y <0)continue;
		//4.2获得resultImg中(j,x)位置的匹配值matchValue  
		float matchValue = 1;
		matchValue = result.at<float>(y, x);
		//4.3给定筛选条件  
		//条件1:概率值大于0.9  
		if (matchValue < minTh)
		{
			//5.给筛选出的点画出边框和文字  
			rectangle(src, Point(x, y), Point(x + temp.cols, y + temp.rows),
				Scalar(0, 255, 0), 4, 8, 0);
			sum++;
			x += 10;
		}
	}
	cout << "识别到" << sum << endl;
	//释放内存
	temp.release();
	result.release();
	resize(src, src, Size(src.cols / 1.5, src.rows / 1.5), 0, 0, INTER_LINEAR);
	imshow("src", src);
	waitKey(0);
    return 0;
}

