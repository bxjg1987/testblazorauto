using Microsoft.AspNetCore.Components.Sections;

namespace AntDesign;
public static class ModalServiceExtensions
{
    public static ModalRef ShowAssociatedCompanyDetailUpdateModal(this ModalService modalService,long sbid) {
        // 创建弹窗配置
        var opt = new ModalOptions
        {
            Title = "设置",
            Width = "55vw",
            //必须释放，因为弹窗内部使用了section固定字符串
            DestroyOnClose = true,
            //点击遮罩层不关闭弹窗
            MaskClosable = true, 
            //Closable=true, 
          //  MaxBodyHeight ="86vh"
            //Style = "top: 20px"
        };

        ////默认的处理就是关闭当前弹窗
        //if (onOk != default)
        //{
        //    var wt = opt.OnOk;
        //    opt.OnOk = async ary =>
        //    {
        //        await wt(ary);
        //        await onOk();
        //    };
        //}
        var tag = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(opt);// t.GetHashCode();

        RenderFragment title = builder =>
        {
            builder.OpenComponent<SectionOutlet>(0);
            builder.AddAttribute(10, "SectionName", $"{tag}-detail-update-title");
            builder.CloseComponent();

            //builder.OpenComponent<GaiJishuForm>(50);
            //// 设置服务器ID
            //builder.AddAttribute(60, "InstanceId", sbid);
            //builder.AddAttribute(70, "Master", tag);
            ////添加OnSuccess事件处理程序
            ////builder.AddAttribute(70, "OnSuccess", EventCallback.Factory.Create(opt, () => opt.OnOk(null)));
            //builder.CloseComponent();


        };


        RenderFragment content = builder =>
        {
            builder.OpenComponent<SectionOutlet>(0);
            builder.AddAttribute(10, "SectionName", $"{tag}-detail-update-body");
            builder.CloseComponent();

            builder.OpenComponent<ZLJ.Admin.CoreRCL.AssociatedCompany. DetailUpdate>(50);
            // 设置服务器ID
            builder.AddAttribute(60, "Id", sbid);
            builder.AddAttribute(70, "Master", tag);
            //添加OnSuccess事件处理程序
            //builder.AddAttribute(70, "OnSuccess", EventCallback.Factory.Create(opt, () => opt.OnOk(null)));
            builder.CloseComponent();


        };

        RenderFragment footer = builder =>
        {
            builder.OpenComponent<SectionOutlet>(0);
            builder.AddAttribute(10, "SectionName", $"{tag}-detail-update-btns");
            builder.CloseComponent();

            //添加关闭按钮
            builder.OpenComponent<Button>(20);
            builder.AddAttribute(30, "OnClick", EventCallback.Factory.Create(opt, opt.OnCancel));
            builder.AddAttribute(40, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.AddContent(0, "关闭");
            }));
            builder.CloseComponent();
        };

        opt.Content = content;
        opt.Footer =  footer;
        opt.TitleTemplate = title;
        //opt.tit

        //modalService.ShowFwqEditModal
        // 创建弹窗并返回引用
        return modalService.CreateModal(opt);
    }
}
