// ConsoleApplication1.cpp : 定义控制台应用程序的入口点。
//第一课

#include "stdafx.h"
#include <opencv2\opencv.hpp>

int first()
{
	const char *pstrImageName = "D://8.jpg";
	const char *pstrWindowsTitle = "OpenCV第一个程序";

	//从文件中读取图像  
	IplImage *pImage = cvLoadImage(pstrImageName, CV_LOAD_IMAGE_UNCHANGED);

	//创建窗口  
	cvNamedWindow(pstrWindowsTitle, CV_WINDOW_AUTOSIZE);

	//在指定窗口中显示图像  
	cvShowImage(pstrWindowsTitle, pImage);

	//等待按键事件  
	cvWaitKey();
	//清除制定窗口
	cvDestroyWindow(pstrWindowsTitle);
	//清空pImage对象内存
	cvReleaseImage(&pImage);
	return 0;
}


//第一课：配置opencv。打开图像
int main()
{
	first();
	//cv::Mat相当于cv.Mat意思是cv命名空间下的Mat类
	//可以在头部加入using namespace cv;省去添加cv命名空间
	cv::Mat img = cv::imread("D://8.jpg");//替换成你的图片路径

	cv::imshow("test", img);

	cv::waitKey();

	//frame= cv::cvarrToMat(pImage); img转mat

	//1. cv::Mat->IplImage
	//	cv::Mat matimg = cv::imread("heels.jpg");
	//IplImage* iplimg;
	//*iplimg = IplImage(matimg);
	//2. IplImage->cv::Mat
	//	IplImage* iplimg = cvLoadImage("heels.jpg");
	//cv::Mat matimg;
	//matimg = cv::Mat(iplimg);

	//图片提取
	//创建宽度为 320,高度为 240 的 3 通道图像
	//Mat img(Size(320, 240), CV_8UC3);
	//roi 是表示 img 中 Rect(10,10,100,100)区域的对象
	//Mat roi(img, Rect(10, 10, 100, 100));
	return 0;
}

