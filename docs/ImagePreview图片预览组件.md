# ImagePreview 图片预览组件

## 概述

ImagePreview 是基于 Ant Design Blazor Image 组件封装的图片预览组件，支持单张图片预览和多张图片相册模式。

## 功能特性

1. **自动生成图片URL**：使用 FileHelper 自动生成缩略图和大图的 URL
2. **单张图片预览**：支持单张图片的点击预览
3. **多张图片相册模式**：多张图片时自动使用相册模式，支持左右切换预览
4. **懒加载大图**：默认只加载缩略图，点击预览时才加载大图
5. **灵活配置**：支持自定义宽度、预览开关等配置

## 使用方法

### 基本用法

```razor
<ImagePreview FileIds="@fileIds" />
```

### 单张图片

```razor
@code {
    private List<Guid> fileIds = new() { Guid.Parse("your-file-id") };
}

<ImagePreview FileIds="@fileIds" />
```

### 多张图片（相册模式）

```razor
@code {
    private List<Guid> fileIds = new() 
    { 
        Guid.Parse("file-id-1"),
        Guid.Parse("file-id-2"),
        Guid.Parse("file-id-3")
    };
}

<ImagePreview FileIds="@fileIds" />
```

### 自定义宽度

```razor
<ImagePreview FileIds="@fileIds" Width="200px" />
```

### 禁用预览

```razor
<ImagePreview FileIds="@fileIds" Preview="false" />
```

### 自定义点击事件

```razor
<ImagePreview FileIds="@fileIds" OnClick="HandleImageClick" />

@code {
    private async Task HandleImageClick(MouseEventArgs args)
    {
        // 自定义点击处理逻辑
    }
}
```

## 组件参数

| 参数 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| FileIds | List\<Guid\> | new() | 文件ID列表 |
| Width | string? | null | 图片宽度 |
| Preview | bool | true | 是否启用预览功能 |
| OnClick | EventCallback\<MouseEventArgs\> | - | 图片点击回调 |

## 实现原理

1. **单张图片模式**：
   - 显示缩略图（使用 `FileHelper.BuildDownloadUrlThum`）
   - 预览时加载大图（使用 `FileHelper.BuildDownloadUrl`）
   - 支持点击预览

2. **多张图片模式**：
   - 显示第一张图片的缩略图
   - 使用 `ImagePreviewGroup` 组件实现相册功能
   - 点击后可左右切换预览所有图片
   - 所有大图在预览时才加载

## 注意事项

1. 组件依赖 `FileHelper`，需要确保已正确注入
2. 文件ID 必须是有效的 GUID
3. 缩略图和大图的 URL 由 `FileHelper` 自动生成
4. 多张图片时，默认显示第一张图片的缩略图
5. 预览功能默认开启，可通过 `Preview="false"` 关闭

## 相关文档

- [文件和附件](./文件和附件.md) - 文件上传和下载相关说明
- [如何切换新的UI组件库](./如何切换新的UI组件库.md) - UI 组件库集成说明
