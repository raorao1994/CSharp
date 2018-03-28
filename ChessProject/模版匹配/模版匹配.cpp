// ģ��ƥ��.cpp : �������̨Ӧ�ó������ڵ㡣
#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/core.hpp>

using namespace std;
using namespace cv;

int main()
{
	//1����ȡģ��
	Mat _temp = imread("img/07.png");
	Mat temp;
	cvtColor(_temp, temp, CV_BGR2GRAY);
	resize(temp, temp, Size(temp.cols *1.5, temp.rows * 1.5), 0, 0, INTER_LINEAR);
	//2����ȡ��ѯͼ��
	Mat _src = imread("2.png");
	Mat src;
	cvtColor(_src, src, CV_BGR2GRAY);
	resize(src, src, Size(src.cols * 1.5, src.rows * 1.5), 0, 0, INTER_LINEAR);
	//3��ʶ��
	double minTh = 0.02;
	Mat result;
	int width = src.cols;
	int height = src.rows;
	//3��ƥ����
	int sum = 0;
	int result_cols = src.cols - temp.cols + 1;
	int result_rows = src.rows - temp.rows + 1;
	//4��ͼ��ƥ��
	//��������ʹ�õ�ƥ���㷨�Ǳ�׼ƽ����ƥ�� method=CV_TM_SQDIFF_NORMED����ֵԽСƥ���Խ��
	matchTemplate(src, temp, result, CV_TM_SQDIFF_NORMED);
	//5����׼��һ��
	normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
	//6�������ƥ��ֵ
	//��Ŀ��ƥ��
	double minVal = -1;
	double maxVal;
	Point minLoc;
	Point maxLoc;
	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
	cout << "��Сֵ�ǣ�" << minVal << endl;
	//7�����Ƴ�ƥ������
	/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
	Scalar(0, 0, 0), 2, 8, 0);*/
	//double matchValue = result.at<float>(minLoc.y, minLoc.x);
	//8���ж��ٽ������Ƿ����ƥ���
	int count = 0;//ͬһֻ�Ʋ��ܳ����ĸ�
	for (int x = minLoc.x - 30 * 3; x<minLoc.x + 30 * 3; x++)
	{
		int y = minLoc.y;
		if (x >= result_cols || y >= result_rows || x <0 || y <0)continue;
		//4.2���resultImg��(j,x)λ�õ�ƥ��ֵmatchValue  
		float matchValue = 1;
		matchValue = result.at<float>(y, x);
		//4.3����ɸѡ����  
		//����1:����ֵ����0.9  
		if (matchValue < minTh)
		{
			//5.��ɸѡ���ĵ㻭���߿������  
			rectangle(src, Point(x, y), Point(x + temp.cols, y + temp.rows),
				Scalar(0, 255, 0), 4, 8, 0);
			sum++;
			x += 10;
		}
	}
	cout << "ʶ��" << sum << endl;
	//�ͷ��ڴ�
	temp.release();
	result.release();
	resize(src, src, Size(src.cols / 1.5, src.rows / 1.5), 0, 0, INTER_LINEAR);
	imshow("src", src);
	waitKey(0);
    return 0;
}

