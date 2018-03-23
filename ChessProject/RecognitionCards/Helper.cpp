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
*stringToLPCWSTR 将string转化为LPCWSTR
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
*CopyScreenToBitmap 获取屏幕位图
*/
HBITMAP Helper::CopyScreenToBitmap()
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

	//返回位图句柄
	return hBitmap;
}
/*
*SaveBitmapToFile 保存屏幕位图
*HBITMAP hBitmap 位图
*string szfilename保存路径
*/
BOOL Helper::SaveBitmapToFile(HBITMAP hBitmap, string szfilename)
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
/*
*获取所有模版图片
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
*识别图片中的扑克牌
*gary待识别的灰度图
*src原图
*/
vector<string> Helper::Recognition(Mat gary, Mat src)
{
	vector<string> resultLable;
	for (size_t i = 0; i < TempImgs.size(); i++)
	{
		Mat result;
		//2、读取模版图片
		Mat temp = TempImgs[i];
		int width = gary.cols;
		int height = gary.rows;
		//3、匹配结果
		int result_cols = gary.cols - temp.cols + 1;
		int result_rows = gary.rows - temp.rows + 1;
		//4、图像匹配
		//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
		matchTemplate(gary, temp, result, CV_TM_SQDIFF_NORMED);
		//5、标准归一化
		//normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
		//6、计算出匹配值
		//单目标匹配
		double minVal = -1;
		double maxVal;
		Point minLoc;
		Point maxLoc;
		minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
		if (minVal > minTh)continue;
		//7、绘制出匹配区域
		/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
		Scalar(0, 0, 0), 2, 8, 0);*/
		//double matchValue = result.at<float>(minLoc.y, minLoc.x);
		//8、判断临近坐标是否存在匹配点
		int count = 0;//同一只牌不能超过四个
		for (int x = minLoc.x - aroundPix*3; x<minLoc.x + aroundPix*3; x++)
		{
			for (int y = minLoc.y- aroundPix*1.5; y < minLoc.y + aroundPix*1.5; y++)
			{
				if (x >= width, y >= height)continue;
				if (count >= 4)break;//同一只牌不能超过四个
				//4.2获得resultImg中(j,x)位置的匹配值matchValue  
				double matchValue = 1;
				try
				{
					matchValue = result.at<float>(y, x);
				}
				catch (exception e) {
					matchValue = 1;
					continue;
				}
				//4.3给定筛选条件  
				//条件1:概率值大于0.9  
				if (matchValue < minTh)
				{
					//cout << "匹配度：" << matchValue << endl;
					//5.给筛选出的点画出边框和文字  
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
*获取模版图片在图片中的坐标
*/
Point Helper::GetTempPoint(string imgPath,string tempPath)
{
	Point p(0,0);
	Mat result;
	Mat img= imread(imgPath, CV_LOAD_IMAGE_GRAYSCALE);
	//2、读取模版图片
	Mat temp = imread(tempPath, CV_LOAD_IMAGE_GRAYSCALE);
	//3、匹配结果
	int result_cols = img.cols - temp.cols + 1;
	int result_rows = img.rows - temp.rows + 1;
	//4、图像匹配
	//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
	matchTemplate(img, temp, result, CV_TM_SQDIFF_NORMED);
	//5、标准归一化
	normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
	//6、计算出匹配值
	//单目标匹配
	double minVal = -1;
	double maxVal;
	Point minLoc;
	Point maxLoc;
	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
	if (minVal > minTh) return p;
	//7、绘制出匹配区域
	/*rectangle(src, minLoc, Point(minLoc.x + temp.cols, minLoc.y + temp.rows),
	Scalar(0, 0, 0), 2, 8, 0);*/
	return minLoc;
}
//_mat图像转HBITMAP
BOOL Helper::MatToHBitmap(HBITMAP& _hBmp, Mat& _mat)
{
	//MAT类的TYPE=（nChannels-1+ CV_8U）<<3
	int nChannels = (_mat.type() >> 3) - CV_8U + 1;
	int iSize = _mat.cols*_mat.rows*nChannels;
	_hBmp = CreateBitmap(_mat.cols, _mat.rows,
		1, nChannels * 8, _mat.data);
	return TRUE;

}
//HBITMAP图像转_mat
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
