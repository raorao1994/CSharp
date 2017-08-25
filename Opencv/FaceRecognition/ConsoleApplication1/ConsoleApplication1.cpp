// ConsoleApplication1.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <opencv2\opencv.hpp>

//第一课：配置opencv。打开图像
int main()
{
	cv::Mat img = cv::imread("D://8.jpg");//替换成你的图片路径

	cv::imshow("test", img);

	cv::waitKey();

	return 0;
}

