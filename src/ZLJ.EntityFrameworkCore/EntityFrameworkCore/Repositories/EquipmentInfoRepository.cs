using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Asset;

namespace ZLJ.EntityFrameworkCore.Repositories
{
    public class EquipmentInfoRepository : ZLJRepositoryBase<EquipmentInfoEntity, long>, IEquipmentInfoRepository
    {
        public EquipmentInfoRepository(IDbContextProvider<ZLJDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<EquipmentInfoEntity>> GetEnclosureAsync(double latitude, double longitude, double distance)
        {
            //暂时不考虑租户的问题
            //这里的sql只是作为最终sql的一部分， 要加top 100 percent  否则报错
            //order by没啥效果
            //return base.Context.EquipmentInfos.FromSqlInterpolated(@$"select top 100 percent * from ( 
            //select *, geography::Point(Latitude, Longitude, 4326).STDistance(geography::Point({latitude}, {longitude}, 4326)) distance from ZLJEquipmentInfos
            //) t where t.distance<={distance} order by t.distance asc").Include(c=>c.Area).ToListAsync();

            var cmd = base.Connection.CreateCommand();
            if (base.Transaction != null)
                cmd.Transaction = base.Transaction;
            cmd.CommandText = @$"select * from ( 
            select a.*, b.displayName areaDisplayName, geography::Point(Latitude, Longitude, 4326).STDistance(geography::Point({latitude}, {longitude}, 4326)) distance from ZLJEquipmentInfos a
            left join BXJGGeneralTrees b on a.AreaId=b.id 
            where a.TenantId={UnitOfWorkManager.Current.GetTenantId()} and a.isdeleted=0
            ) t where t.distance is not null and t.distance<={distance} order by t.distance";
            var list = new List<EquipmentInfoEntity>();
            using (var dr = await cmd.ExecuteReaderAsync(base.CancellationTokenProvider.Token))
            {
                while (await dr.ReadAsync(base.CancellationTokenProvider.Token))
                {
                    var m = new EquipmentInfoEntity
                    {
                        AreaId = dr["AreaId"] != DBNull.Value ? Convert.ToInt64(dr["AreaId"]) : 0,
                        Area = new BXJG.GeneralTree.GeneralTreeEntity
                        {
                            Id = dr["AreaId"] != DBNull.Value ? Convert.ToInt64(dr["AreaId"]) : 0,
                            DisplayName = dr["areaDisplayName"] != DBNull.Value ? dr["areaDisplayName"].ToString() : ""
                        },
                        Code = dr["Code"].ToString(),
                        Distance = dr["Distance"] != DBNull.Value ? Convert.ToDouble(dr["Distance"]) : 0,
                        Id = Convert.ToInt64(dr["Id"]),
                        Latitude = dr["Latitude"] != DBNull.Value ? Convert.ToDouble(dr["Latitude"]) : 0,
                        Longitude = dr["Longitude"] != DBNull.Value ? Convert.ToDouble(dr["Longitude"]) : 0,
                        Size = dr["Size"] != DBNull.Value ? dr["Size"].ToString() : "",
                        CreationTime = dr["CreationTime"] != DBNull.Value ? Convert.ToDateTime(dr["CreationTime"]) : default,
                        CreatorUserId = dr["CreatorUserId"] != DBNull.Value ? Convert.ToInt64(dr["CreationTime"]) : default
                    };
                    list.Add(m);
                }
            }
            return list;
        }
    }
}
