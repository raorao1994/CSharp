// ConsoleApplication1.cpp : �������̨Ӧ�ó������ڵ㡣
//��һ��

#include "stdafx.h"
#include <opencv2\opencv.hpp>

int first()
{
	const char *pstrImageName = "D://8.jpg";
	const char *pstrWindowsTitle = "OpenCV��һ������";

	//���ļ��ж�ȡͼ��  
	IplImage *pImage = cvLoadImage(pstrImageName, CV_LOAD_IMAGE_UNCHANGED);

	//��������  
	cvNamedWindow(pstrWindowsTitle, CV_WINDOW_AUTOSIZE);

	//��ָ����������ʾͼ��  
	cvShowImage(pstrWindowsTitle, pImage);

	//�ȴ������¼�  
	cvWaitKey();
	//����ƶ�����
	cvDestroyWindow(pstrWindowsTitle);
	//���pImage�����ڴ�
	cvReleaseImage(&pImage);
	return 0;
}


//��һ�Σ�����opencv����ͼ��
int main()
{
	first();
	//cv::Mat�൱��cv.Mat��˼��cv�����ռ��µ�Mat��
	//������ͷ������using namespace cv;ʡȥ���cv�����ռ�
	cv::Mat img = cv::imread("D://8.jpg");//�滻�����ͼƬ·��

	cv::imshow("test", img);

	cv::waitKey();

	//frame= cv::cvarrToMat(pImage); imgתmat

	//1. cv::Mat->IplImage
	//	cv::Mat matimg = cv::imread("heels.jpg");
	//IplImage* iplimg;
	//*iplimg = IplImage(matimg);
	//2. IplImage->cv::Mat
	//	IplImage* iplimg = cvLoadImage("heels.jpg");
	//cv::Mat matimg;
	//matimg = cv::Mat(iplimg);

	//ͼƬ��ȡ
	//�������Ϊ 320,�߶�Ϊ 240 �� 3 ͨ��ͼ��
	//Mat img(Size(320, 240), CV_8UC3);
	//roi �Ǳ�ʾ img �� Rect(10,10,100,100)����Ķ���
	//Mat roi(img, Rect(10, 10, 100, 100));
	return 0;
}

