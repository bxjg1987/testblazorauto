namespace ZLJ.App.Admin.BaseInfo.Dto
{
    public class ComboboxInputDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Q { get; set; }

        /// <summary>
        /// 选中项
        /// </summary>
        public long? SelectedId { get; set; }
    }
}