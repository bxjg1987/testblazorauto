using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace ZLJ.Admin.CoreRCL
{
    /// <summary>
    /// 控制台首页
    /// </summary>
    public partial class Home
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName = "";

        /// <summary>
        /// 用户姓名首字
        /// </summary>
        string UserInitial = "";

        /// <summary>
        /// 问候语
        /// </summary>
        string Greeting = "";

        /// <summary>
        /// 今日信息
        /// </summary>
        string TodayInfo = "";

        /// <summary>
        /// 当前趋势Tab
        /// </summary>
        string currentTrendTab = "daily";

        /// <summary>
        /// 趋势图表OptionRaw
        /// </summary>
        string TrendOptionRaw = "";

        /// <summary>
        /// 操作类型分布图表OptionRaw
        /// </summary>
        string TypeOptionRaw = "";

        /// <summary>
        /// 活跃用户排行图表OptionRaw
        /// </summary>
        string UserOptionRaw = "";

        /// <summary>
        /// 快捷导航项
        /// </summary>
        List<QuickNavItem> QuickNavItems = new();

        /// <summary>
        /// 待办事项
        /// </summary>
        List<TodoItem> TodoItems = new();

        /// <summary>
        /// 最近动态
        /// </summary>
        List<RecentActivity> RecentActivities = new();

        /// <summary>
        /// 按日趋势数据
        /// </summary>
        List<string> dailyDates = new();

        List<int> dailyValues = new();

        /// <summary>
        /// 按月趋势数据
        /// </summary>
        List<string> monthlyDates = new();

        List<int> monthlyValues = new();

        protected override void OnInitialized()
        {
            InitUserInfo();
            InitQuickNav();
            InitMockData();
            BuildChartOptions();
        }

        /// <summary>
        /// 初始化用户信息
        /// </summary>
        void InitUserInfo()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 6) Greeting = "凌晨好";
            else if (hour < 9) Greeting = "早上好";
            else if (hour < 12) Greeting = "上午好";
            else if (hour < 14) Greeting = "中午好";
            else if (hour < 18) Greeting = "下午好";
            else Greeting = "晚上好";

            var dayOfWeek = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Tuesday => "星期二",
                DayOfWeek.Wednesday => "星期三",
                DayOfWeek.Thursday => "星期四",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                DayOfWeek.Sunday => "星期日",
                _ => ""
            };
            TodayInfo = $"今天是 {DateTime.Now:yyyy年MM月dd日} {dayOfWeek}";

            if (currentLoginInformations?.User != null)
            {
                UserName = (currentLoginInformations.User.Surname ?? "") + (currentLoginInformations.User.Name ?? "");
                if (string.IsNullOrEmpty(UserName))
                    UserName = currentLoginInformations.User.UserName ?? "用户";
                UserInitial = UserName.Length > 0 ? UserName[..1] : "U";
            }
            else
            {
                UserName = "用户";
                UserInitial = "U";
            }
        }

        /// <summary>
        /// 初始化快捷导航
        /// </summary>
        void InitQuickNav()
        {
            QuickNavItems = new List<QuickNavItem>
            {
                new() { Title = "组织机构", Icon = "apartment", Color = "#1890ff", Url = "/organization-unit" },
                new() { Title = "岗位管理", Icon = "team", Color = "#52c41a", Url = "/post" },
                new() { Title = "员工档案", Icon = "user", Color = "#faad14", Url = "/employee" },
                new() { Title = "来往单位", Icon = "bank", Color = "#722ed1", Url = "/related-company" },
                new() { Title = "数据字典", Icon = "table", Color = "#13c2c2", Url = "/data-dictionary" },
                new() { Title = "通知中心", Icon = "notification", Color = "#eb2f96", Url = "/notification" },
                new() { Title = "审计日志", Icon = "history", Color = "#fa541c", Url = "/auditing" },
                new() { Title = "系统设置", Icon = "setting", Color = "#2f54eb", Url = "/settings" }
            };
        }

        /// <summary>
        /// 初始化模拟数据
        /// </summary>
        void InitMockData()
        {
            var random = new Random();

            for (int i = 29; i >= 0; i--)
            {
                dailyDates.Add(DateTime.Now.AddDays(-i).ToString("MM-dd"));
                dailyValues.Add(random.Next(20, 80));
            }

            for (int i = 11; i >= 0; i--)
            {
                monthlyDates.Add(DateTime.Now.AddMonths(-i).ToString("yyyy-MM"));
                monthlyValues.Add(random.Next(500, 2000));
            }

            TodoItems = new List<TodoItem>
            {
                new() { Title = "审核新增员工信息", Completed = false, Time = "今天" },
                new() { Title = "更新组织架构配置", Completed = false, Time = "今天" },
                new() { Title = "处理来往单位变更申请", Completed = true, Time = "昨天" },
                new() { Title = "检查数据字典完整性", Completed = false, Time = "本周" },
                new() { Title = "审查系统权限分配", Completed = false, Time = "本周" }
            };

            RecentActivities = new List<RecentActivity>
            {
                new() { UserName = "张三", Content = "新增了员工档案", Time = "10分钟前" },
                new() { UserName = "李四", Content = "修改了组织机构信息", Time = "30分钟前" },
                new() { UserName = "王五", Content = "更新了来往单位资料", Time = "1小时前" },
                new() { UserName = "赵六", Content = "导出了审计日志", Time = "2小时前" },
                new() { UserName = "钱七", Content = "修改了系统设置", Time = "3小时前" },
                new() { UserName = "孙八", Content = "新增了数据字典项", Time = "昨天" },
                new() { UserName = "周九", Content = "分配了角色权限", Time = "昨天" },
                new() { UserName = "吴十", Content = "创建了新岗位", Time = "2天前" }
            };
        }

        /// <summary>
        /// 构建图表OptionRaw
        /// </summary>
        void BuildChartOptions()
        {
            BuildTrendOptionRaw();
            BuildTypeOptionRaw();
            BuildUserOptionRaw();
        }

        /// <summary>
        /// 构建趋势图表OptionRaw
        /// </summary>
        void BuildTrendOptionRaw()
        {
            if (currentTrendTab == "daily")
            {
                var option = new
                {
                    tooltip = new { trigger = "axis" },
                    grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true },
                    xAxis = new { type = "category", boundaryGap = false, data = dailyDates },
                    yAxis = new { type = "value" },
                    series = new[]
                    {
                        new { name = "操作数", type = "line", smooth = true, areaStyle = new { }, itemStyle = new { color = "#1890ff" }, data = dailyValues }
                    }
                };
                TrendOptionRaw = JsonSerializer.Serialize(option);
            }
            else
            {
                var option = new
                {
                    tooltip = new { trigger = "axis" },
                    grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true },
                    xAxis = new { type = "category", data = monthlyDates },
                    yAxis = new { type = "value" },
                    series = new[]
                    {
                        new { name = "操作数", type = "bar", itemStyle = new { color = "#1890ff" }, data = monthlyValues }
                    }
                };
                TrendOptionRaw = JsonSerializer.Serialize(option);
            }
        }

        /// <summary>
        /// 构建操作类型分布图表OptionRaw
        /// </summary>
        void BuildTypeOptionRaw()
        {
            var random = new Random();
            var typeData = new[]
            {
                new { name = "用户管理", value = random.Next(100, 300) },
                new { name = "组织机构", value = random.Next(80, 200) },
                new { name = "员工档案", value = random.Next(60, 180) },
                new { name = "来往单位", value = random.Next(40, 150) },
                new { name = "数据字典", value = random.Next(30, 100) },
                new { name = "系统设置", value = random.Next(20, 80) }
            };
            var option = new
            {
                tooltip = new { trigger = "item", formatter = "{b}: {c} ({d}%)" },
                series = new[]
                {
                    new { type = "pie", radius = new[] { "40%", "70%" }, avoidLabelOverlap = false, label = new { show = true, formatter = "{b}\n{d}%" }, data = typeData }
                }
            };
            TypeOptionRaw = JsonSerializer.Serialize(option);
        }

        /// <summary>
        /// 构建活跃用户排行图表OptionRaw
        /// </summary>
        void BuildUserOptionRaw()
        {
            var random = new Random();
            var userNames = new[] { "张三", "李四", "王五", "赵六", "钱七", "孙八", "周九", "吴十" };
            var activeUserData = new List<int>();
            var activeUserNames = new List<string>();
            for (int i = userNames.Length - 1; i >= 0; i--)
            {
                activeUserNames.Add(userNames[i]);
                activeUserData.Add(random.Next(50, 300));
            }
            var option = new
            {
                tooltip = new { trigger = "axis" },
                grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true },
                xAxis = new { type = "value" },
                yAxis = new { type = "category", data = activeUserNames },
                series = new[]
                {
                    new { type = "bar", itemStyle = new { color = "#52c41a" }, data = activeUserData }
                }
            };
            UserOptionRaw = JsonSerializer.Serialize(option);
        }

        /// <summary>
        /// 趋势Tab切换事件
        /// </summary>
        void OnTrendTabChanged(string value)
        {
            currentTrendTab = value ?? "daily";
            BuildTrendOptionRaw();
        }

        /// <summary>
        /// 导航到指定页面
        /// </summary>
        void NavTo(string url)
        {
            NavManager.NavigateTo(url);
        }
    }

    /// <summary>
    /// 快捷导航项
    /// </summary>
    public class QuickNavItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 待办事项
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
    }

    /// <summary>
    /// 最近动态
    /// </summary>
    public class RecentActivity
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
    }
}
