using System;
using Innovare.Shop.Data.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovare.Shop.Data.Abstractions
{
    public abstract class SqlServerEntityTypeConfiguration<TModel> : IEntityTypeConfiguration<TModel>
            where TModel : InnovareShopModelBase
    {
        private const string _typePostfix = "EntityTypeConfiguration";
        private readonly Type _type;

        protected SqlServerEntityTypeConfiguration(string tableComment)
        {
            _type = GetType();

            ValidateTypeName();

            TableName = _type.Name.Replace(_typePostfix, string.Empty);
            TableComment = tableComment ?? throw new ArgumentNullException(nameof(tableComment));
        }

        protected string TableName { get; }

        protected string TableComment { get; }

        public void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder
                .ToTable(TableName)
                .HasComment(TableComment);

            builder
                .HasKey(model => model.Id);

            builder
                .Property(model => model.Id)
                .IsRequired()
                .HasDefaultValueSql("NEWID()")
                .HasComment(Comments.Id);

            builder
                .Property(model => model.RowVersion)
                .IsRowVersion()
                .HasComment(Comments.RowVersion);

            ConfigureEntityType(builder);
        }

        protected virtual void ConfigureEntityType(EntityTypeBuilder<TModel> builder)
        {
        }

        private void ValidateTypeName()
        {
            var isValid = _type.Name.EndsWith(_typePostfix);

            if (!isValid)
            {
                throw new InvalidOperationException(string.Format(Messages.EntityTypeConfigurationName, _type.FullName));
            }
        }
    }
}