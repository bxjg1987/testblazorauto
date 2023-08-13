abp默认提供了一些dto，我们在此基础上做了进一步扩展，下面逐一说明。

# BXJG.Common.Dto
不需要继承abp中dto类 和 不需要实现abp dto接口的 我们放在这里的

## BatchXXXInput
批量操作时的输入模型，里面包含Ids属性，表示要批量操作的数据的id列表。
顶层父类是泛型的，针对常用类型定义了相应子类，如：BatchAuditInputLong、BatchAuditInputGuid

## BatchXXXOut
跟BatchXXXInput类似，只不过这个表示批量操作的输出模型

## ConditionFieldDefine
动态条件定义，参考“动态条件.md”

## ~~GetForSelectInput~~
此类好像意义不大了，由于之前的项目有引用，懒得动。
abp只提供了普通的crudAppservice，我们专门针对可选数据（弹窗或下拉框的数据）定义了单独的应用服务，这种服务接口的输入模型可以继承GetForSelectInput


# BXJG.Utils.Application
需要引用abp类或接口的放这里的

## ~~GetPageForSelectInput~~
此类好像意义不大了，由于之前的项目有引用，懒得动。
意义跟GetForSelectInput类似，只是加了分页排序

## SortedResultRequest
若不分页，只需要排序且条件和排序在一个类中的查询输入模型可以继承它。

## SortedResultRequest<TFilter>
需要排序，且包含独立条件的输入模型，但不做分页。具体分析看PagedAndSortedResultRequest<TFilter>

## PagedAndSortedResultRequest<TFilter>
abp默认有个PagedAndSortedResultRequestDto，通常查询输入模型继承它，
但我们的条件往往需要服用，比如查询列表时，使用的条件，跟统计查询使用的条件可能一样，有时候我们还希望查询符合条件的数量，而不返回真实的数据。

>动态条件本就是动态的，不存在上面这种需要复用的情况，所以动态条件可以直接放输入模型上，而无需在输入模型上单独定义个条件属性，所以查询输入模型可以直接实现IEnurable《动态条件定义》
>详细的参考 动态条件的文档


# 通用树
参考 通用树的 文档