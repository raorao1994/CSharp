#pragma once
#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>
#include <io.h>
#include <map>
#include <fstream>

using namespace std;
using namespace cv;

class Helper
{
public:
	//�����ģ��ͼƬ�б�
	vector<Mat> TempImgs;
	//�����ģ��ͼƬ��Ӧlable�б�
	vector<string> Lables;
	//�˿������ּ������
	int aroundPix = 30;
	//��ȷ�ȷ�ֵ
	double minTh = 0.04;//0.025
	//�����˿���
	map<string, int> Cards;
	//���ʶ�������
	string outputStr = "";
	int PlayCardsCount = 0;//������Ƶ�����
	//�����ʶ�����
	Mat myImg, previousImg, nextImg;
	string myStr="", previousStr = "", nextStr = "";
	int myCount=0, previousCount = 0, nextCount = 0,lableCount=3;//lableCount�ж��ظ�����


	Helper(int Pix=50, double minTh=0.04);
	~Helper();
	/*�ַ���תLPCWSTR*/
	LPCWSTR stringToLPCWSTR(string orig);
	/*��ȡ��Ļͼ��*/
	HBITMAP CopyScreenToBitmap();
	/*����Bit��Ļͼ��*/
	BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);
	/*��ȡ����ģ���ļ�*/
	void GetAllTemp(string path);
	vector<Mat> GetAllMyTemp(string path);
	/*ʶ��ͼ���еĴ������*/
	vector<string> Recognition(Mat img, Mat src);
	vector<string> Recognitions(Mat gary, vector<Mat> tempImgs, double minThs);
	/*ʶ��ģ���ļ��Ƿ����*/
	bool HaveTemp(Mat img, string tempPath, double minThs);
	/*Matͼ��תHBitmapͼ��*/
	BOOL MatToHBitmap(HBITMAP& _hBmp, Mat& _mat);
	/*HBitmapͼ��תMatͼ��*/
	BOOL HBitmapToMat(HBITMAP& _hBmp, Mat& _mat);
	/*��ͼ*/
	Mat CutImg(Mat img,Rect rect);
	/*
	*ͳ����
	*/
	void CountCards(vector<string> lables);
	/*
	*��ʼ���˿���
	*/
	void InitCards();
	/*
	*�������˾���λ�ã��ֱ�ͳ�Ƴ����������
	*/
	void Helper::RecognitionCards(Mat img, Rect my, Rect previous, Rect next);

	/*����תstring*/
	string vectorToString(vector<string> vec);
	/*չʾ����Ϣ*/
	void ShowCards();
	//intתstring
	string int2str(const int &int_temp);
};

