// ScreenShot.cpp : �������̨Ӧ�ó������ڵ㡣
//��ȡ��Ļ

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

	//��ȡ��ĻDC
	hDC = GetDC(NULL);
	err = GetLastError();
	//����һ���ڴ�DC
	hMemDC = CreateCompatibleDC(hDC);
	err = GetLastError();

	//��ȡ��Ļ��С
	x = GetDeviceCaps(hDC, HORZRES);
	y = GetDeviceCaps(hDC, VERTRES);
	//����һ������ĻDC��ͬ��λͼ
	hbmp = ::CreateCompatibleBitmap(hDC, x, y);
	err = ::GetLastError();
	//��λͼѡ���ڴ�DC��
	holdbmp = (HBITMAP)::SelectObject(hMemDC, hbmp);
	//����ĻDC�������ڴ�DC��
	b = BitBlt(hMemDC, 0, 0, x, y, hDC, x, y, SRCCOPY);
	//�õ�λͼ���
	hbmp = (HBITMAP)::SelectObject(hMemDC, holdbmp);
	//����ÿ��������ռ�ֽ���
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
	DWORD dwtsbsize = 0, dwwritten;//��ɫ��
								   //�����ɫ���С
	if (bitcount <= 8)
		dwtsbsize = (1 << bitcount) * sizeof(RGBQUAD);

	BITMAP bmp; //λͼ���Խṹ
	BITMAPFILEHEADER bmfhdr; //λͼ�ļ�ͷ�ṹ
	BITMAPINFOHEADER bi; //λͼ��Ϣͷ�ṹ
	LPBITMAPINFOHEADER lpbi; //ָ��λͼ��Ϣͷ�ṹ
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

	//λͼ��С
	DWORD dwbitsize;
	dwbitsize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
	//Ϊλͼ�����ڴ�
	HANDLE fh, hdib, hpal, holdpal = NULL;
	hdib = VirtualAlloc(NULL, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_COMMIT, PAGE_READWRITE);
	lpbi = (LPBITMAPINFOHEADER)hdib;
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
		sizeof(BITMAPINFOHEADER) + dwtsbsize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
	//�ָ���ɫ�� 
	if (holdpal)
	{
		SelectPalette(hDC, (HPALETTE)holdpal, true);
		RealizePalette(hDC);
	}

	//����λͼ�ļ� 
	string p = "D:/1.bmp";//C:/1.bmp
	fh = CreateFile(stringToLPCWSTR(p), GENERIC_WRITE, 0, NULL,
		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
	if (fh == INVALID_HANDLE_VALUE)
		return 0;

	// ����λͼ�ļ�ͷ
	bmfhdr.bfType = 0x4d42; //"bm"
	//λͼ��С
	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
		dwtsbsize + dwbitsize;
	bmfhdr.bfSize = dwbmpsize;
	bmfhdr.bfReserved1 = 0;
	bmfhdr.bfReserved2 = 0;
	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
		(DWORD)sizeof(BITMAPINFOHEADER) + dwtsbsize;

	// д��λͼ�ļ�ͷ
	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);

	// д��λͼ�ļ���������
	WriteFile(fh, (LPSTR)lpbi, dwbitsize, &dwwritten, NULL);
	//��� 
	VirtualFree(hdib, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_DECOMMIT);
	CloseHandle(fh);
}

int main()
{
	screenshot();
	return 0;
}

