using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Organizations.Dto;
using Abp.UI;
using ZLJ.Authorization;
using ZLJ.Localization;
using ZLJ.BaseInfo;
using Abp.Domain.Entities;
using BXJG.GeneralTree;
using Newtonsoft.Json;
using Abp.Linq;
using System.Dynamic;

namespace ZLJ.Organizations
{
    public class OrganizationUnitAppService : ZLJAppServiceBase, IOrganizationUnitAppService
    {
        protected string allTextForManager = "All", allTextForSearch = "no limit", allTextForForm = "Please select";

        protected readonly OrganizationUnitManager organizationUnitManager;
        protected readonly IRepository<OrganizationUnit, long> ownRepository;

        public OrganizationUnitAppService(
            IRepository<OrganizationUnit, long> repository,
            OrganizationUnitManager organizationUnitManager)//这里的字符串后期可以使用常量
        {
            this.organizationUnitManager = organizationUnitManager;
            this.ownRepository = repository;
        }

        public virtual async Task<OrganizationUnitDto> CreateAsync(EditOrganizationUnitDto input)
        {
            if (input.ParentId == 0)
                input.ParentId = null;

            var m = ObjectMapper.Map<OrganizationUnitEntity>(input);

            await organizationUnitManager.CreateAsync(m);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<OrganizationUnitDto>(m);
        }
        public virtual async Task<OrganizationUnitDto> MoveAsync(MoveInput input)
        {
            if (input.TargetId == 0)
                input.TargetId = null;
            await organizationUnitManager.MoveAsync(input.Id, input.TargetId);
            await base.CurrentUnitOfWork.SaveChangesAsync();//事务提交成功后才能返回
            var m = await ownRepository.GetAsync(input.Id) as OrganizationUnitEntity;
            return ObjectMapper.Map<OrganizationUnitDto>(m);// m.MapTo<OrganizationUnitDto>();
        }
        public virtual async Task<OrganizationUnitDto> UpdateAsync(EditOrganizationUnitDto input)
        {
            if (input.ParentId == 0)
                input.ParentId = null;

            await this.organizationUnitManager.MoveAsync(input.Id, input.ParentId);//如果猜的没错的话 内部的处理逻辑是：若目标父节点与现在父节点id相同时不会再移动

            //_organizationUnitManager本身就存在上下文了(基类泛型仓储)，如果直接又附加会报错
            var m = await ownRepository.GetAsync(input.Id) as OrganizationUnitEntity;
            ObjectMapper.Map<EditOrganizationUnitDto, OrganizationUnitEntity>(input, m); //input.MapTo(m); 扩展即静态，不便于测试


            await base.CurrentUnitOfWork.SaveChangesAsync();//事务提交成功后才能返回
            return ObjectMapper.Map<OrganizationUnitDto>(m);
        }
        public virtual async Task DeleteAsync(EntityDto<long> input)
        {
            await this.organizationUnitManager.DeleteAsync(input.Id);
        }
        public virtual async Task<OrganizationUnitDto> GetAsync(EntityDto<long> input)
        {
            var entity = await ownRepository.GetAsync(input.Id) as OrganizationUnitEntity;

            return ObjectMapper.Map<OrganizationUnitDto>(entity);

        }

        //如果没有上级节点包装 列表默认被选择了 点击新增时 上级几点无法定位到null，因此需要做个包装
        public virtual async Task<IList<OrganizationUnitDto>> GetAllListAsync(GeneralTreeGetTreeInput input)
        {
            //获取父节点的code 方便后续查询所有子集
            string parentCode = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await ownRepository.SingleAsync(c => c.Id == input.ParentId.Value);
                parentCode = top.Code;
            }

            //查询
            var list = await AsyncQueryableExecuter.ToListAsync(ownRepository.GetAll().Where(c => c.Code.StartsWith(parentCode)).OrderBy(c => c.Code));
          // var listTemp = list.Select(c => c as OrganizationUnitEntity);

            //建立dto以及处理父子关系
            //OrganizationUnitEntity parent = list.SingleOrDefault(c => c.Id == input.ParentId);
            //if (parent != null)
            //    list.Remove(parent);

            var list1 = ObjectMapper.Map<IList<OrganizationUnitDto>>(list);//映射排除children

            foreach (var c in list1)
            {
                c.Children = list1.Where(d => d.ParentId == c.Id).ToList();
                var entity = list.Single(d => d.Id == c.Id);

                if (c.Children != null && c.Children.Count > 0)
                    c.State = "closed";//默认值为 open


            }


            var parenOrganizationUnitDto = input.ParentId.HasValue ? list1.SingleOrDefault(c => c.ParentId == input.ParentId) : null;
            //得到顶级节点集合
            list1 = list1.Where(c => c.ParentId == input.ParentId).ToList();

            //处理顶级文本
            if (input.LoadParent)
            {
                if (!string.IsNullOrWhiteSpace(input.ParentText))
                    return new List<OrganizationUnitDto> { new OrganizationUnitDto { DisplayName = L(input.ParentText), Children = list1 } };

                if (input.ParentId.HasValue)
                    return new List<OrganizationUnitDto> { parenOrganizationUnitDto };

                return new List<OrganizationUnitDto> { new OrganizationUnitDto { DisplayName = L(allTextForManager), Children = list1 } };
            }
            return list1;
        }
        public virtual async Task<IList<GeneralTreeNodeDto>> GetTreeForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            //得到实体扁平集合
            string parentCode = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await ownRepository.GetAsync(input.ParentId.Value);
                parentCode = top.Code;
            }
            var query = ownRepository.GetAll()
                .Where(c => c.Code.StartsWith(parentCode))
                .OrderBy(c => c.Code);
            //可以调用虚方法简介给子类一个机会做过滤，暂未实现
            var list = await AsyncQueryableExecuter.ToListAsync(query);//.ToListAsync();


            //先踢出父节点
            //OrganizationUnitEntity parent = list.SingleOrDefault(c => c.Id == input.Id);
            //if (parent != null)
            //    list.Remove(parent);

            //转换为Dtop
            //上面没有直接用投影是为了给子类一个机会来参与遍历过程
            var dtoList = new List<GeneralTreeNodeDto>();
            foreach (var c in list)
            {
                var temp = new GeneralTreeNodeDto
                {
                    id = c.Id.ToString(),
                    parentId = c.ParentId.ToString(),
                    text = c.DisplayName
                };
                temp.attributes = new ExpandoObject();
                temp.attributes.code = c.Code;//这里可以搞个虚方法，允许子类填充自己的字段
                dtoList.Add(temp);
            }
            dtoList.ForEach(c =>
            {
                c.children = dtoList.Where(d => d.parentId == c.id).ToList();

                var entity = list.Single(d => d.Id.ToString() == c.id);

                if (c.children != null && c.children.Count > 0)
                    c.state = "closed";//默认值为 open


            });

            var parenOrganizationUnitDto = input.ParentId.HasValue ? dtoList.SingleOrDefault(c => c.id == input.ParentId.ToString()) : null;

            if (input.ParentId.HasValue)
                dtoList = dtoList.Where(c => c.parentId == input.ParentId.ToString()).ToList();
            else
                dtoList = dtoList.Where(c => string.IsNullOrWhiteSpace(c.parentId)).ToList();

            if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
                return new List<GeneralTreeNodeDto> { new GeneralTreeNodeDto { id = null, text = L(input.ParentText), children = dtoList } };

            if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
                return new List<GeneralTreeNodeDto> { parenOrganizationUnitDto };

            if (input.ForType == 1 || input.ForType == 2)
                return new List<GeneralTreeNodeDto> { new GeneralTreeNodeDto { id = null, text = L(this.allTextForSearch), children = dtoList } };

            if (input.ForType == 3 || input.ForType == 4)
                return new List<GeneralTreeNodeDto> { new GeneralTreeNodeDto { id = null, text = L(this.allTextForForm), children = dtoList } };

            return dtoList;
        }
        public virtual async Task<IList<ComboboxItemDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {

            var query = ownRepository.GetAll()
                 .Where(c => c.ParentId == input.ParentId || c.Id == input.ParentId)
                 .OrderBy(c => c.Code);

            var dtoList = await AsyncQueryableExecuter.ToListAsync(query.Select(c => new ComboboxItemDto { DisplayText = c.DisplayName, Value = c.Id.ToString() }));
            var parenOrganizationUnitDto = input.ParentId.HasValue ? dtoList.SingleOrDefault(c => c.Value == input.ParentId.ToString()) : null;
            if (parenOrganizationUnitDto != null)
            {
                dtoList.Remove(parenOrganizationUnitDto);
                parenOrganizationUnitDto.Value = null;
                parenOrganizationUnitDto.DisplayText = "==" + parenOrganizationUnitDto.DisplayText + "==";
            }
            //dtoList = dtoList.Where(c => c.Value != input.Id).ToList();

            if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
                dtoList.Insert(0, new ComboboxItemDto { Value = null, DisplayText = L(input.ParentText) });
            else if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
                dtoList.Insert(0, parenOrganizationUnitDto);
            else if (input.ForType == 1 || input.ForType == 2)
                dtoList.Insert(0, new ComboboxItemDto { Value = null, DisplayText = L(allTextForSearch) });
            else if (input.ForType == 3 || input.ForType == 4)
                dtoList.Insert(0, new ComboboxItemDto { Value = null, DisplayText = L(allTextForForm) });

            return dtoList;
        }
    }
}
