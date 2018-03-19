#include "stdafx.h"

///图像单目标模板匹配
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	Mat srcImg = imread("j.jpg", CV_LOAD_IMAGE_COLOR);
//	Mat tempImg = imread("template.jpg", CV_LOAD_IMAGE_COLOR);
//	cout << "Size of template: " << tempImg.size() << endl;
//	//1.构建结果图像resultImg(注意大小和类型)
//	//如果原图(待搜索图像)尺寸为W x H, 而模版尺寸为 w x h, 则结果图像尺寸一定是(W-w+1)x(H-h+1)
//	//结果图像必须为单通道32位浮点型图像
//	int width = srcImg.cols - tempImg.cols + 1;
//	int height = srcImg.rows - tempImg.rows + 1;
//	Mat resultImg(Size(width, height), CV_32FC1);
//	//2.模版匹配
//	matchTemplate(srcImg, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//	imshow("result", resultImg);
//	//3.正则化(归一化到0-1)
//	normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//	//4.找出resultImg中的最大值及其位置
//	double minValue = 0;
//	double maxValue = 0;
//	Point minPosition;
//	Point maxPosition;
//	minMaxLoc(resultImg, &minValue, &maxValue, &minPosition, &maxPosition);
//	cout << "minValue: " << minValue << endl;
//	cout << "maxValue: " << maxValue << endl;
//	cout << "minPosition: " << minPosition << endl;
//	cout << "maxPosition: " << maxPosition << endl;
//	//5.根据resultImg中的最大值位置在源图上画出矩形
//	rectangle(srcImg, maxPosition, Point(maxPosition.x + tempImg.cols, maxPosition.y + tempImg.rows), Scalar(0, 255, 0), 1, 8);
//	imshow("srcImg", srcImg);
//	imshow("template", tempImg);
//	waitKey(0);
//	return 0;
//}

///视频单目标模板匹配
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	//1.定义VideoCapture类对象video，读取视频
//	VideoCapture video("1.mp4");
//	//1.1.判断视频是否打开
//	if (!video.isOpened())
//	{
//		cout << "video open error!" << endl;
//		return 0;
//	}
//	//2.循环读取视频的每一帧，对每一帧进行模版匹配
//	while (1)
//	{
//		//2.1.读取帧
//		Mat frame;
//		video >> frame;
//		//2.2.对帧进行异常检测
//		if (frame.empty())
//		{
//			cout << "frame empty" << endl;
//			break;
//		}
//		//2.3.对帧进行模版匹配
//		Mat tempImg = imread("green.JPG", CV_LOAD_IMAGE_COLOR);
//		cout << "Size of template: " << tempImg.size() << endl;
//		//2.3.1.构建结果图像resultImg(注意大小和类型)
//		//如果原图(待搜索图像)尺寸为W x H, 而模版尺寸为 w x h, 则结果图像尺寸一定是(W-w+1)x(H-h+1)
//		//结果图像必须为单通道32位浮点型图像
//		int width = frame.cols - tempImg.cols + 1;
//		int height = frame.rows - tempImg.rows + 1;
//		Mat resultImg(Size(width, height), CV_32FC1);
//		//2.3.2.模版匹配
//		matchTemplate(frame, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//		imshow("result", resultImg);
//		//2.3.3.正则化(归一化到0-1)
//		normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//		//2.3.4.找出resultImg中的最大值及其位置
//		double minValue = 0;
//		double maxValue = 0;
//		Point minPosition;
//		Point maxPosition;
//		minMaxLoc(resultImg, &minValue, &maxValue, &minPosition, &maxPosition);
//		cout << "minValue: " << minValue << endl;
//		cout << "maxValue: " << maxValue << endl;
//		cout << "minPosition: " << minPosition << endl;
//		cout << "maxPosition: " << maxPosition << endl;
//		//2.3.5.根据resultImg中的最大值位置在源图上画出矩形
//		rectangle(frame, maxPosition, Point(maxPosition.x + tempImg.cols, maxPosition.y + tempImg.rows), Scalar(0, 255, 0), 1, 8);
//		imshow("srcImg", frame);
//		imshow("template", tempImg);
//		if (waitKey(10) == 27)
//		{
//			cout << "ESC退出" << endl;
//			break;
//		};
//	}
//	return 0;
//}

///多目标模板匹配
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	Mat srcImg = imread("k.jpg", CV_LOAD_IMAGE_COLOR);
//	Mat tempImg = imread("template.jpg", CV_LOAD_IMAGE_COLOR);
//	//1.构建结果图像resultImg(注意大小和类型)
//	//如果原图(待搜索图像)尺寸为W x H, 而模版尺寸为 w x h, 则结果图像尺寸一定是(W-w+1)x(H-h+1)
//	//结果图像必须为单通道32位浮点型图像
//	int width = srcImg.cols - tempImg.cols + 1;
//	int height = srcImg.rows - tempImg.rows + 1;
//	Mat resultImg(Size(width, height), CV_32FC1);
//	//2.模版匹配
//	matchTemplate(srcImg, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//	imshow("result", resultImg);
//	//3.正则化(归一化到0-1)
//	normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//	//4.遍历resultImg，给定筛选条件，筛选出前几个匹配位置
//	int tempX = 0;
//	int tempY = 0;
//	char prob[10] = { 0 };
//	//4.1遍历resultImg
//	for (int i = 0; i<resultImg.rows; i++)
//	{
//		for (int j = 0; j<resultImg.cols; j++)
//		{
//			//4.2获得resultImg中(j,x)位置的匹配值matchValue
//			double matchValue = resultImg.at<float>(i, j);
//			sprintf(prob, "%.2f", matchValue);
//			//4.3给定筛选条件
//			//条件1:概率值大于0.9
//			//条件2:任何选中的点在x方向和y方向上都要比上一个点大5(避免画边框重影的情况)
//			if (matchValue > 0.9&& abs(i - tempY)>5 && abs(j - tempX)>5)
//			{
//				//5.给筛选出的点画出边框和文字
//				rectangle(srcImg, Point(j, i), Point(j + tempImg.cols, i + tempImg.rows), Scalar(0, 255, 0), 1, 8);
//				putText(srcImg, prob, Point(j, i + 100), CV_FONT_BLACK, 1, Scalar(0, 0, 255), 1);
//				tempX = j;
//				tempY = i;
//			}
//		}
//	}
//	imshow("srcImg", srcImg);
//	imshow("template", tempImg);
//	waitKey(0);
//	return 0;
//}