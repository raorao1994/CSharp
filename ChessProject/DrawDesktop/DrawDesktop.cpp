// DrawDesktop.cpp : 定义控制台应用程序的入口点。
//电脑屏幕画字

#include "stdafx.h"
#include <iostream>
#include<windows.h>
#include <io.h>
#include <map>
#include <fstream>
#include <atlimage.h>

using namespace std;

//wstring转换成string  
string WChar2Ansi(LPCWSTR pwszSrc)
{
	int nLen = WideCharToMultiByte(CP_ACP, 0, pwszSrc, -1, NULL, 0, NULL, NULL);

	if (nLen <= 0) return std::string("");

	char* pszDst = new char[nLen];
	if (NULL == pszDst) return std::string("");

	WideCharToMultiByte(CP_ACP, 0, pwszSrc, -1, pszDst, nLen, NULL, NULL);
	pszDst[nLen - 1] = 0;

	std::string strTemp(pszDst);
	delete[] pszDst;

	return strTemp;
}
//string转换成  wstring
wstring s2ws(string s)
{
	int len;
	int slength = (int)s.length() + 1;
	len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0);
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
	std::wstring r(buf);
	delete[] buf;
	return r.c_str();
}

//string转换成  LPCWSTR
LPCWSTR stringToLPCWSTR(string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	wstring wstrResult(wcstring);
	//free(wcstring);
	return wstrResult.c_str();
}
//透明化
void TransparentPNG(CImage *png)
{
	for (int i = 0; i <png->GetWidth(); i++)
	{
		for (int j = 0; j <png->GetHeight(); j++)
		{
			unsigned char* pucColor = reinterpret_cast<unsigned char*>(png->GetPixelAddress(i, j));
			pucColor[0] = pucColor[0] * pucColor[3] / 255;
			pucColor[1] = pucColor[1] * pucColor[3] / 255;
			pucColor[2] = pucColor[2] * pucColor[3] / 255;
		}
	}
}


void  draw()
{
	// 获取一个可供画图的DC，我这里就直接用桌面算了
	HDC hdc = GetWindowDC(GetDesktopWindow());

	// 创建红色1像素宽度的实线画笔
	HPEN hpen1 = CreatePen(PS_SOLID, 1, RGB(255, 0, 0));
	// 创建绿色5像素宽度的破折画笔，如果你想创建其他种类的画笔请参阅MSDN
	HPEN hpen2 = CreatePen(PS_DASH, 5, RGB(0, 255, 0));
	// 创建一个实体蓝色画刷
	HBRUSH hbrush1 = CreateSolidBrush(RGB(0, 0, 255));
	// 创造一个透明的画刷，如果你想创建其他种类的画刷请参阅MSDN
	HBRUSH hbrush2 = (HBRUSH)GetStockObject(NULL_BRUSH);

	// 将hpen1和hbrush1选进HDC，并保存HDC原来的画笔和画刷
	HPEN hpen_old = (HPEN)SelectObject(hdc, hpen1);
	HBRUSH hbrush_old = (HBRUSH)SelectObject(hdc, hbrush1);

	// 在(40,30)处画一个宽200像素，高50像素的矩形
	Rectangle(hdc, 40, 30, 40 + 200, 30 + 50);

	// 换hpen1和hbrush1，然后在(40,100)处也画一个矩形，看看有何差别
	SelectObject(hdc, hpen2);
	SelectObject(hdc, hbrush2);
	Rectangle(hdc, 40, 100, 40 + 200, 100 + 50);

	// 画个椭圆看看
	Ellipse(hdc, 40, 200, 40 + 200, 200 + 50);

	// 画个(0,600)到(800,0)的直线看看
	MoveToEx(hdc, 0, 600, NULL);
	LineTo(hdc, 800, 0);

	// 在(700,500)处画个黄点，不过这个点只有一像素大小，你细细的看才能找到
	SetPixel(hdc, 700, 500, RGB(255, 255, 0));

	// 恢复原来的画笔和画刷
	SelectObject(hdc, hpen_old);
	SelectObject(hdc, hbrush_old);
}

void DrawText1()
{
	// 获取一个可供画图的DC，我这里就直接用桌面算了
	HDC hdc = GetWindowDC(GetDesktopWindow());
	// 创建红色1像素宽度的实线画笔
	HPEN hpen1 = CreatePen(PS_SOLID, 1, RGB(255, 0, 0));
	// 将hpen1选进HDC，并保存HDC原来的画笔和画刷
	HPEN hpen_old = (HPEN)SelectObject(hdc, hpen1);
	//文字
	//画布、距屏幕左上角xy，显示的文字，文字宽度
	TextOutA(hdc, 700, 500, "陈兴旺", 6);
	// 恢复原来的画笔和画刷
	SelectObject(hdc, hpen_old);
}

int main()
{
	int nWidth, nHeight;
	//屏幕和内存设备描述表
	HDC hScrDC, hMemDC;
	//位图句柄
	HBITMAP hBitmap , hOldBitmap;
	//屏幕分辨率
	int xScrn, yScrn;
	//为屏幕创建设备描述表
	hScrDC = GetDC(NULL);
	//为屏幕设备描述表创建兼容的内存设备描述表
	hMemDC = CreateCompatibleDC(hScrDC);
	//获得屏幕分辨率
	xScrn = GetDeviceCaps(hScrDC, HORZRES);
	yScrn = GetDeviceCaps(hScrDC, VERTRES);

	//存储屏幕的宽度
	nWidth = xScrn;
	nHeight = yScrn;

	//创建一个与屏幕设备描述表兼容的 位图
	hBitmap = CreateCompatibleBitmap(hScrDC, xScrn, yScrn);
	//把新位图选到内存设备描述表中
	hOldBitmap = (HBITMAP)SelectObject(hMemDC, hBitmap);
	//把屏幕设备描述表拷贝到内存设备描述表中
	BitBlt(hMemDC, 0, 0, xScrn, yScrn, hScrDC, 0, 0, SRCCOPY);
	//得到屏幕位图句柄
	hBitmap = (HBITMAP)SelectObject(hMemDC, hOldBitmap);
	//写入文字
	//CFont font;
	//font.CreatePointFont(120, "微软雅黑", &hMemDC);
	//SelectObject(hMemDC, font)
	//CWindowDC dc(GetDesktopWindow());
	//dc.TextOut(0,0,"aaaa",strlen("aaaa"));

	//清除
	DeleteDC(hScrDC);
	DeleteDC(hMemDC);
	DeleteObject(hMemDC);
	DeleteObject(hScrDC);
	DeleteObject(hOldBitmap);

	while (true)
	{
		DrawText1();
		Sleep(100);
	}
	

	system("prase");
}

