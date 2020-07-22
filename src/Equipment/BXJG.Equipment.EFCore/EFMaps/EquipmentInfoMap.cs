using BXJG.Equipment;
using BXJG.Equipment.EquipmentInfo;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment
{
    public class EquipmentInfoMap<TEntity, TDataDictionary> : IEntityTypeConfiguration<TEntity>
        where TEntity : EquipmentInfoEntity<TDataDictionary>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            builder.Property(c => c.Name).HasMaxLength(BXJGEquipmentConst.EquipmentInfoNameMaxLength).IsRequired();
            builder.Property(c => c.Longitude).IsRequired();
            builder.Property(c => c.Latitude).IsRequired();
        }
    }

    public class EquipmentInfoMap<TDataDictionary> : EquipmentInfoMap<EquipmentInfoEntity<TDataDictionary>, TDataDictionary>
        , IEntityTypeConfiguration<EquipmentInfoEntity<TDataDictionary>>
    { }

    //经过测试这里不能继承EquipmentInfoMap<TDataDictionary>，因为IEntityTypeConfiguration接口不是泛型协变的
    public class EquipmentInfoMap : EquipmentInfoMap<EquipmentInfoEntity, GeneralTreeEntity>, IEntityTypeConfiguration<EquipmentInfoEntity>
    {}
}
