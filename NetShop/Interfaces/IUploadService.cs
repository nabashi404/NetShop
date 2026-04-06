using FluentResults;
using Microsoft.AspNetCore.Components.Forms;
using NetShop.Models;

namespace NetShop.Interfaces;

public interface IUploadService
{
    Task<Result<UploadResponse>> UploadImageAsync(IBrowserFile file);
}
