#include "stdafx.h"

///ͼ��Ŀ��ģ��ƥ��
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	Mat srcImg = imread("j.jpg", CV_LOAD_IMAGE_COLOR);
//	Mat tempImg = imread("template.jpg", CV_LOAD_IMAGE_COLOR);
//	cout << "Size of template: " << tempImg.size() << endl;
//	//1.�������ͼ��resultImg(ע���С������)
//	//���ԭͼ(������ͼ��)�ߴ�ΪW x H, ��ģ��ߴ�Ϊ w x h, ����ͼ��ߴ�һ����(W-w+1)x(H-h+1)
//	//���ͼ�����Ϊ��ͨ��32λ������ͼ��
//	int width = srcImg.cols - tempImg.cols + 1;
//	int height = srcImg.rows - tempImg.rows + 1;
//	Mat resultImg(Size(width, height), CV_32FC1);
//	//2.ģ��ƥ��
//	matchTemplate(srcImg, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//	imshow("result", resultImg);
//	//3.����(��һ����0-1)
//	normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//	//4.�ҳ�resultImg�е����ֵ����λ��
//	double minValue = 0;
//	double maxValue = 0;
//	Point minPosition;
//	Point maxPosition;
//	minMaxLoc(resultImg, &minValue, &maxValue, &minPosition, &maxPosition);
//	cout << "minValue: " << minValue << endl;
//	cout << "maxValue: " << maxValue << endl;
//	cout << "minPosition: " << minPosition << endl;
//	cout << "maxPosition: " << maxPosition << endl;
//	//5.����resultImg�е����ֵλ����Դͼ�ϻ�������
//	rectangle(srcImg, maxPosition, Point(maxPosition.x + tempImg.cols, maxPosition.y + tempImg.rows), Scalar(0, 255, 0), 1, 8);
//	imshow("srcImg", srcImg);
//	imshow("template", tempImg);
//	waitKey(0);
//	return 0;
//}

///��Ƶ��Ŀ��ģ��ƥ��
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	//1.����VideoCapture�����video����ȡ��Ƶ
//	VideoCapture video("1.mp4");
//	//1.1.�ж���Ƶ�Ƿ��
//	if (!video.isOpened())
//	{
//		cout << "video open error!" << endl;
//		return 0;
//	}
//	//2.ѭ����ȡ��Ƶ��ÿһ֡����ÿһ֡����ģ��ƥ��
//	while (1)
//	{
//		//2.1.��ȡ֡
//		Mat frame;
//		video >> frame;
//		//2.2.��֡�����쳣���
//		if (frame.empty())
//		{
//			cout << "frame empty" << endl;
//			break;
//		}
//		//2.3.��֡����ģ��ƥ��
//		Mat tempImg = imread("green.JPG", CV_LOAD_IMAGE_COLOR);
//		cout << "Size of template: " << tempImg.size() << endl;
//		//2.3.1.�������ͼ��resultImg(ע���С������)
//		//���ԭͼ(������ͼ��)�ߴ�ΪW x H, ��ģ��ߴ�Ϊ w x h, ����ͼ��ߴ�һ����(W-w+1)x(H-h+1)
//		//���ͼ�����Ϊ��ͨ��32λ������ͼ��
//		int width = frame.cols - tempImg.cols + 1;
//		int height = frame.rows - tempImg.rows + 1;
//		Mat resultImg(Size(width, height), CV_32FC1);
//		//2.3.2.ģ��ƥ��
//		matchTemplate(frame, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//		imshow("result", resultImg);
//		//2.3.3.����(��һ����0-1)
//		normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//		//2.3.4.�ҳ�resultImg�е����ֵ����λ��
//		double minValue = 0;
//		double maxValue = 0;
//		Point minPosition;
//		Point maxPosition;
//		minMaxLoc(resultImg, &minValue, &maxValue, &minPosition, &maxPosition);
//		cout << "minValue: " << minValue << endl;
//		cout << "maxValue: " << maxValue << endl;
//		cout << "minPosition: " << minPosition << endl;
//		cout << "maxPosition: " << maxPosition << endl;
//		//2.3.5.����resultImg�е����ֵλ����Դͼ�ϻ�������
//		rectangle(frame, maxPosition, Point(maxPosition.x + tempImg.cols, maxPosition.y + tempImg.rows), Scalar(0, 255, 0), 1, 8);
//		imshow("srcImg", frame);
//		imshow("template", tempImg);
//		if (waitKey(10) == 27)
//		{
//			cout << "ESC�˳�" << endl;
//			break;
//		};
//	}
//	return 0;
//}

///��Ŀ��ģ��ƥ��
//#include "opencv2/opencv.hpp"
//using namespace cv;
//#include <iostream>
//using namespace std;
//int main()
//{
//	Mat srcImg = imread("k.jpg", CV_LOAD_IMAGE_COLOR);
//	Mat tempImg = imread("template.jpg", CV_LOAD_IMAGE_COLOR);
//	//1.�������ͼ��resultImg(ע���С������)
//	//���ԭͼ(������ͼ��)�ߴ�ΪW x H, ��ģ��ߴ�Ϊ w x h, ����ͼ��ߴ�һ����(W-w+1)x(H-h+1)
//	//���ͼ�����Ϊ��ͨ��32λ������ͼ��
//	int width = srcImg.cols - tempImg.cols + 1;
//	int height = srcImg.rows - tempImg.rows + 1;
//	Mat resultImg(Size(width, height), CV_32FC1);
//	//2.ģ��ƥ��
//	matchTemplate(srcImg, tempImg, resultImg, CV_TM_CCOEFF_NORMED);
//	imshow("result", resultImg);
//	//3.����(��һ����0-1)
//	normalize(resultImg, resultImg, 0, 1, NORM_MINMAX, -1);
//	//4.����resultImg������ɸѡ������ɸѡ��ǰ����ƥ��λ��
//	int tempX = 0;
//	int tempY = 0;
//	char prob[10] = { 0 };
//	//4.1����resultImg
//	for (int i = 0; i<resultImg.rows; i++)
//	{
//		for (int j = 0; j<resultImg.cols; j++)
//		{
//			//4.2���resultImg��(j,x)λ�õ�ƥ��ֵmatchValue
//			double matchValue = resultImg.at<float>(i, j);
//			sprintf(prob, "%.2f", matchValue);
//			//4.3����ɸѡ����
//			//����1:����ֵ����0.9
//			//����2:�κ�ѡ�еĵ���x�����y�����϶�Ҫ����һ�����5(���⻭�߿���Ӱ�����)
//			if (matchValue > 0.9&& abs(i - tempY)>5 && abs(j - tempX)>5)
//			{
//				//5.��ɸѡ���ĵ㻭���߿������
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