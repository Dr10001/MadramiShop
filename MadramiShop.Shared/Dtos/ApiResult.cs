namespace MadramiShop.Shared.Dtos;

public record ApiResult(bool IsSuccess, string? Error)
{
    public static ApiResult Success() => new(true, null);
    public static ApiResult Fail(string errorMessage) => new(false, errorMessage);
}
public record ApiResult<Tdata>(bool IsSuccess, Tdata data, string? Error)
{
    public static ApiResult<Tdata> Success(Tdata data) => new(true, data, null);
    public static ApiResult<Tdata> Fail(string errorMessage) => new(false, default!, errorMessage);
}