// ScreenShot.cpp : 定义控制台应用程序的入口点。
//截取屏幕

#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>

using namespace std;
using namespace cv;

LPCWSTR stringToLPCWSTR(std::string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);

	return wcstring;
}

int screenshot()
{
	HDC hDC, hMemDC;
	int x, y;
	HBITMAP hbmp, holdbmp;
	DWORD err;

	BOOL b;

	//获取屏幕DC
	hDC = GetDC(NULL);
	err = GetLastError();
	//创建一个内存DC
	hMemDC = CreateCompatibleDC(hDC);
	err = GetLastError();

	//获取屏幕大小
	x = GetDeviceCaps(hDC, HORZRES);
	y = GetDeviceCaps(hDC, VERTRES);
	//创建一个和屏幕DC相同的位图
	hbmp = ::CreateCompatibleBitmap(hDC, x, y);
	err = ::GetLastError();
	//把位图选到内存DC中
	holdbmp = (HBITMAP)::SelectObject(hMemDC, hbmp);
	//把屏幕DC拷贝到内存DC中
	b = BitBlt(hMemDC, 0, 0, x, y, hDC, x, y, SRCCOPY);
	//得到位图句柄
	hbmp = (HBITMAP)::SelectObject(hMemDC, holdbmp);
	//计算每个像数所占字节数
	int ibits;
	WORD bitcount;
	ibits = GetDeviceCaps(hDC, BITSPIXEL)*GetDeviceCaps(hDC, PLANES);
	if (ibits <= 1)
		bitcount = 1;
	else if (ibits <= 4)
		bitcount = 4;
	else if (ibits <= 8)
		bitcount = 8;
	else if (ibits <= 16)
		bitcount = 16;
	else if (ibits <= 24)
		bitcount = 24;
	else
		bitcount = 32;
	DWORD dwtsbsize = 0, dwwritten;//调色板
								   //计算调色板大小
	if (bitcount <= 8)
		dwtsbsize = (1 << bitcount) * sizeof(RGBQUAD);

	BITMAP bmp; //位图属性结构
	BITMAPFILEHEADER bmfhdr; //位图文件头结构
	BITMAPINFOHEADER bi; //位图信息头结构
	LPBITMAPINFOHEADER lpbi; //指向位图信息头结构
	GetObject(hbmp, sizeof(bmp), (LPVOID)&bmp);
	bi.biSize = sizeof(BITMAPINFOHEADER);
	bi.biWidth = bmp.bmWidth;
	bi.biHeight = bmp.bmHeight;
	bi.biPlanes = 1;
	bi.biBitCount = bitcount;
	bi.biCompression = BI_RGB;
	bi.biSizeImage = 0;
	bi.biXPelsPerMeter = 0;
	bi.biYPelsPerMeter = 0;
	bi.biClrUsed = 0;
	bi.biClrImportant = 0;

	//位图大小
	DWORD dwbitsize;
	dwbitsize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
	//为位图分配内存
	HANDLE fh, hdib, hpal, holdpal = NULL;
	hdib = VirtualAlloc(NULL, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_COMMIT, PAGE_READWRITE);
	lpbi = (LPBITMAPINFOHEADER)hdib;
	*lpbi = bi;
	// 处理调色板 
	hpal = GetStockObject(DEFAULT_PALETTE);
	if (hpal)
	{
		holdpal = SelectPalette(hDC, (HPALETTE)hpal, false);
		RealizePalette(hDC);
	}
	// 获取该调色板下新的像素值
	GetDIBits(hDC, hbmp, 0, (UINT)bmp.bmHeight, (LPSTR)lpbi +
		sizeof(BITMAPINFOHEADER) + dwtsbsize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
	//恢复调色板 
	if (holdpal)
	{
		SelectPalette(hDC, (HPALETTE)holdpal, true);
		RealizePalette(hDC);
	}

	//创建位图文件 
	string p = "D:/1.bmp";//C:/1.bmp
	fh = CreateFile(stringToLPCWSTR(p), GENERIC_WRITE, 0, NULL,
		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
	if (fh == INVALID_HANDLE_VALUE)
		return 0;

	// 设置位图文件头
	bmfhdr.bfType = 0x4d42; //"bm"
	//位图大小
	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
		dwtsbsize + dwbitsize;
	bmfhdr.bfSize = dwbmpsize;
	bmfhdr.bfReserved1 = 0;
	bmfhdr.bfReserved2 = 0;
	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
		(DWORD)sizeof(BITMAPINFOHEADER) + dwtsbsize;

	// 写入位图文件头
	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);

	// 写入位图文件其余内容
	WriteFile(fh, (LPSTR)lpbi, dwbitsize, &dwwritten, NULL);
	//清除 
	VirtualFree(hdib, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_DECOMMIT);
	CloseHandle(fh);
}

int main()
{
	screenshot();
	return 0;
}

