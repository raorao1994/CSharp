using System;

namespace PublicAccount
{
    /// <summary>
    /// 行业（微信中的副行业）
    /// </summary>
    public class Industry
    {
        /// <summary>
        /// 获取代码
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 获取主行业
        /// </summary>
        public PrimaryIndustryEnum PrimaryIndustry { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="name">名称</param>
        /// <param name="primaryIndustry">主行业</param>
        private Industry(string code, string name, PrimaryIndustryEnum primaryIndustry)
        {
            Code = code;
            Name = name;
            PrimaryIndustry = primaryIndustry;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="primaryIndustry"></param>
        private Industry(int code, string name, PrimaryIndustryEnum primaryIndustry)
            : this(code.ToString(), name, primaryIndustry)
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("行业代码：{0}\r\n名称：{1}\r\n主行业：{2:g}",
                Code, Name, PrimaryIndustry);
        }

        //静态成员
        /// <summary>
        /// 互联网/电子商务
        /// </summary>
        public static readonly Industry InternetOrElectronicBusiness = new Industry(1, "互联网/电子商务", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// IT软件与服务
        /// </summary>
        public static readonly Industry SoftwareOrService = new Industry(2, "IT软件与服务", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// IT硬件与设备
        /// </summary>
        public static readonly Industry HardwareOrDevice = new Industry(3, "IT硬件与设备", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// 电子技术
        /// </summary>
        public static readonly Industry ElectronicTechnique = new Industry(4, "电子技术", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// 通信与运营商
        /// </summary>
        public static readonly Industry CommunicationOrISP = new Industry(5, "通信与运营商", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// 网络游戏
        /// </summary>
        public static readonly Industry OnlineGame = new Industry(6, "网络游戏", PrimaryIndustryEnum.IT科技);
        /// <summary>
        /// 银行
        /// </summary>
        public static readonly Industry Bank = new Industry(7, "银行", PrimaryIndustryEnum.金融业);
        /// <summary>
        /// 基金|理财|信托
        /// </summary>
        public static readonly Industry FundFinanceOrTrust = new Industry(8, "基金|理财|信托", PrimaryIndustryEnum.金融业);
        /// <summary>
        /// 保险
        /// </summary>
        public static readonly Industry Insurance = new Industry(9, "保险", PrimaryIndustryEnum.金融业);
        /// <summary>
        /// 餐饮
        /// </summary>
        public static readonly Industry Catering = new Industry(10, "餐饮", PrimaryIndustryEnum.餐饮);
        /// <summary>
        /// 酒店
        /// </summary>
        public static readonly Industry Hotel = new Industry(11, "酒店", PrimaryIndustryEnum.酒店旅游);
        /// <summary>
        /// 旅游
        /// </summary>
        public static readonly Industry Tour = new Industry(12, "旅游", PrimaryIndustryEnum.酒店旅游);
        /// <summary>
        /// 快递
        /// </summary>
        public static readonly Industry Express = new Industry(13, "快递", PrimaryIndustryEnum.运输与仓储);
        /// <summary>
        /// 物流
        /// </summary>
        public static readonly Industry Logistics = new Industry(14, "物流", PrimaryIndustryEnum.运输与仓储);
        /// <summary>
        /// 仓储
        /// </summary>
        public static readonly Industry Storage = new Industry(15, "仓储", PrimaryIndustryEnum.运输与仓储);
        /// <summary>
        /// 培训
        /// </summary>
        public static readonly Industry Training = new Industry(16, "培训", PrimaryIndustryEnum.教育);
        /// <summary>
        /// 院校
        /// </summary>
        public static readonly Industry Academy = new Industry(17, "院校", PrimaryIndustryEnum.教育);
        /// <summary>
        /// 学术研究
        /// </summary>
        public static readonly Industry AcademicResearch = new Industry(18, "学术研究", PrimaryIndustryEnum.政府与公共事业);
        /// <summary>
        /// 交警
        /// </summary>
        public static readonly Industry TrafficPolice = new Industry(19, "交警", PrimaryIndustryEnum.政府与公共事业);
        /// <summary>
        /// 博物馆
        /// </summary>
        public static readonly Industry Museum = new Industry(20, "博物馆", PrimaryIndustryEnum.政府与公共事业);
        /// <summary>
        /// 公共事业|非盈利机构
        /// </summary>
        public static readonly Industry PublicServiceOrNonprofitOrganization = new Industry(21, "公共事业|非盈利机构", PrimaryIndustryEnum.政府与公共事业);
        /// <summary>
        /// 医药医疗
        /// </summary>
        public static readonly Industry MedicineOrMedical = new Industry(22, "医药医疗", PrimaryIndustryEnum.医药护理);
        /// <summary>
        /// 护理美容
        /// </summary>
        public static readonly Industry NursingOrCosmetology = new Industry(23, "护理美容", PrimaryIndustryEnum.医药护理);
        /// <summary>
        /// 保健与卫生
        /// </summary>
        public static readonly Industry HealthCare = new Industry(24, "保健与卫生", PrimaryIndustryEnum.医药护理);
        /// <summary>
        /// 汽车相关
        /// </summary>
        public static readonly Industry Car = new Industry(25, "汽车相关", PrimaryIndustryEnum.交通工具);
        /// <summary>
        /// 摩托车相关
        /// </summary>
        public static readonly Industry Motorcycle = new Industry(26, "摩托车相关", PrimaryIndustryEnum.交通工具);
        /// <summary>
        /// 火车相关
        /// </summary>
        public static readonly Industry Train = new Industry(27, "火车相关", PrimaryIndustryEnum.交通工具);
        /// <summary>
        /// 飞机相关
        /// </summary>
        public static readonly Industry Plane = new Industry(28, "飞机相关", PrimaryIndustryEnum.交通工具);
        /// <summary>
        /// 建筑
        /// </summary>
        public static readonly Industry Building = new Industry(29, "建筑", PrimaryIndustryEnum.房地产);
        /// <summary>
        /// 物业
        /// </summary>
        public static readonly Industry PropertyManagement = new Industry(30, "物业", PrimaryIndustryEnum.房地产);
        /// <summary>
        /// 消费品
        /// </summary>
        public static readonly Industry ConsumerGoods = new Industry(31, "消费品", PrimaryIndustryEnum.消费品);
        /// <summary>
        /// 法律
        /// </summary>
        public static readonly Industry Law = new Industry(32, "法律", PrimaryIndustryEnum.商业服务);
        /// <summary>
        /// 会展
        /// </summary>
        public static readonly Industry ConferenceOrExhibition = new Industry(33, "会展", PrimaryIndustryEnum.商业服务);
        /// <summary>
        /// 中介服务
        /// </summary>
        public static readonly Industry Agency = new Industry(34, "中介服务", PrimaryIndustryEnum.商业服务);
        /// <summary>
        /// 认证
        /// </summary>
        public static readonly Industry Authentication = new Industry(35, "认证", PrimaryIndustryEnum.商业服务);
        /// <summary>
        /// 审计
        /// </summary>
        public static readonly Industry Audit = new Industry(36, "审计", PrimaryIndustryEnum.商业服务);
        /// <summary>
        /// 传媒
        /// </summary>
        public static readonly Industry Media = new Industry(37, "传媒", PrimaryIndustryEnum.文体娱乐);
        /// <summary>
        /// 体育
        /// </summary>
        public static readonly Industry Sport = new Industry(38, "体育", PrimaryIndustryEnum.文体娱乐);
        /// <summary>
        /// 娱乐休闲
        /// </summary>
        public static readonly Industry AmusementOrLeisure = new Industry(39, "娱乐休闲", PrimaryIndustryEnum.文体娱乐);
        /// <summary>
        /// 印刷
        /// </summary>
        public static readonly Industry Press = new Industry(40, "印刷", PrimaryIndustryEnum.印刷);
        /// <summary>
        /// 其它
        /// </summary>
        public static readonly Industry Other = new Industry(41, "其它", PrimaryIndustryEnum.其它);

        /// <summary>
        /// 已知的行业
        /// </summary>
        public static readonly Industry[] Industries = new Industry[] {
            InternetOrElectronicBusiness,SoftwareOrService,HardwareOrDevice,ElectronicTechnique,CommunicationOrISP,OnlineGame,
            Bank,FundFinanceOrTrust,Insurance,
            Catering,
            Hotel,Tour,
            Express,Logistics,Storage,
            Training,Academy,AcademicResearch,TrafficPolice,Museum,PublicServiceOrNonprofitOrganization,
            MedicineOrMedical,NursingOrCosmetology,HealthCare,
            Car,Motorcycle,Train,Plane,
            Building,PropertyManagement,
            ConsumerGoods,
            Law,ConferenceOrExhibition,Agency,Authentication,Audit,
            Media,Sport,AmusementOrLeisure,
            Press,
            Other };
    }
}
