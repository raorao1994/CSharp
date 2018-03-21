// RecognitionCards.cpp : �������̨Ӧ�ó������ڵ㡣
//ʶ���˿���
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
	//0����ȡ����ģ��
	GetAllTemp();
	//1����ȡͼƬ�ļ�
	QueryPerformanceCounter(&start_t);
	Mat src = imread(imgPath);
	Mat gray;
	cvtColor(src, gray, CV_BGR2GRAY);
	vector<string> str=Recognition(gray, src);
	//������������ʱ
	QueryPerformanceCounter(&stop_t);
	exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
	cout << "��ʱ" << exe_time << "����"<<endl;
	imshow("ԭͼ", src);
	waitKey(0);
    return 0;
}
//��ȡ����ģ��
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
//ʶ������ģ��
vector<string> Recognition(Mat img,Mat src)
{
	vector<string> resultLable;
	for (size_t i = 0; i < TempImgs.size(); i++)
	{
		Mat result;
		//2����ȡģ��ͼƬ
		Mat temp = TempImgs[i];
		//3��ƥ����
		int result_cols = img.cols - temp.cols + 1;
		int result_rows = img.rows - temp.rows + 1;
		//4��ͼ��ƥ��
		//��������ʹ�õ�ƥ���㷨�Ǳ�׼ƽ����ƥ�� method=CV_TM_SQDIFF_NORMED����ֵԽСƥ���Խ��
		matchTemplate(img, temp, result, CV_TM_SQDIFF_NORMED);
		//5����׼��һ��
		//normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
		//6�������ƥ��ֵ
		//��Ŀ��ƥ��
		double minVal = -1;
		double maxVal;
		Point minLoc;
		Point maxLoc;
		minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
		if (minVal > minTh)continue;
		//7�����Ƴ�ƥ������
		/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
			Scalar(0, 0, 0), 2, 8, 0);*/
		double matchValue = result.at<float>(minLoc.y, minLoc.x);
		//8���ж��ٽ������Ƿ����ƥ���
		for (int x = minLoc.x- aroundPix; x<minLoc.x+ aroundPix; x++)
		{
			//4.2���resultImg��(j,x)λ�õ�ƥ��ֵmatchValue  
			double matchValue = result.at<float>(minLoc.y, x);
			//4.3����ɸѡ����  
			//����1:����ֵ����0.9  
			if (matchValue < minTh)
			{
				//cout << "ƥ��ȣ�" << matchValue << endl;
				//5.��ɸѡ���ĵ㻭���߿������  
				rectangle(src, Point(x, minLoc.y), Point(x+ temp.cols, minLoc.y + temp.rows),
					Scalar(0, 255, 0), 2, 8, 0);
				x += 10;
				int index=Lables[0].find('.');
				string a = Lables[0].substr(0, index);
				resultLable.push_back(a);
			}
		}
		//��Ŀ��ƥ��
		//int tempX = 0;
		//int tempY = 0;
		//char prob[10] = { 0 };
		////4.1����resultImg  
		//for (int i = 0; i<result.rows; i++)
		//{
		//	for (int j = 0; j<result.cols; j++)
		//	{
		//		//4.2���resultImg��(j,x)λ�õ�ƥ��ֵmatchValue  
		//		double matchValue = result.at<float>(i, j);
		//		sprintf(prob, "%.2f", matchValue);
		//		//4.3����ɸѡ����  
		//		//����1:����ֵ����0.9  
		//		//����2:�κ�ѡ�еĵ���x�����y�����϶�Ҫ����һ�����5(���⻭�߿���Ӱ�����)  
		//		if (matchValue < 0.09&& abs(i - tempY)>5 && abs(j - tempX)>5)
		//		{
		//			//cout << "ƥ��ȣ�" << matchValue << endl;
		//			//5.��ɸѡ���ĵ㻭���߿������  
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