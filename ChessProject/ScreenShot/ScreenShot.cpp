// ScreenShot.cpp : 定义控制台应用程序的入口点。
//截取屏幕

#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>

using namespace std;
using namespace cv;

BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename);

LPCWSTR stringToLPCWSTR(string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}
//截图1
int screenshot1(string path)
{
	//屏幕和内存设备描述表
	HDC hDC, hMemDC;
	//屏幕分辨率
	int x, y;
	//位图句柄
	HBITMAP hbmp, holdbmp;
	//获取屏幕DC
	hDC = GetDC(NULL);
	//创建一个内存DC
	hMemDC = CreateCompatibleDC(hDC);
	//获取屏幕大小
	x = GetDeviceCaps(hDC, HORZRES);
	y = GetDeviceCaps(hDC, VERTRES);
	//创建一个与屏幕设备描述表兼容的 位图
	hbmp = CreateCompatibleBitmap(hDC, x, y);
	//把新位图选到内存设备描述表中
	holdbmp = (HBITMAP)SelectObject(hMemDC, hbmp);
	//把屏幕设备描述表拷贝到内存设备描述表中
	BitBlt(hMemDC, 0, 0, x, y, hDC, 0, 0, SRCCOPY);
	//得到位图句柄
	hbmp = (HBITMAP)SelectObject(hMemDC, holdbmp);

	//SaveBitmapToFile(hbmp, path);
	//return 1;
	//清除
	DeleteDC(hMemDC);

	//保存位图
	//计算每个像数所占字节数
	int ibits;
	WORD bitcount;
	//hDC= CreateDC(stringToLPCWSTR("DISPLAY"), NULL, NULL, NULL);
	ibits = GetDeviceCaps(hDC, BITSPIXEL)*GetDeviceCaps(hDC, PLANES);
	//清除
	DeleteDC(hDC);
	if (ibits <= 1)
		bitcount = 1;
	else  if (ibits <= 4)
		bitcount = 4;
	else if (ibits <= 8)
		bitcount = 8;
	else
		bitcount = 24;

	BITMAP bmp; //位图属性结构
	BITMAPFILEHEADER bmfhdr; //位图文件头结构
	BITMAPINFOHEADER bi; //位图信息头结构
	LPBITMAPINFOHEADER lpbi; //指向位图信息头结构
	GetObject(hbmp, sizeof(bmp), (LPSTR)&bmp);
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

	//定义调色板大小,位图中像素字节大小，位图文件大小写入文件字节数    
	DWORD dwPaletteSize =0, dwBmBitsSize = 0, dwwritten=0;//调色板;
	dwBmBitsSize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
	//为位图分配内存
	HANDLE fh, hdib, hpal, holdpal = NULL;
	hdib = GlobalAlloc(GHND, dwPaletteSize + dwBmBitsSize + sizeof(BITMAPINFOHEADER));
	lpbi = (LPBITMAPINFOHEADER)GlobalLock(hdib);
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
		sizeof(BITMAPINFOHEADER) + dwPaletteSize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
	//恢复调色板 
	if (holdpal)
	{
		SelectPalette(hDC, (HPALETTE)holdpal, true);
		RealizePalette(hDC);
		ReleaseDC(NULL, hDC);
	}

	//创建位图文件 
	fh = CreateFile(stringToLPCWSTR(path), GENERIC_WRITE, 0, NULL,
		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
	if (fh == INVALID_HANDLE_VALUE)
		return 0;

	// 设置位图文件头
	bmfhdr.bfType = 0x4d42; //"bm"
	//位图大小
	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
		dwBmBitsSize + dwPaletteSize;
	bmfhdr.bfSize = dwbmpsize;
	bmfhdr.bfReserved1 = 0;
	bmfhdr.bfReserved2 = 0;
	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
		(DWORD)sizeof(BITMAPINFOHEADER) + dwPaletteSize;

	// 写入位图文件头
	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);
	// 写入位图文件其余内容
	WriteFile(fh, (LPSTR)lpbi, dwbmpsize, &dwwritten, NULL);
	//清除                    
	GlobalUnlock(hdib);
	GlobalFree(hdib);
	CloseHandle(fh);
}
//截图2
HBITMAP CopyScreenToBitmap()
{
	int nWidth, nHeight;
	//屏幕和内存设备描述表
	HDC hScrDC, hMemDC;
	//位图句柄
	HBITMAP hBitmap, hOldBitmap;
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
	//清除
	DeleteDC(hScrDC);
	DeleteDC(hMemDC);
	DeleteObject(hOldBitmap);
	//返回位图句柄
	return hBitmap;
}
//截图3
BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename)
{
	HDC hScrDC;
	HDC     hDC;
	//当前分辨率下每象素所占字节数            
	int     iBits;
	//位图中每象素所占字节数            
	WORD     wBitCount;
	//定义调色板大小，     位图中像素字节大小     ，位图文件大小     ，     写入文件字节数                
	DWORD     dwPaletteSize = 0, dwBmBitsSize = 0, dwDIBSize = 0, dwWritten = 0;
	//位图属性结构                
	BITMAP     Bitmap;
	//位图文件头结构            
	BITMAPFILEHEADER     bmfHdr;
	//位图信息头结构                
	BITMAPINFOHEADER     bi;
	//指向位图信息头结构                    
	LPBITMAPINFOHEADER     lpbi;
	//定义文件，分配内存句柄，调色板句柄                
	HANDLE     fh, hDib, hPal, hOldPal = NULL;

	//计算位图文件每个像素所占字节数                
	hDC = CreateDC(stringToLPCWSTR("DISPLAY"), NULL, NULL, NULL);
	iBits = GetDeviceCaps(hDC, BITSPIXEL)     *     GetDeviceCaps(hDC, PLANES);
	DeleteDC(hDC);
	if (iBits <= 1)
		wBitCount = 1;
	else  if (iBits <= 4)
		wBitCount = 4;
	else if (iBits <= 8)
		wBitCount = 8;
	else
		wBitCount = 24;

	GetObject(hBitmap, sizeof(Bitmap), (LPSTR)&Bitmap);
	bi.biSize = sizeof(BITMAPINFOHEADER);
	bi.biWidth = Bitmap.bmWidth;
	bi.biHeight = Bitmap.bmHeight;
	bi.biPlanes = 1;
	bi.biBitCount = wBitCount;
	bi.biCompression = BI_RGB;
	bi.biSizeImage = 0;
	bi.biXPelsPerMeter = 0;
	bi.biYPelsPerMeter = 0;
	bi.biClrImportant = 0;
	bi.biClrUsed = 0;

	dwBmBitsSize = ((Bitmap.bmWidth *wBitCount + 31) / 32) * 4 * Bitmap.bmHeight;

	//为位图内容分配内存                
	hDib = GlobalAlloc(GHND, dwBmBitsSize + dwPaletteSize + sizeof(BITMAPINFOHEADER));
	lpbi = (LPBITMAPINFOHEADER)GlobalLock(hDib);
	*lpbi = bi;

	//     处理调色板                    
	hPal = GetStockObject(DEFAULT_PALETTE);
	if (hPal)
	{
		hDC = ::GetDC(NULL);
		hOldPal = ::SelectPalette(hDC, (HPALETTE)hPal, FALSE);
		RealizePalette(hDC);
	}

	//     获取该调色板下新的像素值                
	GetDIBits(hDC, hBitmap, 0, (UINT)Bitmap.bmHeight,
		(LPSTR)lpbi + sizeof(BITMAPINFOHEADER) + dwPaletteSize,
		(BITMAPINFO *)lpbi, DIB_RGB_COLORS);

	//恢复调色板                    
	if (hOldPal)
	{
		::SelectPalette(hDC, (HPALETTE)hOldPal, TRUE);
		RealizePalette(hDC);
		::ReleaseDC(NULL, hDC);
	}

	//创建位图文件                    
	fh = CreateFile(stringToLPCWSTR(szfilename), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, NULL);

	if (fh == INVALID_HANDLE_VALUE)         return     FALSE;

	//     设置位图文件头                
	bmfHdr.bfType = 0x4D42;     //     "BM"                
	dwDIBSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + dwPaletteSize + dwBmBitsSize;
	bmfHdr.bfSize = dwDIBSize;
	bmfHdr.bfReserved1 = 0;
	bmfHdr.bfReserved2 = 0;
	bmfHdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) + (DWORD)sizeof(BITMAPINFOHEADER) + dwPaletteSize;
	//     写入位图文件头                
	WriteFile(fh, (LPSTR)&bmfHdr, sizeof(BITMAPFILEHEADER), &dwWritten, NULL);
	//     写入位图文件其余内容                
	WriteFile(fh, (LPSTR)lpbi, dwDIBSize, &dwWritten, NULL);
	//清除                    
	GlobalUnlock(hDib);
	GlobalFree(hDib);
	CloseHandle(fh);

	return     TRUE;

}
//_mat图像转HBITMAP
BOOL MatToHBitmap(HBITMAP& _hBmp, Mat& _mat)
{
	//MAT类的TYPE=（nChannels-1+ CV_8U）<<3
	int nChannels = (_mat.type() >> 3) - CV_8U + 1;
	int iSize = _mat.cols*_mat.rows*nChannels;
	_hBmp = CreateBitmap(_mat.cols, _mat.rows,
		1, nChannels * 8, _mat.data);
	return TRUE;

}
//HBITMAP图像转_mat
BOOL HBitmapToMat(HBITMAP& _hBmp, Mat& _mat)
{
	BITMAP bmp;
	GetObject(_hBmp, sizeof(BITMAP), &bmp);
	int nChannels = bmp.bmBitsPixel == 1 ? 1 : bmp.bmBitsPixel / 8;
	int depth = bmp.bmBitsPixel == 1 ? IPL_DEPTH_1U : IPL_DEPTH_8U;
	Mat v_mat;
	v_mat.create(cvSize(bmp.bmWidth, bmp.bmHeight), CV_MAKETYPE(CV_8U, nChannels));
	GetBitmapBits(_hBmp, bmp.bmHeight*bmp.bmWidth*nChannels, v_mat.data);
	_mat = v_mat;
	return TRUE;
}
int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);
	
	while (true) {
		//截图
		QueryPerformanceCounter(&start_t);
		//screenshot();
		HBITMAP img = CopyScreenToBitmap();
		//SaveBitmapToFile(img, "D:/1.bmp");
		//screenshot1("D:/1.bmp");
		QueryPerformanceCounter(&stop_t);
		exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
		cout << "耗时" << exe_time << "毫秒" << endl;
		//Mat imgb = imread("D:/1.bmp");
		Mat imgb;
		if(HBitmapToMat(img, imgb))
			imshow("imgb", imgb);
		waitKey(1000);
	}
	
	waitKey();
	return 0;
}

