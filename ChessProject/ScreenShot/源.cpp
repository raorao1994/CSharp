#include "stdafx.h"
#include<opencv2/opencv.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include<windows.h>

using namespace std;
using namespace cv;
//
//LPCWSTR stringToLPCWSTR(std::string orig)
//{
//	size_t origsize = orig.length() + 1;
//	const size_t newsize = 100;
//	size_t convertedChars = 0;
//	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
//	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
//
//	return wcstring;
//}
//
////��ͼ1
//int screenshot()
//{
//	//��Ļ���ڴ��豸������
//	HDC hDC, hMemDC;
//	//��Ļ�ֱ���
//	int x, y;
//	//λͼ���
//	HBITMAP hbmp, holdbmp;
//	DWORD err;
//
//	BOOL b;
//
//	//��ȡ��ĻDC
//	hDC = GetDC(NULL);
//	err = GetLastError();
//	//����һ���ڴ�DC
//	hMemDC = CreateCompatibleDC(hDC);
//	err = GetLastError();
//
//	//��ȡ��Ļ��С
//	x = GetDeviceCaps(hDC, HORZRES);
//	y = GetDeviceCaps(hDC, VERTRES);
//	//����һ������ĻDC��ͬ��λͼ
//	hbmp = ::CreateCompatibleBitmap(hDC, x, y);
//	err = ::GetLastError();
//	//��λͼѡ���ڴ�DC��
//	holdbmp = (HBITMAP)::SelectObject(hMemDC, hbmp);
//	//����ĻDC�������ڴ�DC��
//	b = BitBlt(hMemDC, 0, 0, x, y, hDC, x, y, SRCCOPY);
//	//�õ�λͼ���
//	hbmp = (HBITMAP)::SelectObject(hMemDC, holdbmp);
//	//����ÿ��������ռ�ֽ���
//	int ibits;
//	WORD bitcount;
//	ibits = GetDeviceCaps(hDC, BITSPIXEL)*GetDeviceCaps(hDC, PLANES);
//	if (ibits <= 1)
//		bitcount = 1;
//	else if (ibits <= 4)
//		bitcount = 4;
//	else if (ibits <= 8)
//		bitcount = 8;
//	else if (ibits <= 16)
//		bitcount = 16;
//	else if (ibits <= 24)
//		bitcount = 24;
//	else
//		bitcount = 32;
//	DWORD dwtsbsize = 0, dwwritten;//��ɫ��
//								   //�����ɫ���С
//	if (bitcount <= 8)
//		dwtsbsize = (1 << bitcount) * sizeof(RGBQUAD);
//
//	BITMAP bmp; //λͼ���Խṹ
//	BITMAPFILEHEADER bmfhdr; //λͼ�ļ�ͷ�ṹ
//	BITMAPINFOHEADER bi; //λͼ��Ϣͷ�ṹ
//	LPBITMAPINFOHEADER lpbi; //ָ��λͼ��Ϣͷ�ṹ
//	GetObject(hbmp, sizeof(bmp), (LPVOID)&bmp);
//	bi.biSize = sizeof(BITMAPINFOHEADER);
//	bi.biWidth = bmp.bmWidth;
//	bi.biHeight = bmp.bmHeight;
//	bi.biPlanes = 1;
//	bi.biBitCount = bitcount;
//	bi.biCompression = BI_RGB;
//	bi.biSizeImage = 0;
//	bi.biXPelsPerMeter = 0;
//	bi.biYPelsPerMeter = 0;
//	bi.biClrUsed = 0;
//	bi.biClrImportant = 0;
//
//	//λͼ��С
//	DWORD dwbitsize;
//	dwbitsize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
//	//Ϊλͼ�����ڴ�
//	HANDLE fh, hdib, hpal, holdpal = NULL;
//	hdib = VirtualAlloc(NULL, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_COMMIT, PAGE_READWRITE);
//	lpbi = (LPBITMAPINFOHEADER)hdib;
//	*lpbi = bi;
//	// �����ɫ�� 
//	hpal = GetStockObject(DEFAULT_PALETTE);
//	if (hpal)
//	{
//		holdpal = SelectPalette(hDC, (HPALETTE)hpal, false);
//		RealizePalette(hDC);
//	}
//	// ��ȡ�õ�ɫ�����µ�����ֵ
//	GetDIBits(hDC, hbmp, 0, (UINT)bmp.bmHeight, (LPSTR)lpbi +
//		sizeof(BITMAPINFOHEADER) + dwtsbsize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
//	//�ָ���ɫ�� 
//	if (holdpal)
//	{
//		SelectPalette(hDC, (HPALETTE)holdpal, true);
//		RealizePalette(hDC);
//	}
//
//	//����λͼ�ļ� 
//	string p = "D:/1.bmp";//C:/1.bmp
//	fh = CreateFile(stringToLPCWSTR(p), GENERIC_WRITE, 0, NULL,
//		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
//		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
//	if (fh == INVALID_HANDLE_VALUE)
//		return 0;
//
//	// ����λͼ�ļ�ͷ
//	bmfhdr.bfType = 0x4d42; //"bm"
//							//λͼ��С
//	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
//		dwtsbsize + dwbitsize;
//	bmfhdr.bfSize = dwbmpsize;
//	bmfhdr.bfReserved1 = 0;
//	bmfhdr.bfReserved2 = 0;
//	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
//		(DWORD)sizeof(BITMAPINFOHEADER) + dwtsbsize;
//
//	// д��λͼ�ļ�ͷ
//	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);
//
//	// д��λͼ�ļ���������
//	WriteFile(fh, (LPSTR)lpbi, dwbitsize, &dwwritten, NULL);
//	//��� 
//	VirtualFree(hdib, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER), MEM_DECOMMIT);
//	CloseHandle(fh);
//}
//
LPCWSTR stringToLPCWSTR1(std::string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}
//��ͼ1
//int screenshot1(string path)
//{
//	//��Ļ���ڴ��豸������
//	HDC hDC, hMemDC;
//	//��ȡ��ĻDC
//	hDC = GetDC(NULL);
//	//����һ���ڴ�DC
//	hMemDC = CreateCompatibleDC(hDC);
//	//��Ļ�ֱ���
//	int x, y;
//	//λͼ���
//	HBITMAP hbmp, holdbmp;
//
//	//��ȡ��Ļ��С
//	x = GetDeviceCaps(hDC, HORZRES);
//	y = GetDeviceCaps(hDC, VERTRES);
//	//����һ������Ļ�豸��������ݵ� λͼ
//	hbmp = CreateCompatibleBitmap(hDC, x, y);
//	//����λͼѡ���ڴ��豸��������
//	holdbmp = (HBITMAP)SelectObject(hMemDC, hbmp);
//	//����Ļ�豸�����������ڴ��豸��������
//	BitBlt(hMemDC, 0, 0, x, y, hDC, x, y, SRCCOPY);
//	//�õ�λͼ���
//	hbmp = (HBITMAP)SelectObject(hMemDC, holdbmp);
//	//���
//	DeleteDC(hMemDC);
//
//	//����λͼ
//	//����ÿ��������ռ�ֽ���
//	int ibits;
//	WORD bitcount;
//	ibits = GetDeviceCaps(hDC, BITSPIXEL)*GetDeviceCaps(hDC, PLANES);
//	//���
//	DeleteDC(hDC);
//	if (ibits <= 1)
//		bitcount = 1;
//	else  if (ibits <= 4)
//		bitcount = 4;
//	else if (ibits <= 8)
//		bitcount = 8;
//	else
//		bitcount = 24;
//
//	BITMAP bmp; //λͼ���Խṹ
//	BITMAPFILEHEADER bmfhdr; //λͼ�ļ�ͷ�ṹ
//	BITMAPINFOHEADER bi; //λͼ��Ϣͷ�ṹ
//	LPBITMAPINFOHEADER lpbi; //ָ��λͼ��Ϣͷ�ṹ
//	GetObject(hbmp, sizeof(bmp), (LPVOID)&bmp);
//	bi.biSize = sizeof(BITMAPINFOHEADER);
//	bi.biWidth = bmp.bmWidth;
//	bi.biHeight = bmp.bmHeight;
//	bi.biPlanes = 1;
//	bi.biBitCount = bitcount;
//	bi.biCompression = BI_RGB;
//	bi.biSizeImage = 0;
//	bi.biXPelsPerMeter = 0;
//	bi.biYPelsPerMeter = 0;
//	bi.biClrUsed = 0;
//	bi.biClrImportant = 0;
//
//	//λͼ��С
//	DWORD dwbitsize, dwtsbsize = 0, dwwritten;//��ɫ��;
//	dwbitsize = ((bmp.bmWidth*bitcount + 31) / 32) * 4 * bmp.bmHeight;
//	//Ϊλͼ�����ڴ�
//	HANDLE fh, hdib, hpal, holdpal = NULL;
//	hdib = GlobalAlloc(GHND, dwbitsize + dwtsbsize + sizeof(BITMAPINFOHEADER));
//	lpbi = (LPBITMAPINFOHEADER)GlobalLock(hdib);
//	*lpbi = bi;	
//	
//	// �����ɫ�� 
//	hpal = GetStockObject(DEFAULT_PALETTE);
//	if (hpal)
//	{
//		holdpal = SelectPalette(hDC, (HPALETTE)hpal, false);
//		RealizePalette(hDC);
//	}
//	// ��ȡ�õ�ɫ�����µ�����ֵ
//	GetDIBits(hDC, hbmp, 0, (UINT)bmp.bmHeight, (LPSTR)lpbi +
//		sizeof(BITMAPINFOHEADER) + dwtsbsize, (BITMAPINFO*)lpbi, DIB_RGB_COLORS);
//	//�ָ���ɫ�� 
//	if (holdpal)
//	{
//		SelectPalette(hDC, (HPALETTE)holdpal, true);
//		RealizePalette(hDC);
//		ReleaseDC(NULL, hDC);
//	}
//
//	//����λͼ�ļ� 
//	fh = CreateFile(stringToLPCWSTR(path), GENERIC_WRITE, 0, NULL,
//		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL |
//		FILE_FLAG_SEQUENTIAL_SCAN, NULL);
//	if (fh == INVALID_HANDLE_VALUE)
//		return 0;
//
//	// ����λͼ�ļ�ͷ
//	bmfhdr.bfType = 0x4d42; //"bm"
//							//λͼ��С
//	DWORD dwbmpsize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) +
//		dwtsbsize + dwbitsize;
//	bmfhdr.bfSize = dwbmpsize;
//	bmfhdr.bfReserved1 = 0;
//	bmfhdr.bfReserved2 = 0;
//	bmfhdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
//		(DWORD)sizeof(BITMAPINFOHEADER) + dwtsbsize;
//
//	// д��λͼ�ļ�ͷ
//	WriteFile(fh, (LPSTR)&bmfhdr, sizeof(BITMAPFILEHEADER), &dwwritten, NULL);
//	// д��λͼ�ļ���������
//	WriteFile(fh, (LPSTR)lpbi, dwbitsize, &dwwritten, NULL);
//	//���                    
//	GlobalUnlock(hdib);
//	GlobalFree(hdib);
//	CloseHandle(fh);
//}
//��ͼ2
HBITMAP CopyScreenToBitmap1()
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
BOOL SaveBitmapToFile1(HBITMAP   hBitmap, string szfilename)
{
	HDC hScrDC;
	HDC     hDC;
	//��ǰ�ֱ�����ÿ������ռ�ֽ���            
	int     iBits;
	//λͼ��ÿ������ռ�ֽ���            
	WORD     wBitCount;
	
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
	hDC = CreateDC(stringToLPCWSTR1("DISPLAY"), NULL, NULL, NULL);
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
	//�����ɫ���С��     λͼ�������ֽڴ�С     ��λͼ�ļ���С     ��     д���ļ��ֽ���                
	DWORD     dwPaletteSize = 0, dwBmBitsSize = 0, dwDIBSize = 0, dwWritten = 0;
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
	fh = CreateFile(stringToLPCWSTR1(szfilename), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
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