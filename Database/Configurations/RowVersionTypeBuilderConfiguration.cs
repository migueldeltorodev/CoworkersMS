using ManagementSystem.Api.Common.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagementSystem.Api.Database.Configurations;

public static class RowVersionTypeBuilderConfiguration
{
    public static void ConfigureRowVersion<T>(this EntityTypeBuilder<T> builder) where T : class, IHasConcurrencyToken
    {
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}