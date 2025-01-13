namespace ManagementSystem.Api.Common.Domain;

public interface IHasConcurrencyToken
{
    byte[] RowVersion { get; set; }
}