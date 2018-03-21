// ScreenShot.cpp : �������̨Ӧ�ó������ڵ㡣
//��ȡ��Ļ

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
//��ͼ1
int screenshot1(string path)
{
	//��Ļ���ڴ��豸������
	HDC hDC, hMemDC;
	//��Ļ�ֱ���
	int x, y;
	//λͼ���
	HBITMAP hbmp, holdbmp;
	//��ȡ��ĻDC
	hDC = GetDC(NULL);
	//����һ���ڴ�DC
	hMemDC = CreateCompatibleDC(hDC);
	//��ȡ��Ļ��С
	x = GetDeviceCaps(hDC, HORZRES);
	y = GetDeviceCaps(hDC, VERTRES);
	//����һ������Ļ�豸��������ݵ� λͼ
	hbmp = CreateCompatibleBitmap(hDC, x, y);
	//����λͼѡ���ڴ��豸��������
	holdbmp = (HBITMAP)SelectObject(hMemDC, hbmp);
	//����Ļ�豸�����������ڴ��豸��������
	BitBlt(hMemDC, 0, 0, x, y, hDC, 0, 0, SRCCOPY);
	//�õ�λͼ���
	hbmp = (HBITMAP)SelectObject(hMemDC, holdbmp);

	//SaveBitmapToFile(hbmp, path);
	//return 1;
	//���
	DeleteDC(hMemDC);

	//����λͼ
	//����ÿ��������ռ�ֽ���
	int ibits;
	WORD bitcount;
	//hDC= CreateDC(stringToLPCWSTR("DISPLAY"), NULL, NULL, NULL);
	ibits = GetDeviceCaps(hDC, BITSPIXEL)*GetDeviceCaps(hDC, PLANES);
	//���
	DeleteDC(hDC);
	if (ibits <= 1)
		bitcount = 1;
	else  if (ibits <= 4)
		bitcount = 4;
	else if (ibits <= 8)
		bitcount = 8;
	else
		bitcount = 24;

	BITMAP bmp; //λͼ���Խṹ
	BITMAPFILEHEADER bmfhdr; //λͼ�ļ�ͷ�ṹ
	BITMAPINFOHEADER bi; //λͼ��Ϣͷ�ṹ
	LPBITMAPINFOHEADER lpbi; //ָ��λͼ��Ϣͷ�ṹ
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

	//�����ɫ���С,λͼ�������ֽڴ�С��λͼ�ļ���Сд���ļ��ֽ���    
	DWORD dwPaletteSize =0, dwBmBitsSize = 0, dwwritten=0;//��ɫ��;
	dwBmBitsSize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
	//Ϊλͼ�����ڴ�
	HANDLE fh, hdib, hpal, holdpal = NULL;
	hdib = GlobalAlloc(GHND, dwPaletteSize + dwBmBitsSize + sizeof(BITMAPINFOHEADER));
	lpbi = (LPBITMAPINFOHEADER)GlobalLock(hdib);
	*lpbi = bi;

	// �����ɫ�� 
	hpal = GetStockObject(DEFAULT_PALETTE);
	if (hpal)
	{
		holdpal = SelectPalette(hDC, (HPALETTE)hpal, false);
		RealizePalette(hDC);
	}
	// ��ȡ�õ�ɫ�����µ�����ֵ
	GetDIBits(hDC, hbmp, 0, (UINT)bmp.bmHeight, (LPSTR)lpbi +
		sizeof(BITMAPINFOHEADER) + dwPaletteSize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
	//�ָ���ɫ�� 
	if (holdpal)
	{
		SelectPalette(hDC, (HPALETTE)holdpal, true);
		RealizePalette(hDC);
		ReleaseDC(NULL, hDC);
	}

	//����λͼ�ļ� 
	fh = CreateFile(stringToLPCWSTR(path), GENERIC_WRITE, 0, NULL,
		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
	if (fh == INVALID_HANDLE_VALUE)
		return 0;

	// ����λͼ�ļ�ͷ
	bmfhdr.bfType = 0x4d42; //"bm"
	//λͼ��С
	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
		dwBmBitsSize + dwPaletteSize;
	bmfhdr.bfSize = dwbmpsize;
	bmfhdr.bfReserved1 = 0;
	bmfhdr.bfReserved2 = 0;
	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
		(DWORD)sizeof(BITMAPINFOHEADER) + dwPaletteSize;

	// д��λͼ�ļ�ͷ
	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);
	// д��λͼ�ļ���������
	WriteFile(fh, (LPSTR)lpbi, dwbmpsize, &dwwritten, NULL);
	//���                    
	GlobalUnlock(hdib);
	GlobalFree(hdib);
	CloseHandle(fh);
}
//��ͼ2
HBITMAP CopyScreenToBitmap()
{
	int nWidth, nHeight;
	//��Ļ���ڴ��豸������
	HDC hScrDC, hMemDC;
	//λͼ���
	HBITMAP hBitmap, hOldBitmap;
	//��Ļ�ֱ���
	int xScrn, yScrn;
	//Ϊ��Ļ�����豸������
	hScrDC = GetDC(NULL);
	//Ϊ��Ļ�豸�����������ݵ��ڴ��豸������
	hMemDC = CreateCompatibleDC(hScrDC);
	//�����Ļ�ֱ���
	xScrn = GetDeviceCaps(hScrDC, HORZRES);
	yScrn = GetDeviceCaps(hScrDC, VERTRES);

	//�洢��Ļ�Ŀ��
	nWidth = xScrn;
	nHeight = yScrn;

	//����һ������Ļ�豸��������ݵ� λͼ
	hBitmap = CreateCompatibleBitmap(hScrDC, xScrn, yScrn);
	//����λͼѡ���ڴ��豸��������
	hOldBitmap = (HBITMAP)SelectObject(hMemDC, hBitmap);
	//����Ļ�豸�����������ڴ��豸��������
	BitBlt(hMemDC, 0, 0, xScrn, yScrn, hScrDC, 0, 0, SRCCOPY);
	//�õ���Ļλͼ���
	hBitmap = (HBITMAP)SelectObject(hMemDC, hOldBitmap);
	//���
	DeleteDC(hScrDC);
	DeleteDC(hMemDC);

	//����λͼ���
	return hBitmap;
}
//��ͼ3
BOOL SaveBitmapToFile(HBITMAP   hBitmap, string szfilename)
{
	HDC hScrDC;
	HDC     hDC;
	//��ǰ�ֱ�����ÿ������ռ�ֽ���            
	int     iBits;
	//λͼ��ÿ������ռ�ֽ���            
	WORD     wBitCount;
	//�����ɫ���С��     λͼ�������ֽڴ�С     ��λͼ�ļ���С     ��     д���ļ��ֽ���                
	DWORD     dwPaletteSize = 0, dwBmBitsSize = 0, dwDIBSize = 0, dwWritten = 0;
	//λͼ���Խṹ                
	BITMAP     Bitmap;
	//λͼ�ļ�ͷ�ṹ            
	BITMAPFILEHEADER     bmfHdr;
	//λͼ��Ϣͷ�ṹ                
	BITMAPINFOHEADER     bi;
	//ָ��λͼ��Ϣͷ�ṹ                    
	LPBITMAPINFOHEADER     lpbi;
	//�����ļ��������ڴ�������ɫ����                
	HANDLE     fh, hDib, hPal, hOldPal = NULL;

	//����λͼ�ļ�ÿ��������ռ�ֽ���                
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

	//Ϊλͼ���ݷ����ڴ�                
	hDib = GlobalAlloc(GHND, dwBmBitsSize + dwPaletteSize + sizeof(BITMAPINFOHEADER));
	lpbi = (LPBITMAPINFOHEADER)GlobalLock(hDib);
	*lpbi = bi;

	//     �����ɫ��                    
	hPal = GetStockObject(DEFAULT_PALETTE);
	if (hPal)
	{
		hDC = ::GetDC(NULL);
		hOldPal = ::SelectPalette(hDC, (HPALETTE)hPal, FALSE);
		RealizePalette(hDC);
	}

	//     ��ȡ�õ�ɫ�����µ�����ֵ                
	GetDIBits(hDC, hBitmap, 0, (UINT)Bitmap.bmHeight,
		(LPSTR)lpbi + sizeof(BITMAPINFOHEADER) + dwPaletteSize,
		(BITMAPINFO *)lpbi, DIB_RGB_COLORS);

	//�ָ���ɫ��                    
	if (hOldPal)
	{
		::SelectPalette(hDC, (HPALETTE)hOldPal, TRUE);
		RealizePalette(hDC);
		::ReleaseDC(NULL, hDC);
	}

	//����λͼ�ļ�                    
	fh = CreateFile(stringToLPCWSTR(szfilename), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, NULL);

	if (fh == INVALID_HANDLE_VALUE)         return     FALSE;

	//     ����λͼ�ļ�ͷ                
	bmfHdr.bfType = 0x4D42;     //     "BM"                
	dwDIBSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + dwPaletteSize + dwBmBitsSize;
	bmfHdr.bfSize = dwDIBSize;
	bmfHdr.bfReserved1 = 0;
	bmfHdr.bfReserved2 = 0;
	bmfHdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) + (DWORD)sizeof(BITMAPINFOHEADER) + dwPaletteSize;
	//     д��λͼ�ļ�ͷ                
	WriteFile(fh, (LPSTR)&bmfHdr, sizeof(BITMAPFILEHEADER), &dwWritten, NULL);
	//     д��λͼ�ļ���������                
	WriteFile(fh, (LPSTR)lpbi, dwDIBSize, &dwWritten, NULL);
	//���                    
	GlobalUnlock(hDib);
	GlobalFree(hDib);
	CloseHandle(fh);

	return     TRUE;

}


int main()
{
	LARGE_INTEGER freq;
	LARGE_INTEGER start_t, stop_t;
	double exe_time;
	QueryPerformanceFrequency(&freq);
	QueryPerformanceCounter(&start_t);
	//screenshot();
	//��ͼ
	HBITMAP img=CopyScreenToBitmap();
	SaveBitmapToFile(img,"D:/1.bmp");
	screenshot1("D:/1.bmp");

	QueryPerformanceCounter(&stop_t);
	exe_time = 1e3*(stop_t.QuadPart - start_t.QuadPart) / freq.QuadPart;
	cout << "��ʱ" << exe_time << "����" << endl;
	Mat imgb=imread("D:/1.bmp");
	imshow("imgb", imgb);
	waitKey();
	return 0;
}

