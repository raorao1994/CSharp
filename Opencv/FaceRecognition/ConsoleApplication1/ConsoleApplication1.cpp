// ConsoleApplication1.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include <opencv2\opencv.hpp>

//��һ�Σ�����opencv����ͼ��
int main()
{
	cv::Mat img = cv::imread("D://8.jpg");//�滻�����ͼƬ·��

	cv::imshow("test", img);

	cv::waitKey();

	return 0;
}

