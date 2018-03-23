#include "stdafx.h"
#include "Helper.h"


Helper::Helper(int Pix, double minThs)
{
	aroundPix = Pix;
	minTh = minThs;
}

Helper::~Helper()
{
}

/*
*stringToLPCWSTR ��stringת��ΪLPCWSTR
*/
LPCWSTR Helper::stringToLPCWSTR(string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}
/*
*CopyScreenToBitmap ��ȡ��Ļλͼ
*/
HBITMAP Helper::CopyScreenToBitmap()
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
/*
*SaveBitmapToFile ������Ļλͼ
*HBITMAP hBitmap λͼ
*string szfilename����·��
*/
BOOL Helper::SaveBitmapToFile(HBITMAP hBitmap, string szfilename)
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
/*
*��ȡ����ģ��ͼƬ
*/
void Helper::GetAllTemp(string path)
{
	intptr_t hFile = 0;
	struct  _finddata_t  fileinfo;
	string p;
	long i;
	if ((hFile = _findfirst(p.assign(path).append("\\*").c_str(), &fileinfo)) != -1)
	{
		do
		{
			if (!(fileinfo.attrib& _A_SUBDIR))
			{
				string tmp_path = p.assign(path).append("\\").append(fileinfo.name);
				Mat temp = imread(tmp_path, CV_LOAD_IMAGE_GRAYSCALE);
				TempImgs.push_back(temp);
				Lables.push_back(fileinfo.name);
			}
		} while (_findnext(hFile, &fileinfo) == 0);
	}
}
/*
*ʶ��ͼƬ�е��˿���
*gary��ʶ��ĻҶ�ͼ
*srcԭͼ
*/
vector<string> Helper::Recognition(Mat gary, Mat src)
{
	vector<string> resultLable;
	for (size_t i = 0; i < TempImgs.size(); i++)
	{
		Mat result;
		//2����ȡģ��ͼƬ
		Mat temp = TempImgs[i];
		int width = gary.cols;
		int height = gary.rows;
		//3��ƥ����
		int result_cols = gary.cols - temp.cols + 1;
		int result_rows = gary.rows - temp.rows + 1;
		//4��ͼ��ƥ��
		//��������ʹ�õ�ƥ���㷨�Ǳ�׼ƽ����ƥ�� method=CV_TM_SQDIFF_NORMED����ֵԽСƥ���Խ��
		matchTemplate(gary, temp, result, CV_TM_SQDIFF_NORMED);
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
		//double matchValue = result.at<float>(minLoc.y, minLoc.x);
		//8���ж��ٽ������Ƿ����ƥ���
		int count = 0;//ͬһֻ�Ʋ��ܳ����ĸ�
		for (int x = minLoc.x - aroundPix*3; x<minLoc.x + aroundPix*3; x++)
		{
			for (int y = minLoc.y- aroundPix*1.5; y < minLoc.y + aroundPix*1.5; y++)
			{
				if (x >= width, y >= height)continue;
				if (count >= 4)break;//ͬһֻ�Ʋ��ܳ����ĸ�
				//4.2���resultImg��(j,x)λ�õ�ƥ��ֵmatchValue  
				double matchValue = 1;
				try
				{
					matchValue = result.at<float>(y, x);
				}
				catch (exception e) {
					matchValue = 1;
					continue;
				}
				//4.3����ɸѡ����  
				//����1:����ֵ����0.9  
				if (matchValue < minTh)
				{
					//cout << "ƥ��ȣ�" << matchValue << endl;
					//5.��ɸѡ���ĵ㻭���߿������  
					rectangle(src, Point(x, y), Point(x + temp.cols, y + temp.rows),
						Scalar(0, 255, 0), 2, 8, 0);
					int index = Lables[i].find('.');
					string a = Lables[i].substr(0, index);
					resultLable.push_back(a);
					count++; x += 10;
				}
			}
		}
	}
	return resultLable;
}
/*
*��ȡģ��ͼƬ��ͼƬ�е�����
*/
Point Helper::GetTempPoint(string imgPath,string tempPath)
{
	Point p(0,0);
	Mat result;
	Mat img= imread(imgPath, CV_LOAD_IMAGE_GRAYSCALE);
	//2����ȡģ��ͼƬ
	Mat temp = imread(tempPath, CV_LOAD_IMAGE_GRAYSCALE);
	//3��ƥ����
	int result_cols = img.cols - temp.cols + 1;
	int result_rows = img.rows - temp.rows + 1;
	//4��ͼ��ƥ��
	//��������ʹ�õ�ƥ���㷨�Ǳ�׼ƽ����ƥ�� method=CV_TM_SQDIFF_NORMED����ֵԽСƥ���Խ��
	matchTemplate(img, temp, result, CV_TM_SQDIFF_NORMED);
	//5����׼��һ��
	normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
	//6�������ƥ��ֵ
	//��Ŀ��ƥ��
	double minVal = -1;
	double maxVal;
	Point minLoc;
	Point maxLoc;
	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
	if (minVal > minTh) return p;
	//7�����Ƴ�ƥ������
	/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
	Scalar(0, 0, 0), 2, 8, 0);*/
	return minLoc;
}
//_matͼ��תHBITMAP
BOOL Helper::MatToHBitmap(HBITMAP& _hBmp, Mat& _mat)
{
	//MAT���TYPE=��nChannels-1+ CV_8U��<<3
	int nChannels = (_mat.type() >> 3) - CV_8U + 1;
	int iSize = _mat.cols*_mat.rows*nChannels;
	_hBmp = CreateBitmap(_mat.cols, _mat.rows,
		1, nChannels * 8, _mat.data);
	return TRUE;

}
//HBITMAPͼ��ת_mat
BOOL Helper::HBitmapToMat(HBITMAP& _hBmp, Mat& _mat)
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
